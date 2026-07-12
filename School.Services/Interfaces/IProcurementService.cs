using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using School_DTOs;
using School_DTOs.Inventory;

namespace School.Services.Interfaces
{
    public interface IProcurementService
    {
        // --- Item Categories ---
        Task<APIResponse<List<ItemCategoryDto>>> GetCategoriesAsync(int schoolId, string? search = null);
        Task<APIResponse<ItemCategoryDto>> GetCategoryByIdAsync(int schoolId, int id);
        Task<APIResponse<ItemCategoryDto>> CreateCategoryAsync(int schoolId, ItemCategoryDto dto, string user);
        Task<APIResponse<ItemCategoryDto>> UpdateCategoryAsync(int schoolId, int id, ItemCategoryDto dto, string user);
        Task<APIResponse<bool>> DeleteCategoryAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportCategoriesAsync(int schoolId, string? search = null);

        // --- Inventory Items ---
        Task<APIResponse<List<InventoryItemDto>>> GetInventoryItemsAsync(int schoolId, string? search = null, int? categoryId = null);
        Task<APIResponse<InventoryItemDto>> GetInventoryItemByIdAsync(int schoolId, int id);
        Task<APIResponse<CreateInventoryItemDto>> GetInventoryItemForEditAsync(int schoolId, int id);
        Task<APIResponse<InventoryItemDto>> CreateInventoryItemAsync(int schoolId, CreateInventoryItemDto dto, string user);
        Task<APIResponse<InventoryItemDto>> UpdateInventoryItemAsync(int schoolId, int id, CreateInventoryItemDto dto, string user);
        Task<APIResponse<bool>> DeleteInventoryItemAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportInventoryItemsAsync(int schoolId, string? search = null, int? categoryId = null);

        // --- Vendors ---
        Task<APIResponse<List<VendorDto>>> GetVendorsAsync(int schoolId, string? search = null);
        Task<APIResponse<VendorDto>> GetVendorByIdAsync(int schoolId, int id);
        Task<APIResponse<VendorDto>> CreateVendorAsync(int schoolId, CreateVendorDto dto, string user);
        Task<APIResponse<VendorDto>> UpdateVendorAsync(int schoolId, int id, CreateVendorDto dto, string user);
        Task<APIResponse<bool>> DeleteVendorAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportVendorsAsync(int schoolId, string? search = null);

        // --- Purchase Requisitions ---
        Task<APIResponse<List<PurchaseRequisitionDto>>> GetRequisitionsAsync(int schoolId, string? status = null);
        Task<APIResponse<PurchaseRequisitionDto>> GetRequisitionByIdAsync(int schoolId, int id);
        Task<APIResponse<PurchaseRequisitionDto>> CreateRequisitionAsync(int schoolId, CreatePurchaseRequisitionDto dto, string requester, string user);
        Task<APIResponse<PurchaseRequisitionDto>> UpdateRequisitionAsync(int schoolId, int id, CreatePurchaseRequisitionDto dto, string user);
        Task<APIResponse<bool>> UpdateRequisitionStatusAsync(int schoolId, int reqId, string status, string user);
        Task<APIResponse<bool>> DeleteRequisitionAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportRequisitionsAsync(int schoolId, string? status = null);

        // --- Purchase Orders ---
        Task<APIResponse<List<PurchaseOrderDto>>> GetPurchaseOrdersAsync(int schoolId, string? status = null);
        Task<APIResponse<PurchaseOrderDto>> GetPurchaseOrderByIdAsync(int schoolId, int id);
        Task<APIResponse<PurchaseOrderDto>> CreatePurchaseOrderAsync(int schoolId, CreatePurchaseOrderDto dto, string user);
        Task<APIResponse<PurchaseOrderDto>> UpdatePurchaseOrderAsync(int schoolId, int id, CreatePurchaseOrderDto dto, string user);
        Task<APIResponse<bool>> UpdatePoStatusAsync(int schoolId, int poId, string status, string user);
        Task<APIResponse<bool>> DeletePurchaseOrderAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportPurchaseOrdersAsync(int schoolId, string? status = null);

        // --- Goods Receipt Notes (GRN) ---
        Task<APIResponse<List<GoodsReceiptNoteDto>>> GetGrnsAsync(int schoolId, string? status = null);
        Task<APIResponse<GoodsReceiptNoteDto>> GetGrnByIdAsync(int schoolId, int id);
        Task<APIResponse<GoodsReceiptNoteDto>> ReceiveGoodsAsync(int schoolId, CreateGrnDto dto, string receiver, string user);
        Task<APIResponse<byte[]>> ExportGrnsAsync(int schoolId, string? status = null);

        // --- Stock Transactions & Logs ---
        Task<APIResponse<List<StockTransactionDto>>> GetStockTransactionsAsync(int schoolId, int itemId);

        // --- Fixed Asset Depreciation & Maintenance ---
        Task<APIResponse<List<AssetDepreciationLogDto>>> GetAssetDepreciationsAsync(int schoolId, int itemId);
        Task<APIResponse<AssetDepreciationLogDto>> CalculateDepreciationAsync(int schoolId, int itemId, decimal ratePercent, string remarks, string user);
        Task<APIResponse<List<AssetMaintenanceLogDto>>> GetMaintenanceLogsAsync(int schoolId, int itemId);
        Task<APIResponse<AssetMaintenanceLogDto>> LogMaintenanceAsync(int schoolId, CreateAssetMaintenanceDto dto, string user);

        // --- Warehouses & Bins ---
        Task<APIResponse<List<WarehouseDto>>> GetWarehousesAsync(int schoolId, string? search = null);
        Task<APIResponse<WarehouseDto>> GetWarehouseByIdAsync(int schoolId, int id);
        Task<APIResponse<WarehouseDto>> CreateWarehouseAsync(int schoolId, CreateWarehouseDto dto, string user);
        Task<APIResponse<WarehouseDto>> UpdateWarehouseAsync(int schoolId, int id, CreateWarehouseDto dto, string user);
        Task<APIResponse<bool>> DeleteWarehouseAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportWarehousesAsync(int schoolId, string? search = null);

        Task<APIResponse<List<WarehouseBinDto>>> GetWarehouseBinsAsync(int schoolId, int? warehouseId = null);
        Task<APIResponse<WarehouseBinDto>> GetWarehouseBinByIdAsync(int schoolId, int id);
        Task<APIResponse<WarehouseBinDto>> CreateWarehouseBinAsync(int schoolId, CreateWarehouseBinDto dto, string user);
        Task<APIResponse<WarehouseBinDto>> UpdateWarehouseBinAsync(int schoolId, int id, CreateWarehouseBinDto dto, string user);
        Task<APIResponse<bool>> DeleteWarehouseBinAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportWarehouseBinsAsync(int schoolId, int? warehouseId = null);

        // --- Stores ---
        Task<APIResponse<List<StoreDto>>> GetStoresAsync(int schoolId, string? storeType = null);
        Task<APIResponse<StoreDto>> GetStoreByIdAsync(int schoolId, int id);
        Task<APIResponse<StoreDto>> CreateStoreAsync(int schoolId, CreateStoreDto dto, string user);
        Task<APIResponse<StoreDto>> UpdateStoreAsync(int schoolId, int id, CreateStoreDto dto, string user);
        Task<APIResponse<bool>> DeleteStoreAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportStoresAsync(int schoolId, string? storeType = null);

        // --- Request For Quotation (RFQ) & Quotations ---
        Task<APIResponse<List<RequestForQuotationDto>>> GetRfqsAsync(int schoolId, string? status = null);
        Task<APIResponse<RequestForQuotationDto>> GetRfqByIdAsync(int schoolId, int id);
        Task<APIResponse<RequestForQuotationDto>> CreateRfqAsync(int schoolId, CreateRequestForQuotationDto dto, string user);
        Task<APIResponse<bool>> UpdateRfqStatusAsync(int schoolId, int id, string status, string user);
        Task<APIResponse<bool>> DeleteRfqAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportRfqsAsync(int schoolId, string? status = null);

        Task<APIResponse<List<VendorQuotationDto>>> GetQuotationsAsync(int schoolId, int? rfqId = null);
        Task<APIResponse<VendorQuotationDto>> GetQuotationByIdAsync(int schoolId, int id);
        Task<APIResponse<VendorQuotationDto>> CreateQuotationAsync(int schoolId, CreateVendorQuotationDto dto, string user);
        Task<APIResponse<bool>> UpdateQuotationStatusAsync(int schoolId, int id, string status, string user);
        Task<APIResponse<bool>> DeleteQuotationAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportQuotationsAsync(int schoolId, int? rfqId = null);

        // --- Purchase Returns ---
        Task<APIResponse<List<PurchaseReturnDto>>> GetReturnsAsync(int schoolId, string? status = null);
        Task<APIResponse<PurchaseReturnDto>> GetReturnByIdAsync(int schoolId, int id);
        Task<APIResponse<PurchaseReturnDto>> CreateReturnAsync(int schoolId, CreatePurchaseReturnDto dto, string user);
        Task<APIResponse<bool>> UpdateReturnStatusAsync(int schoolId, int id, string status, string user);
        Task<APIResponse<bool>> DeleteReturnAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportReturnsAsync(int schoolId, string? status = null);

        // --- Stock Issues ---
        Task<APIResponse<List<StockIssueDto>>> GetStockIssuesAsync(int schoolId, string? issuedToType = null, int? issuedToId = null);
        Task<APIResponse<StockIssueDto>> GetStockIssueByIdAsync(int schoolId, int id);
        Task<APIResponse<StockIssueDto>> CreateStockIssueAsync(int schoolId, CreateStockIssueDto dto, string user);
        Task<APIResponse<bool>> UpdateStockIssueStatusAsync(int schoolId, int id, string status, string user);
        Task<APIResponse<bool>> DeleteStockIssueAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportStockIssuesAsync(int schoolId, string? issuedToType = null);

        // --- Quality Inspections ---
        Task<APIResponse<List<QualityInspectionDto>>> GetInspectionsAsync(int schoolId, int? grnId = null);
        Task<APIResponse<QualityInspectionDto>> GetInspectionByIdAsync(int schoolId, int id);
        Task<APIResponse<QualityInspectionDto>> CreateInspectionAsync(int schoolId, CreateQualityInspectionDto dto, string user);
        Task<APIResponse<bool>> DeleteInspectionAsync(int schoolId, int id, string user);
        Task<APIResponse<byte[]>> ExportInspectionsAsync(int schoolId, int? grnId = null);

        // --- AI Forecast & Reorder Operations ---
        Task<APIResponse<AIDemandForecastDto>> GetDemandForecastAsync(int schoolId, int itemId);
        Task<APIResponse<List<AIReorderSuggestionDto>>> GetReorderSuggestionsAsync(int schoolId);
    }
}
