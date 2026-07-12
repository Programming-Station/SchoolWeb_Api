using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using School_DTOs;
using School_DTOs.Inventory;

namespace School.Services.Interfaces
{
    public interface IProcurementService
    {
        // Item Category & Inventory Items
        Task<APIResponse<List<ItemCategoryDto>>> GetCategoriesAsync(int schoolId);
        Task<APIResponse<List<InventoryItemDto>>> GetInventoryItemsAsync(int schoolId);
        Task<APIResponse<InventoryItemDto>> CreateInventoryItemAsync(int schoolId, CreateInventoryItemDto dto, string user);

        // Vendors
        Task<APIResponse<List<VendorDto>>> GetVendorsAsync(int schoolId);
        Task<APIResponse<VendorDto>> CreateVendorAsync(int schoolId, CreateVendorDto dto, string user);

        // Purchase Requisitions
        Task<APIResponse<List<PurchaseRequisitionDto>>> GetRequisitionsAsync(int schoolId);
        Task<APIResponse<PurchaseRequisitionDto>> CreateRequisitionAsync(int schoolId, CreatePurchaseRequisitionDto dto, string requester, string user);
        Task<APIResponse<bool>> UpdateRequisitionStatusAsync(int schoolId, int reqId, string status, string user);

        // Purchase Orders
        Task<APIResponse<List<PurchaseOrderDto>>> GetPurchaseOrdersAsync(int schoolId);
        Task<APIResponse<PurchaseOrderDto>> CreatePurchaseOrderAsync(int schoolId, CreatePurchaseOrderDto dto, string user);
        Task<APIResponse<bool>> UpdatePoStatusAsync(int schoolId, int poId, string status, string user);

        // Goods Receipt Note (GRN)
        Task<APIResponse<List<GoodsReceiptNoteDto>>> GetGrnsAsync(int schoolId);
        Task<APIResponse<GoodsReceiptNoteDto>> ReceiveGoodsAsync(int schoolId, CreateGrnDto dto, string receiver, string user);

        // Stock Transactions & Logs
        Task<APIResponse<List<StockTransactionDto>>> GetStockTransactionsAsync(int schoolId, int itemId);

        // Fixed Asset Depreciation & Maintenance
        Task<APIResponse<List<AssetDepreciationLogDto>>> GetAssetDepreciationsAsync(int schoolId, int itemId);
        Task<APIResponse<AssetDepreciationLogDto>> CalculateDepreciationAsync(int schoolId, int itemId, decimal ratePercent, string remarks, string user);
        Task<APIResponse<List<AssetMaintenanceLogDto>>> GetMaintenanceLogsAsync(int schoolId, int itemId);
        Task<APIResponse<AssetMaintenanceLogDto>> LogMaintenanceAsync(int schoolId, CreateAssetMaintenanceDto dto, string user);
    }
}
