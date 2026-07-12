using System;
using System.Collections.Generic;

namespace School_DTOs.Inventory
{
    // Item Category DTOs
    public class ItemCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
    }

    // Inventory Item DTOs
    public class InventoryItemDto
    {
        public int Id { get; set; }
        public string Sku { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Uom { get; set; } = "pcs";
        public decimal MinStockLevel { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal UnitPrice { get; set; }
        public int? AssetAccountId { get; set; }
        public string? AssetAccountName { get; set; }
        public int? ExpenseAccountId { get; set; }
        public string? ExpenseAccountName { get; set; }
    }

    public class CreateInventoryItemDto
    {
        public string Sku { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public string Uom { get; set; } = "pcs";
        public decimal MinStockLevel { get; set; }
        public decimal UnitPrice { get; set; }
        public int? AssetAccountId { get; set; }
        public int? ExpenseAccountId { get; set; }
    }

    // Vendor DTOs
    public class VendorDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? TaxRegistrationNo { get; set; }
        public int? CreditorAccountId { get; set; }
        public string? CreditorAccountName { get; set; }
    }

    public class CreateVendorDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? TaxRegistrationNo { get; set; }
        public int? CreditorAccountId { get; set; }
    }

    // Requisition DTOs
    public class PurchaseRequisitionDto
    {
        public int Id { get; set; }
        public string RequisitionNo { get; set; } = null!;
        public string RequestedBy { get; set; } = null!;
        public DateTime RequestDate { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public string? Remarks { get; set; }
        public string Status { get; set; } = "Draft";
        public List<PurchaseRequisitionItemDto> Items { get; set; } = new();
    }

    public class PurchaseRequisitionItemDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public string ItemSku { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal EstimatedCost { get; set; }
    }

    public class CreatePurchaseRequisitionDto
    {
        public int DepartmentId { get; set; }
        public string? Remarks { get; set; }
        public List<CreatePurchaseRequisitionItemDto> Items { get; set; } = new();
    }

    public class CreatePurchaseRequisitionItemDto
    {
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal EstimatedCost { get; set; }
    }

    // Purchase Order DTOs
    public class PurchaseOrderDto
    {
        public int Id { get; set; }
        public string PoNumber { get; set; } = null!;
        public int VendorId { get; set; }
        public string VendorName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Draft";
        public List<PurchaseOrderItemDto> Items { get; set; } = new();
    }

    public class PurchaseOrderItemDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public decimal QuantityOrdered { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class CreatePurchaseOrderDto
    {
        public int VendorId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
    }

    public class CreatePurchaseOrderItemDto
    {
        public int ItemId { get; set; }
        public decimal QuantityOrdered { get; set; }
        public decimal UnitPrice { get; set; }
    }

    // Goods Receipt DTOs
    public class GoodsReceiptNoteDto
    {
        public int Id { get; set; }
        public string GrnNumber { get; set; } = null!;
        public int PurchaseOrderId { get; set; }
        public string PoNumber { get; set; } = null!;
        public string VendorName { get; set; } = null!;
        public DateTime ReceivedDate { get; set; }
        public string? InvoiceNo { get; set; }
        public string ReceivedBy { get; set; } = null!;
        public string Status { get; set; } = "Draft";
        public List<GoodsReceiptNoteItemDto> Items { get; set; } = new();
    }

    public class GoodsReceiptNoteItemDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public decimal QuantityAccepted { get; set; }
        public decimal QuantityRejected { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class CreateGrnDto
    {
        public int PurchaseOrderId { get; set; }
        public string? InvoiceNo { get; set; }
        public List<CreateGrnItemDto> Items { get; set; } = new();
    }

    public class CreateGrnItemDto
    {
        public int ItemId { get; set; }
        public decimal QuantityAccepted { get; set; }
        public decimal QuantityRejected { get; set; }
        public decimal UnitPrice { get; set; }
    }

    // Stock Transaction DTOs
    public class StockTransactionDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public string TransactionType { get; set; } = "Inward";
        public decimal Quantity { get; set; }
        public string? ReferenceNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? WarehouseLoc { get; set; }
    }

    // Asset Depreciation DTOs
    public class AssetDepreciationLogDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public DateTime DepreciationDate { get; set; }
        public decimal DepreciationAmount { get; set; }
        public decimal BookValueAfterDepreciation { get; set; }
        public string? Remarks { get; set; }
    }

    // Asset Maintenance DTOs
    public class AssetMaintenanceLogDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceType { get; set; } = "Preventative";
        public decimal Cost { get; set; }
        public string? ServiceDetails { get; set; }
        public string? PerformedBy { get; set; }
    }

    public class CreateAssetMaintenanceDto
    {
        public int ItemId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceType { get; set; } = "Preventative";
        public decimal Cost { get; set; }
        public string? ServiceDetails { get; set; }
        public string? PerformedBy { get; set; }
    }

    // --- Enterprise Extensions ---

    // Warehouse DTOs
    public class WarehouseDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public decimal Capacity { get; set; }
    }

    public class CreateWarehouseDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public decimal Capacity { get; set; }
    }

    // Warehouse Bin DTOs
    public class WarehouseBinDto
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = null!;
        public string Zone { get; set; } = "Default";
        public string Rack { get; set; } = "Default";
        public string Shelf { get; set; } = "Default";
        public string BinCode { get; set; } = null!;
        public decimal Capacity { get; set; }
    }

    public class CreateWarehouseBinDto
    {
        public int WarehouseId { get; set; }
        public string Zone { get; set; } = "Default";
        public string Rack { get; set; } = "Default";
        public string Shelf { get; set; } = "Default";
        public string BinCode { get; set; } = null!;
        public decimal Capacity { get; set; }
    }

    // Store DTOs
    public class StoreDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string StoreType { get; set; } = "General";
        public string? ContactPerson { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateStoreDto
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string StoreType { get; set; } = "General";
        public string? ContactPerson { get; set; }
        public bool IsActive { get; set; }
    }

    // RFQ DTOs
    public class RequestForQuotationDto
    {
        public int Id { get; set; }
        public string RfqNo { get; set; } = null!;
        public int RequisitionId { get; set; }
        public string RequisitionNo { get; set; } = null!;
        public DateTime RfqDate { get; set; }
        public string Status { get; set; } = "Draft";
    }

    public class CreateRequestForQuotationDto
    {
        public int RequisitionId { get; set; }
    }

    // Quotation DTOs
    public class VendorQuotationDto
    {
        public int Id { get; set; }
        public int RfqId { get; set; }
        public string RfqNo { get; set; } = null!;
        public int VendorId { get; set; }
        public string VendorName { get; set; } = null!;
        public DateTime QuoteDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Submitted";
    }

    public class CreateVendorQuotationDto
    {
        public int RfqId { get; set; }
        public int VendorId { get; set; }
        public decimal TotalAmount { get; set; }
    }

    // Return DTOs
    public class PurchaseReturnDto
    {
        public int Id { get; set; }
        public string ReturnNo { get; set; } = null!;
        public int GrnId { get; set; }
        public string GrnNo { get; set; } = null!;
        public DateTime ReturnDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Reason { get; set; } = null!;
        public string? CreditNoteNo { get; set; }
        public decimal RefundAmount { get; set; }
        public string Status { get; set; } = "Draft";
    }

    public class CreatePurchaseReturnDto
    {
        public int GrnId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Reason { get; set; } = null!;
        public string? CreditNoteNo { get; set; }
        public decimal RefundAmount { get; set; }
    }

    // Stock Issue DTOs
    public class StockIssueDto
    {
        public int Id { get; set; }
        public string IssueNo { get; set; } = null!;
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public decimal Quantity { get; set; }
        public string IssuedToType { get; set; } = null!;
        public int IssuedToId { get; set; }
        public DateTime IssueDate { get; set; }
        public bool Returnable { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = "Issued";
    }

    public class CreateStockIssueDto
    {
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public string IssuedToType { get; set; } = null!;
        public int IssuedToId { get; set; }
        public bool Returnable { get; set; }
        public DateTime? DueDate { get; set; }
    }

    // Quality Inspection DTOs
    public class QualityInspectionDto
    {
        public int Id { get; set; }
        public int GrnId { get; set; }
        public string GrnNo { get; set; } = null!;
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public decimal QuantityInspected { get; set; }
        public decimal QuantityAccepted { get; set; }
        public decimal QuantityRejected { get; set; }
        public string? InspectionReport { get; set; }
        public decimal QualityScore { get; set; }
        public string InspectedBy { get; set; } = null!;
    }

    public class CreateQualityInspectionDto
    {
        public int GrnId { get; set; }
        public int ItemId { get; set; }
        public decimal QuantityInspected { get; set; }
        public decimal QuantityAccepted { get; set; }
        public decimal QuantityRejected { get; set; }
        public string? InspectionReport { get; set; }
        public decimal QualityScore { get; set; }
    }

    // AI Forecast DTOs
    public class AIDemandForecastDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public double ForecastedDemandNextMonth { get; set; }
        public double ConfidenceInterval { get; set; }
        public string Recommendation { get; set; } = null!;
    }

    public class AIReorderSuggestionDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public decimal CurrentStock { get; set; }
        public decimal ReorderLevel { get; set; }
        public decimal RecommendedReorderQty { get; set; }
        public string UrgencyLevel { get; set; } = "Medium";
    }
}
