using School.Domain;
using School.Infrastructure.Repositories.IRepositories;
using School.Models.Student;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Student;
using System.Net;

namespace School.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public Task<APIResponse<StudentDto>> CreateStudentAsync(StudentModel model)
        {
            return _studentRepository.AddStudentAsync(model);
        }

        public Task<APIResponse<StudentDto>> GetStudentByIdAsync(int id)
        {
            return _studentRepository.GetStudentByIdAsync(id);
        }

        public Task<APIResponse<StudentDto>> GetStudentByStudentIdAsync(string studentId)
        {
            return _studentRepository.GetStudentByStudentIdAsync(studentId);
        }

        public Task<APIResponse<IEnumerable<StudentDto>>> GetAllStudentsAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? status = null)
        {
            return _studentRepository.GetAllStudentsAsync(pageNumber, pageSize, searchTerm, status);
        }

        public Task<APIResponse> UpdateStudentAsync(StudentModel model)
        {
            return _studentRepository.UpdateStudentAsync(model);
        }

        public Task<APIResponse> DeleteStudentAsync(int id)
        {
            return _studentRepository.DeleteStudentAsync(id);
        }

        public async Task<APIResponse<string>> GenerateStudentIdAsync()
        {
            string studentId = await _studentRepository.GenerateStudentIdAsync();
            return new APIResponse<string>
            {
                Success = true,
                Message = "Student ID generated successfully",
                StatusCode = HttpStatusCode.OK,
                Data = studentId
            };
        }
    }
}

