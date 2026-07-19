using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Domain.Administration;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Administration;

namespace School.Services.Administration
{
    public class CertificateService : ICertificateService
    {
        private readonly SchoolDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPdfCertificateService _pdfService;

        public CertificateService(SchoolDbContext context, IMapper mapper, IPdfCertificateService pdfService)
        {
            _context = context;
            _mapper = mapper;
            _pdfService = pdfService;
        }

        public async Task<APIResponse<List<CertificateIssuanceLogDto>>> GetCertificatesAsync(CertificateFilterDto filter, int schoolId)
        {
            var query = _context.CertificateIssuanceLogs
                .Where(c => c.SchoolRegistrationId == schoolId && !c.IsDeleted);

            if (!string.IsNullOrEmpty(filter.CertificateType))
                query = query.Where(c => c.CertificateType == filter.CertificateType);
            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(c => c.Status == filter.Status);
            if (filter.StudentId.HasValue)
                query = query.Where(c => c.StudentId == filter.StudentId);
            if (filter.FromDate.HasValue)
                query = query.Where(c => c.IssuedDate >= filter.FromDate);
            if (filter.ToDate.HasValue)
                query = query.Where(c => c.IssuedDate <= filter.ToDate);

            var items = await query.OrderByDescending(c => c.IssuedDate).ToListAsync();
            return new APIResponse<List<CertificateIssuanceLogDto>>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = _mapper.Map<List<CertificateIssuanceLogDto>>(items)
            };
        }

        public async Task<APIResponse<CertificateIssuanceLogDto>> GetCertificateByIdAsync(int id, int schoolId)
        {
            var entity = await _context.CertificateIssuanceLogs
                .FirstOrDefaultAsync(c => c.Id == id && c.SchoolRegistrationId == schoolId && !c.IsDeleted);

            if (entity == null)
                return new APIResponse<CertificateIssuanceLogDto> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Certificate not found" };

            return new APIResponse<CertificateIssuanceLogDto> { Success = true, StatusCode = HttpStatusCode.OK, Data = _mapper.Map<CertificateIssuanceLogDto>(entity) };
        }

        public async Task<APIResponse<CertificateIssuanceLogDto>> IssueCertificateAsync(CreateCertificateIssuanceDto dto, string userId, string userName, int schoolId)
        {
            var entity = _mapper.Map<CertificateIssuanceLog>(dto);
            entity.SchoolRegistrationId = schoolId;
            entity.CertificateNumber = await GenerateCertificateNumber(dto.CertificateType, schoolId);
            entity.IssuedDate = DateTime.UtcNow;
            entity.IssuedByUserId = userId;
            entity.IssuedByName = userName;
            entity.Status = "Issued";
            entity.QrVerificationCode = Guid.NewGuid().ToString("N")[..12].ToUpper();
            entity.PrintCount = 1;
            entity.LastPrintedDate = DateTime.UtcNow;
            entity.CreatedBy = userId;
            entity.CreatedDate = DateTime.UtcNow;

            _context.CertificateIssuanceLogs.Add(entity);
            await _context.SaveChangesAsync();

            return new APIResponse<CertificateIssuanceLogDto> { Success = true, StatusCode = HttpStatusCode.Created, Data = _mapper.Map<CertificateIssuanceLogDto>(entity), Message = "Certificate issued" };
        }

        public async Task<APIResponse<bool>> RevokeCertificateAsync(int id, string reason, int schoolId)
        {
            var entity = await _context.CertificateIssuanceLogs.FirstOrDefaultAsync(c => c.Id == id && c.SchoolRegistrationId == schoolId);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Certificate not found" };

            entity.Status = "Revoked";
            entity.RevocationReason = reason;
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true, Message = "Certificate revoked" };
        }

        public async Task<APIResponse<bool>> IncrementPrintCountAsync(int id, int schoolId)
        {
            var entity = await _context.CertificateIssuanceLogs.FirstOrDefaultAsync(c => c.Id == id && c.SchoolRegistrationId == schoolId);
            if (entity == null)
                return new APIResponse<bool> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Certificate not found" };

            entity.PrintCount++;
            entity.LastPrintedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Data = true };
        }

        public async Task<APIResponse<byte[]>> GeneratePdfAsync(int id, int schoolId)
        {
            var cert = await _context.CertificateIssuanceLogs
                .Where(c => c.Id == id && c.SchoolRegistrationId == schoolId && !c.IsDeleted)
                .FirstOrDefaultAsync();

            if (cert == null)
            {
                return new APIResponse<byte[]> { Success = false, StatusCode = HttpStatusCode.NotFound, Message = "Certificate record not found" };
            }

            var dto = _mapper.Map<CertificateIssuanceLogDto>(cert);

            if (string.IsNullOrEmpty(dto.AcademicYearName))
            {
                var ay = await _context.AcademicYears.FindAsync(cert.AcademicYearId);
                if (ay != null)
                {
                    dto.AcademicYearName = ay.YearName;
                }
            }

            var pdfBytes = await _pdfService.GenerateStudentCertificatePdfAsync(dto, "http://localhost:5000");

            try
            {
                var uploadsFolder = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", "uploads", "certificates");
                if (!System.IO.Directory.Exists(uploadsFolder))
                {
                    System.IO.Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"{cert.CertificateNumber}_{DateTime.UtcNow.Ticks}.pdf";
                var filePath = System.IO.Path.Combine(uploadsFolder, fileName);
                await System.IO.File.WriteAllBytesAsync(filePath, pdfBytes);

                cert.PdfUrl = $"/uploads/certificates/{fileName}";
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Fallback gracefully if filesystem write fails (e.g. read-only environment)
            }

            return new APIResponse<byte[]> { Success = true, StatusCode = HttpStatusCode.OK, Data = pdfBytes };
        }

        private async Task<string> GenerateCertificateNumber(string type, int schoolId)
        {
            var prefix = type switch
            {
                "TC" => "TC",
                "Bonafide" => "BF",
                "Character" => "CC",
                "Migration" => "MC",
                _ => "CERT"
            };
            var count = await _context.CertificateIssuanceLogs.CountAsync(c => c.SchoolRegistrationId == schoolId && c.CertificateType == type);
            return $"{prefix}-{DateTime.UtcNow.Year}-{(count + 1):D4}";
        }
    }
}
