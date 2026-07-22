using School_DTOs.Fee;

namespace School.Services.Interfaces
{
    public interface IFeeReportService
    {
        Task<FinanceCollectionSummaryDto> GetCollectionSummaryAsync(DateTime? fromDate, DateTime? toDate, int schoolId);
        Task<IEnumerable<FeeHeadBreakupDto>> GetHeadBreakupAsync(DateTime? fromDate, DateTime? toDate, int schoolId);
        Task<IEnumerable<ClassFeeSummaryDto>> GetClassWiseSummaryAsync(int schoolId);
    }
}
