using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using School.Domain.Academic;
using School.Infrastructure.Repositories.IRepositories;
using School.Services.Interfaces.Academic;

namespace School.Services.Academic
{
    public class StudentAttendanceService : IStudentAttendanceService
    {
        private readonly IStudentAttendanceRepository _repo;

        public StudentAttendanceService(IStudentAttendanceRepository repo)
        {
            _repo = repo;
        }

        public async Task<(bool Success, string Message, int Marked)> MarkBulkAttendanceAsync(
            BulkAttendanceRequest request, string markedBy, int schoolRegistrationId)
        {
            var toAdd = new List<StudentAttendance>();

            foreach (var entry in request.Entries)
            {
                // Skip if already marked for this student/date/subject
                var exists = await _repo.ExistsAsync(entry.StudentId, request.Date, request.SubjectId, schoolRegistrationId);
                if (exists) continue;

                toAdd.Add(new StudentAttendance
                {
                    StudentId          = entry.StudentId,
                    SubjectId          = request.SubjectId,
                    ClassId            = request.ClassId,
                    AttendanceDate     = request.Date.Date,
                    Status             = entry.Status,
                    PeriodNo           = request.PeriodNo,
                    MarkedBy           = markedBy,
                    Remarks            = entry.Remarks,
                    SchoolRegistrationId = schoolRegistrationId,
                    CreatedBy          = markedBy
                });
            }

            if (toAdd.Any())
                await _repo.AddRangeAsync(toAdd);

            return (true, $"{toAdd.Count} attendance records saved.", toAdd.Count);
        }

        public async Task<IEnumerable<AttendanceRecordDto>> GetByDateAndClassAsync(
            DateTime date, int classId, int schoolRegistrationId)
        {
            var data = await _repo.GetByDateAndClassAsync(date, classId, schoolRegistrationId);
            return data.Select(MapToDto);
        }

        public async Task<IEnumerable<AttendanceRecordDto>> GetByStudentAsync(
            int studentId, int schoolRegistrationId, DateTime? from, DateTime? to)
        {
            var data = await _repo.GetByStudentAsync(studentId, schoolRegistrationId, from, to);
            return data.Select(MapToDto);
        }

        public async Task<AttendanceSummaryDto> GetStudentSummaryAsync(
            int studentId, int schoolRegistrationId, DateTime? from, DateTime? to)
        {
            var (present, absent, late, leave, total) =
                await _repo.GetStudentSummaryAsync(studentId, schoolRegistrationId, from, to);

            return new AttendanceSummaryDto
            {
                StudentId = studentId,
                Present   = present,
                Absent    = absent,
                Late      = late,
                Leave     = leave,
                Total     = total
            };
        }

        public async Task<IEnumerable<AttendanceSummaryDto>> GetClassSummaryAsync(
            int classId, int month, int year, int schoolRegistrationId)
        {
            var records = await _repo.GetByClassAndMonthAsync(classId, month, year, schoolRegistrationId);

            return records
                .GroupBy(r => new { r.StudentId, r.Student?.Name })
                .Select(g => new AttendanceSummaryDto
                {
                    StudentId   = g.Key.StudentId,
                    StudentName = g.Key.Name ?? string.Empty,
                    Present     = g.Count(x => x.Status == "Present"),
                    Absent      = g.Count(x => x.Status == "Absent"),
                    Late        = g.Count(x => x.Status == "Late"),
                    Leave       = g.Count(x => x.Status == "Leave"),
                    Total       = g.Count()
                })
                .OrderBy(x => x.StudentName)
                .ToList();
        }

        public async Task<(bool Success, string Message)> UpdateAttendanceAsync(
            int id, string status, string? remarks, int schoolRegistrationId)
        {
            var record = await _repo.GetByIdAsync(id);
            if (record == null || record.SchoolRegistrationId != schoolRegistrationId)
                return (false, "Record not found.");

            record.Status  = status;
            record.Remarks = remarks;
            await _repo.UpdateAsync(record);
            return (true, "Attendance updated.");
        }

        private static AttendanceRecordDto MapToDto(StudentAttendance a) => new()
        {
            Id             = a.Id,
            StudentId      = a.StudentId,
            StudentName    = a.Student?.Name ?? string.Empty,
            StudentCode    = a.Student?.StudentId ?? string.Empty,
            SubjectId      = a.SubjectId,
            SubjectName    = a.Subject?.Name,
            ClassId        = a.ClassId,
            ClassName      = a.Class?.Name,
            AttendanceDate = a.AttendanceDate,
            Status         = a.Status,
            PeriodNo       = a.PeriodNo,
            Remarks        = a.Remarks
        };
    }
}
