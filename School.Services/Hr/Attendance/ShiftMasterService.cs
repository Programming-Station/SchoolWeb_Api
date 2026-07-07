using School.Infrastructure.Repositories.Hr.Attendance;

namespace School.Services.Hr.Attendance
{
    public class ShiftMasterService : IShiftMasterService
    {
        private readonly IShiftMasterRepository _repository;
        public ShiftMasterService(IShiftMasterRepository repository)
        {
            _repository = repository;
        }
    }
}
