using System;

namespace School_DTOs.Fee
{
    #nullable disable

    public class FinanceCollectionSummaryDto
    {
        public decimal TotalBilled { get; set; }
        public decimal TotalCollected { get; set; }
        public decimal TotalRefunded { get; set; }
        public decimal TotalScholarships { get; set; }
        public decimal TotalFinesApplied { get; set; }
        public decimal TotalFinesCollected { get; set; }
        public decimal TotalOutstanding { get; set; }
        public int TotalTransactionsCount { get; set; }
    }

    public class FeeHeadBreakupDto
    {
        public string FeeHeadName { get; set; }
        public decimal TargetBilled { get; set; }
        public decimal TotalCollected { get; set; }
        public decimal TotalRefunded { get; set; }
        public decimal Outstanding { get; set; }
    }

    public class ClassFeeSummaryDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public decimal TotalBilled { get; set; }
        public decimal TotalCollected { get; set; }
        public decimal TotalRefunded { get; set; }
        public decimal TotalOutstanding { get; set; }
        public int DefaultersCount { get; set; }
    }
}
