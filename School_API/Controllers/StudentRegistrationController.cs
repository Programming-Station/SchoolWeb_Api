using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School.Models.Student;
using School_DTOs.Student;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace School_API.Controllers
{
    public class StudentRegistrationController : BaseController
    {
        private readonly IStudentRegistrationService _studentRegistrationService;
        private readonly IPdfCertificateService _pdfCertificateService;

        public StudentRegistrationController(
            IStudentRegistrationService studentRegistrationService, 
            IPdfCertificateService pdfCertificateService,
            ICurrentUserService currentUserService)
            : base(currentUserService)
        {
            _studentRegistrationService = studentRegistrationService;
            _pdfCertificateService = pdfCertificateService;
        }

        /// <summary>
        /// Create a new student registration (Public endpoint - no authentication required)
        /// </summary>
        [HttpPost] 
        public async Task<IActionResult> Create([FromBody] StudentRegistrationModel model)
        {
            var response = await _studentRegistrationService.AddStudentRegistrationAsync(model);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Get student registration by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _studentRegistrationService.GetStudentRegistrationByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Get all student registrations with pagination and filters
        /// </summary>
        [HttpGet] 
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? status = null)
        {
            var response = await _studentRegistrationService.GetAllAsync(pageNumber, pageSize, searchTerm, status);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Update student registration
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] StudentRegistrationModel model)
        {
            var response = await _studentRegistrationService.UpdateStudentRegistrationAsync(model);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Update student registration status (Admin only)
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStudentRegistrationStatusDto dto)
        {
            var response = await _studentRegistrationService.UpdateStatusAsync(dto);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Delete student registration (Soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _studentRegistrationService.DeleteStudentRegistrationAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Get student registration by mobile number
        /// </summary>
        [HttpGet("mobile/{mobile}")]
        public async Task<IActionResult> GetByMobile(string mobile)
        {
            var response = await _studentRegistrationService.GetByMobileAsync(mobile);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Get student registration by Aadhaar number
        /// </summary>
        [HttpGet("aadhaar/{aadhaarNumber}")]
        public async Task<IActionResult> GetByAadhaar(string aadhaarNumber)
        {
            var response = await _studentRegistrationService.GetByAadhaarAsync(aadhaarNumber);
            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Verify certificate by ID (Public endpoint - for QR code scanning)
        /// </summary>
        [AllowAnonymous]
        [HttpGet("Verify/{id}")]
        public async Task<IActionResult> VerifyCertificate(int id)
        {
            var response = await _studentRegistrationService.GetStudentRegistrationByIdAsync(id);
            
            if (!response.Success || response.Data == null)
            {
                var notFoundHtml = GenerateNotFoundHtml();
                return Content(notFoundHtml, "text/html", Encoding.UTF8);
            }

            var html = GenerateCertificateDetailsHtml(response.Data);
            return Content(html, "text/html", Encoding.UTF8);
        }

        private string GenerateCertificateDetailsHtml(StudentRegistrationDto registration)
        {
            var issueDate = DateTime.UtcNow;
            var expiryDate = issueDate.AddYears(5);
            
            var html = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Certificate Verification - {registration.FullName}</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 20px;
            min-height: 100vh;
        }}
        .container {{
            max-width: 900px;
            margin: 0 auto;
            background: white;
            border-radius: 10px;
            box-shadow: 0 10px 40px rgba(0,0,0,0.2);
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 30px;
            text-align: center;
        }}
        .header h1 {{
            font-size: 28px;
            margin-bottom: 10px;
        }}
        .header p {{
            font-size: 14px;
            opacity: 0.9;
        }}
        .content {{
            padding: 30px;
        }}
        .verified-badge {{
            background: #10b981;
            color: white;
            padding: 10px 20px;
            border-radius: 5px;
            display: inline-block;
            margin-bottom: 20px;
            font-weight: bold;
        }}
        table {{
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }}
        table th {{
            background: #f3f4f6;
            padding: 15px;
            text-align: left;
            font-weight: 600;
            color: #374151;
            border-bottom: 2px solid #e5e7eb;
            width: 35%;
        }}
        table td {{
            padding: 15px;
            border-bottom: 1px solid #e5e7eb;
            color: #1f2937;
        }}
        table tr:hover {{
            background: #f9fafb;
        }}
        .footer {{
            background: #f9fafb;
            padding: 20px;
            text-align: center;
            color: #6b7280;
            font-size: 12px;
        }}
        @media (max-width: 768px) {{
            table {{
                font-size: 14px;
            }}
            table th, table td {{
                padding: 10px;
            }}
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>PARA MEDICAL COUNCIL OF INDIA</h1>
            <p>Certificate Verification</p>
        </div>
        <div class='content'>
            <div class='verified-badge'>✓ Certificate Verified</div>
            <table>
                <tr>
                    <th>Registration No</th>
                    <td>{registration.CouncilEnrollmentNo ?? $"PMCI/{registration.Id}/25"}</td>
                </tr>
                <tr>
                    <th>Name</th>
                    <td>{registration.FullName}</td>
                </tr>
                <tr>
                    <th>Father Name</th>
                    <td>{registration.FathersName}</td>
                </tr>
                <tr>
                    <th>Mother Name</th>
                    <td>{registration.MothersName}</td>
                </tr>
                <tr>
                    <th>Aadhaar No / ID Proof No</th>
                    <td>{registration.AadhaarNumber}</td>
                </tr>
                <tr>
                    <th>Blood Group</th>
                    <td>{registration.BloodGroup ?? "N/A"}</td>
                </tr>
                <tr>
                    <th>Address</th>
                    <td>{registration.PermanentAddress}, PIN-{registration.PinCode}</td>
                </tr>
                <tr>
                    <th>Course</th>
                    <td>{registration.CourseName ?? registration.CourseType}</td>
                </tr>
                <tr>
                    <th>Examining Body</th>
                    <td>PARAMEDICAL EDUCATION & TRAINING COUNCIL</td>
                </tr>
                <tr>
                    <th>Date of Birth</th>
                    <td>{(registration.DateOfBirth != DateTime.MinValue ? registration.DateOfBirth.ToString("yyyy-MM-dd") : "N/A")}</td>
                </tr>
                <tr>
                    <th>Month/Year of Passing</th>
                    <td>{registration.PassYear}</td>
                </tr>
                <tr>
                    <th>Date of Issue</th>
                    <td>{issueDate.ToString("yyyy-MM-dd")}</td>
                </tr>
                <tr>
                    <th>Date of Expiry</th>
                    <td>{expiryDate.ToString("yyyy-MM-dd")}</td>
                </tr>
                <tr>
                    <th>Institute Name</th>
                    <td>{registration.InstituteName}</td>
                </tr>
                <tr>
                    <th>Mobile</th>
                    <td>{registration.Mobile}</td>
                </tr>
                <tr>
                    <th>Email</th>
                    <td>{registration.Email ?? "N/A"}</td>
                </tr>
                <tr>
                    <th>Registration Status</th>
                    <td><strong style='color: #10b981;'>{registration.RegistrationStatus.ToUpper()}</strong></td>
                </tr>
            </table>
        </div>
        <div class='footer'>
            <p>Para Medical Council of India | http://Schoolvns.org</p>
            <p>This certificate has been verified and is authentic.</p>
        </div>
    </div>
</body>
</html>";
            return html;
        }

        private string GenerateNotFoundHtml()
        {
            return @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Certificate Not Found</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 20px;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        .container {
            max-width: 500px;
            background: white;
            border-radius: 10px;
            box-shadow: 0 10px 40px rgba(0,0,0,0.2);
            padding: 40px;
            text-align: center;
        }
        .error-icon {
            font-size: 64px;
            color: #ef4444;
            margin-bottom: 20px;
        }
        h1 {
            color: #1f2937;
            margin-bottom: 10px;
        }
        p {
            color: #6b7280;
            margin-top: 10px;
        }
    </style>
</head>
<body>
    <div class='container'>
        <div class='error-icon'>⚠</div>
        <h1>Certificate Not Found</h1>
        <p>The certificate you are looking for does not exist or has been removed.</p>
        <p style='margin-top: 20px; font-size: 12px;'>Please verify the QR code or contact support.</p>
    </div>
</body>
</html>";
        }

        /// <summary>
        /// Generate and download registration certificate PDF
        /// </summary>
        [HttpGet("{id}/certificate")]
        public async Task<IActionResult> GenerateCertificate(int id)
        {
            var registrationResponse = await _studentRegistrationService.GetStudentRegistrationByIdAsync(id);
            
            if (!registrationResponse.Success || registrationResponse.Data == null)
            {
                return StatusCode((int)registrationResponse.StatusCode, registrationResponse);
            }
            try
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var pdfBytes = await _pdfCertificateService.GenerateRegistrationCertificateAsync(registrationResponse.Data, baseUrl);
                
                var fileName = $"{registrationResponse.Data.FullName.Replace(" ", "_")}Registration_Certificate_.pdf";
                
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    Success = false,
                    Message = $"Failed to generate certificate: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
        }
    }
}

