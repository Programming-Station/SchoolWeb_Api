using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School.Infrastructure.Interfaces;
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

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var r = await _svc.GetCategoriesAsync(SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetInventoryItems()
        {
            var r = await _svc.GetInventoryItemsAsync(SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInventoryItem([FromBody] CreateInventoryItemDto dto)
        {
            var r = await _svc.CreateInventoryItemAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetVendors()
        {
            var r = await _svc.GetVendorsAsync(SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVendor([FromBody] CreateVendorDto dto)
        {
            var r = await _svc.CreateVendorAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetRequisitions()
        {
            var r = await _svc.GetRequisitionsAsync(SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequisition([FromBody] CreatePurchaseRequisitionDto dto)
        {
            var r = await _svc.CreateRequisitionAsync(SchoolId, dto, UserName, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{reqId}")]
        public async Task<IActionResult> UpdateRequisitionStatus(int reqId, [FromQuery] string status)
        {
            var r = await _svc.UpdateRequisitionStatusAsync(SchoolId, reqId, status, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrders()
        {
            var r = await _svc.GetPurchaseOrdersAsync(SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderDto dto)
        {
            var r = await _svc.CreatePurchaseOrderAsync(SchoolId, dto, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost("{poId}")]
        public async Task<IActionResult> UpdatePoStatus(int poId, [FromQuery] string status)
        {
            var r = await _svc.UpdatePoStatusAsync(SchoolId, poId, status, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet]
        public async Task<IActionResult> GetGrns()
        {
            var r = await _svc.GetGrnsAsync(SchoolId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveGoods([FromBody] CreateGrnDto dto)
        {
            var r = await _svc.ReceiveGoodsAsync(SchoolId, dto, UserName, UserName);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetStockTransactions(int itemId)
        {
            var r = await _svc.GetStockTransactionsAsync(SchoolId, itemId);
            return r.Success ? Ok(r) : StatusCode((int)r.StatusCode, r);
        }

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
    }
}
