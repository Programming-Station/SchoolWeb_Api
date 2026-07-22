using Microsoft.AspNetCore.Mvc;
using School.Infrastructure.Interfaces;
using School.Services.Interfaces;
using School_API.Common.Interface;
using School_DTOs.Inventory;

namespace School_API.Controllers.Inventory
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProcurementController : BaseController
    {
        private readonly IProcurementService _svc;
        private readonly ITenantService _tenantSvc;

        public ProcurementController(IProcurementService svc, ITenantService tenantSvc, ICurrentUserService cur) : base(cur)
        {
            _svc = svc;
            _tenantSvc = tenantSvc;
        }

        private int SchoolId => _tenantSvc.GetTenantId() ?? 1;

        // --- Item Categories ---
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] string? search = null)
        {
            var r = await _svc.GetCategoriesAsync(SchoolId, search);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] ItemCategoryDto dto)
        {
            var r = await _svc.CreateCategoryAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] ItemCategoryDto dto)
        {
            var r = await _svc.UpdateCategoryAsync(SchoolId, id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var r = await _svc.DeleteCategoryAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportCategories([FromQuery] string? search = null)
        {
            var r = await _svc.ExportCategoriesAsync(SchoolId, search);
            if (r.Success && r.Data != null) return File(r.Data, "text/csv", "item_categories.csv");
            return StatusCode((int)r.StatusCode, r);
        }

        // --- Inventory Items ---
        [HttpGet]
        public async Task<IActionResult> GetInventoryItems([FromQuery] string? search = null, [FromQuery] int? categoryId = null)
        {
            var r = await _svc.GetInventoryItemsAsync(SchoolId, search, categoryId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInventoryItem([FromBody] CreateInventoryItemDto dto)
        {
            var r = await _svc.CreateInventoryItemAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventoryItem(int id, [FromBody] CreateInventoryItemDto dto)
        {
            var r = await _svc.UpdateInventoryItemAsync(SchoolId, id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryItem(int id)
        {
            var r = await _svc.DeleteInventoryItemAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportInventoryItems([FromQuery] string? search = null, [FromQuery] int? categoryId = null)
        {
            var r = await _svc.ExportInventoryItemsAsync(SchoolId, search, categoryId);
            if (r.Success && r.Data != null) return File(r.Data, "text/csv", "inventory_items.csv");
            return StatusCode((int)r.StatusCode, r);
        }

        // --- Vendors ---
        [HttpGet]
        public async Task<IActionResult> GetVendors([FromQuery] string? search = null)
        {
            var r = await _svc.GetVendorsAsync(SchoolId, search);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVendor([FromBody] CreateVendorDto dto)
        {
            var r = await _svc.CreateVendorAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVendor(int id, [FromBody] CreateVendorDto dto)
        {
            var r = await _svc.UpdateVendorAsync(SchoolId, id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            var r = await _svc.DeleteVendorAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportVendors([FromQuery] string? search = null)
        {
            var r = await _svc.ExportVendorsAsync(SchoolId, search);
            if (r.Success && r.Data != null) return File(r.Data, "text/csv", "vendors.csv");
            return StatusCode((int)r.StatusCode, r);
        }

        // --- Purchase Requisitions ---
        [HttpGet]
        public async Task<IActionResult> GetRequisitions([FromQuery] string? status = null)
        {
            var r = await _svc.GetRequisitionsAsync(SchoolId, status);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequisition([FromBody] CreatePurchaseRequisitionDto dto)
        {
            var r = await _svc.CreateRequisitionAsync(SchoolId, dto, UserName, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRequisition(int id, [FromBody] CreatePurchaseRequisitionDto dto)
        {
            var r = await _svc.UpdateRequisitionAsync(SchoolId, id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{reqId}")]
        public async Task<IActionResult> UpdateRequisitionStatus(int reqId, [FromQuery] string status)
        {
            var r = await _svc.UpdateRequisitionStatusAsync(SchoolId, reqId, status, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequisition(int id)
        {
            var r = await _svc.DeleteRequisitionAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportRequisitions([FromQuery] string? status = null)
        {
            var r = await _svc.ExportRequisitionsAsync(SchoolId, status);
            if (r.Success && r.Data != null) return File(r.Data, "text/csv", "purchase_requisitions.csv");
            return StatusCode((int)r.StatusCode, r);
        }

        // --- Purchase Orders ---
        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrders([FromQuery] string? status = null)
        {
            var r = await _svc.GetPurchaseOrdersAsync(SchoolId, status);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderDto dto)
        {
            var r = await _svc.CreatePurchaseOrderAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchaseOrder(int id, [FromBody] CreatePurchaseOrderDto dto)
        {
            var r = await _svc.UpdatePurchaseOrderAsync(SchoolId, id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{poId}")]
        public async Task<IActionResult> UpdatePoStatus(int poId, [FromQuery] string status)
        {
            var r = await _svc.UpdatePoStatusAsync(SchoolId, poId, status, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseOrder(int id)
        {
            var r = await _svc.DeletePurchaseOrderAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportPurchaseOrders([FromQuery] string? status = null)
        {
            var r = await _svc.ExportPurchaseOrdersAsync(SchoolId, status);
            if (r.Success && r.Data != null) return File(r.Data, "text/csv", "purchase_orders.csv");
            return StatusCode((int)r.StatusCode, r);
        }

        // --- Goods Receipt Notes ---
        [HttpGet]
        public async Task<IActionResult> GetGrns([FromQuery] string? status = null)
        {
            var r = await _svc.GetGrnsAsync(SchoolId, status);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveGoods([FromBody] CreateGrnDto dto)
        {
            var r = await _svc.ReceiveGoodsAsync(SchoolId, dto, UserName, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportGrns([FromQuery] string? status = null)
        {
            var r = await _svc.ExportGrnsAsync(SchoolId, status);
            if (r.Success && r.Data != null) return File(r.Data, "text/csv", "goods_receipt_notes.csv");
            return StatusCode((int)r.StatusCode, r);
        }

        // --- Stock Transactions ---
        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetStockTransactions(int itemId)
        {
            var r = await _svc.GetStockTransactionsAsync(SchoolId, itemId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // --- Asset Depreciation ---
        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetAssetDepreciations(int itemId)
        {
            var r = await _svc.GetAssetDepreciationsAsync(SchoolId, itemId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CalculateDepreciation([FromQuery] int itemId, [FromQuery] decimal ratePercent, [FromQuery] string remarks)
        {
            var r = await _svc.CalculateDepreciationAsync(SchoolId, itemId, ratePercent, remarks, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // --- Asset Maintenance ---
        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetMaintenanceLogs(int itemId)
        {
            var r = await _svc.GetMaintenanceLogsAsync(SchoolId, itemId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> LogMaintenance([FromBody] CreateAssetMaintenanceDto dto)
        {
            var r = await _svc.LogMaintenanceAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        // --- Warehouses ---
        [HttpGet]
        public async Task<IActionResult> GetWarehouses([FromQuery] string? search = null)
        {
            var r = await _svc.GetWarehousesAsync(SchoolId, search);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseDto dto)
        {
            var r = await _svc.CreateWarehouseAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWarehouse(int id, [FromBody] CreateWarehouseDto dto)
        {
            var r = await _svc.UpdateWarehouseAsync(SchoolId, id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            var r = await _svc.DeleteWarehouseAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportWarehouses([FromQuery] string? search = null)
        {
            var r = await _svc.ExportWarehousesAsync(SchoolId, search);
            if (r.Success && r.Data != null) return File(r.Data, "text/csv", "warehouses.csv");
            return StatusCode((int)r.StatusCode, r);
        }

        // --- Bins ---
        [HttpGet]
        public async Task<IActionResult> GetWarehouseBins([FromQuery] int? warehouseId = null)
        {
            var r = await _svc.GetWarehouseBinsAsync(SchoolId, warehouseId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouseBin([FromBody] CreateWarehouseBinDto dto)
        {
            var r = await _svc.CreateWarehouseBinAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWarehouseBin(int id, [FromBody] CreateWarehouseBinDto dto)
        {
            var r = await _svc.UpdateWarehouseBinAsync(SchoolId, id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouseBin(int id)
        {
            var r = await _svc.DeleteWarehouseBinAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportWarehouseBins([FromQuery] int? warehouseId = null)
        {
            var r = await _svc.ExportWarehouseBinsAsync(SchoolId, warehouseId);
            if (r.Success && r.Data != null) return File(r.Data, "text/csv", "warehouse_bins.csv");
            return StatusCode((int)r.StatusCode, r);
        }

        // --- Stores ---
        [HttpGet]
        public async Task<IActionResult> GetStores([FromQuery] string? storeType = null)
        {
            var r = await _svc.GetStoresAsync(SchoolId, storeType);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStore([FromBody] CreateStoreDto dto)
        {
            var r = await _svc.CreateStoreAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStore(int id, [FromBody] CreateStoreDto dto)
        {
            var r = await _svc.UpdateStoreAsync(SchoolId, id, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var r = await _svc.DeleteStoreAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportStores([FromQuery] string? storeType = null)
        {
            var r = await _svc.ExportStoresAsync(SchoolId, storeType);
            if (r.Success && r.Data != null) return File(r.Data, "text/csv", "stores.csv");
            return StatusCode((int)r.StatusCode, r);
        }

        // --- RFQs ---
        [HttpGet]
        public async Task<IActionResult> GetRfqs([FromQuery] string? status = null)
        {
            var r = await _svc.GetRfqsAsync(SchoolId, status);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRfq([FromBody] CreateRequestForQuotationDto dto)
        {
            var r = await _svc.CreateRfqAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateRfqStatus(int id, [FromQuery] string status)
        {
            var r = await _svc.UpdateRfqStatusAsync(SchoolId, id, status, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRfq(int id)
        {
            var r = await _svc.DeleteRfqAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportRfqs([FromQuery] string? status = null)
        {
            var r = await _svc.ExportRfqsAsync(SchoolId, status);
            if (r.Success && r.Data != null) return File(r.Data, "text/csv", "request_for_quotations.csv");
            return StatusCode((int)r.StatusCode, r);
        }

        // --- Quotations ---
        [HttpGet]
        public async Task<IActionResult> GetQuotations([FromQuery] int? rfqId = null)
        {
            var r = await _svc.GetQuotationsAsync(SchoolId, rfqId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuotation([FromBody] CreateVendorQuotationDto dto)
        {
            var r = await _svc.CreateQuotationAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateQuotationStatus(int id, [FromQuery] string status)
        {
            var r = await _svc.UpdateQuotationStatusAsync(SchoolId, id, status, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuotation(int id)
        {
            var r = await _svc.DeleteQuotationAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportQuotations([FromQuery] int? rfqId = null)
        {
            var r = await _svc.ExportQuotationsAsync(SchoolId, rfqId);
            if (r.Success && r.Data != null) return File(r.Data, "text/csv", "vendor_quotations.csv");
            return StatusCode((int)r.StatusCode, r);
        }

        // --- Returns ---
        [HttpGet]
        public async Task<IActionResult> GetReturns([FromQuery] string? status = null)
        {
            var r = await _svc.GetReturnsAsync(SchoolId, status);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReturn([FromBody] CreatePurchaseReturnDto dto)
        {
            var r = await _svc.CreateReturnAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateReturnStatus(int id, [FromQuery] string status)
        {
            var r = await _svc.UpdateReturnStatusAsync(SchoolId, id, status, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReturn(int id)
        {
            var r = await _svc.DeleteReturnAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportReturns([FromQuery] string? status = null)
        {
            var r = await _svc.ExportReturnsAsync(SchoolId, status);
            if (r.Success && r.Data != null) return File(r.Data, "text/csv", "purchase_returns.csv");
            return StatusCode((int)r.StatusCode, r);
        }

        // --- Stock Issues ---
        [HttpGet]
        public async Task<IActionResult> GetStockIssues([FromQuery] string? issuedToType = null, [FromQuery] int? issuedToId = null)
        {
            var r = await _svc.GetStockIssuesAsync(SchoolId, issuedToType, issuedToId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStockIssue([FromBody] CreateStockIssueDto dto)
        {
            var r = await _svc.CreateStockIssueAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateStockIssueStatus(int id, [FromQuery] string status)
        {
            var r = await _svc.UpdateStockIssueStatusAsync(SchoolId, id, status, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStockIssue(int id)
        {
            var r = await _svc.DeleteStockIssueAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportStockIssues([FromQuery] string? issuedToType = null)
        {
            var r = await _svc.ExportStockIssuesAsync(SchoolId, issuedToType);
            if (r.Success && r.Data != null) return File(r.Data, "text/csv", "stock_issues.csv");
            return StatusCode((int)r.StatusCode, r);
        }

        // --- Quality Inspections ---
        [HttpGet]
        public async Task<IActionResult> GetInspections([FromQuery] int? grnId = null)
        {
            var r = await _svc.GetInspectionsAsync(SchoolId, grnId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInspection([FromBody] CreateQualityInspectionDto dto)
        {
            var r = await _svc.CreateInspectionAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspection(int id)
        {
            var r = await _svc.DeleteInspectionAsync(SchoolId, id, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> ExportInspections([FromQuery] int? grnId = null)
        {
            var r = await _svc.ExportInspectionsAsync(SchoolId, grnId);
            if (r.Success && r.Data != null) return File(r.Data, "text/csv", "quality_inspections.csv");
            return StatusCode((int)r.StatusCode, r);
        }

        // --- AI Predictions ---
        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetDemandForecast(int itemId)
        {
            var r = await _svc.GetDemandForecastAsync(SchoolId, itemId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetReorderSuggestions()
        {
            var r = await _svc.GetReorderSuggestionsAsync(SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var r = await _svc.GetCategoryByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryItem(int id)
        {
            var r = await _svc.GetInventoryItemByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVendor(int id)
        {
            var r = await _svc.GetVendorByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequisition(int id)
        {
            var r = await _svc.GetRequisitionByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseOrder(int id)
        {
            var r = await _svc.GetPurchaseOrderByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGrn(int id)
        {
            var r = await _svc.GetGrnByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouse(int id)
        {
            var r = await _svc.GetWarehouseByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouseBin(int id)
        {
            var r = await _svc.GetWarehouseBinByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStore(int id)
        {
            var r = await _svc.GetStoreByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRfq(int id)
        {
            var r = await _svc.GetRfqByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuotation(int id)
        {
            var r = await _svc.GetQuotationByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReturn(int id)
        {
            var r = await _svc.GetReturnByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockIssue(int id)
        {
            var r = await _svc.GetStockIssueByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInspection(int id)
        {
            var r = await _svc.GetInspectionByIdAsync(SchoolId, id);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }
    }
}
