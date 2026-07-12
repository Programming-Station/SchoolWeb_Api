using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Inventory;
using School.Infrastructure;

namespace School.Infrastructure.Seeds
{
    public class DefaultInventoryData
    {
        public static async Task SeedAsync(SchoolDbContext context)
        {
            var school = await context.SchoolRegistrations.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (school == null) return;

            int schoolId = school.Id;

            // Fetch asset, expense, and creditor accounts
            var stockAsset = await context.CoaAccounts.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.SchoolRegistrationId == schoolId && a.Code == "12000");
            var expenseAcc = await context.CoaAccounts.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.SchoolRegistrationId == schoolId && a.Code == "51000");
            var creditorAcc = await context.CoaAccounts.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.SchoolRegistrationId == schoolId && a.Code == "20000");

            // 1. Item Categories
            if (!await context.ItemCategories.IgnoreQueryFilters().AnyAsync(c => c.SchoolRegistrationId == schoolId))
            {
                var stationery = new ItemCategory { Name = "Stationery Supplies", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var labs = new ItemCategory { Name = "Science Lab Equipment", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var assets = new ItemCategory { Name = "IT Hardware & Assets", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };
                var sports = new ItemCategory { Name = "Sports Equipment", SchoolRegistrationId = schoolId, CreatedDate = DateTime.UtcNow };

                context.ItemCategories.AddRange(stationery, labs, assets, sports);
                await context.SaveChangesAsync();

                // 2. Inventory Items
                var notepad = new InventoryItem
                {
                    Sku = "ST-NOTEPAD-01",
                    Name = "Executive Notepad A4",
                    CategoryId = stationery.Id,
                    Uom = "pcs",
                    MinStockLevel = 10,
                    CurrentStock = 120,
                    UnitPrice = 45.00m,
                    AssetAccountId = stockAsset?.Id,
                    ExpenseAccountId = expenseAcc?.Id,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow
                };

                var beaker = new InventoryItem
                {
                    Sku = "LB-BEAKER-250",
                    Name = "Borosilicate Beaker 250ml",
                    CategoryId = labs.Id,
                    Uom = "pcs",
                    MinStockLevel = 5,
                    CurrentStock = 35,
                    UnitPrice = 120.00m,
                    AssetAccountId = stockAsset?.Id,
                    ExpenseAccountId = expenseAcc?.Id,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow
                };

                var laptop = new InventoryItem
                {
                    Sku = "IT-LAPTOP-LNV",
                    Name = "Lenovo V15 Core i5 Laptop",
                    CategoryId = assets.Id,
                    Uom = "pcs",
                    MinStockLevel = 2,
                    CurrentStock = 15,
                    UnitPrice = 45000.00m,
                    AssetAccountId = stockAsset?.Id,
                    ExpenseAccountId = expenseAcc?.Id,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow
                };

                var basketball = new InventoryItem
                {
                    Sku = "SP-BASKETBALL-07",
                    Name = "Spalding Size 7 Basketball",
                    CategoryId = sports.Id,
                    Uom = "pcs",
                    MinStockLevel = 4,
                    CurrentStock = 8,
                    UnitPrice = 1500.00m,
                    AssetAccountId = stockAsset?.Id,
                    ExpenseAccountId = expenseAcc?.Id,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow
                };

                context.InventoryItems.AddRange(notepad, beaker, laptop, basketball);
                await context.SaveChangesAsync();

                // 3. Vendors
                var vendorStationery = new Vendor
                {
                    Code = "VND-DELHI-01",
                    Name = "Delhi Books and Stationers Ltd",
                    ContactPerson = "Amit Sharma",
                    Email = "sales@delhibooks.com",
                    Phone = "011-4521635",
                    TaxRegistrationNo = "07AAAAA1111A1Z1",
                    CreditorAccountId = creditorAcc?.Id,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow
                };

                var vendorIT = new Vendor
                {
                    Code = "VND-TECHNO-02",
                    Name = "Techno Computers Private Ltd",
                    ContactPerson = "Rajesh Gupta",
                    Email = "contracts@technopc.com",
                    Phone = "022-7894561",
                    TaxRegistrationNo = "27BBBBB2222B2Z2",
                    CreditorAccountId = creditorAcc?.Id,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow
                };

                context.Vendors.AddRange(vendorStationery, vendorIT);
                await context.SaveChangesAsync();

                // 4. Warehouses & Bins
                var whMain = new Warehouse
                {
                    Code = "WH-MAIN",
                    Name = "Central School Warehouse",
                    Address = "Campus Gate No. 2, Ground Floor",
                    Capacity = 1500,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow
                };
                context.Warehouses.Add(whMain);
                await context.SaveChangesAsync();

                var binA1 = new WarehouseBin
                {
                    WarehouseId = whMain.Id,
                    Zone = "A",
                    Rack = "1",
                    Shelf = "A",
                    BinCode = "WH-MAIN-A-1-A",
                    Capacity = 100,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow
                };
                var binB2 = new WarehouseBin
                {
                    WarehouseId = whMain.Id,
                    Zone = "B",
                    Rack = "2",
                    Shelf = "B",
                    BinCode = "WH-MAIN-B-2-B",
                    Capacity = 100,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow
                };
                context.WarehouseBins.AddRange(binA1, binB2);

                // 5. Stores
                var storeCentral = new Store
                {
                    Code = "STR-CEN",
                    Name = "Central Stationery Store",
                    StoreType = "General",
                    ContactPerson = "Mohan Lal",
                    IsActive = true,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow
                };
                var storeScience = new Store
                {
                    Code = "STR-SCI",
                    Name = "Science Labs Sub-Store",
                    StoreType = "Chemistry & Physics Labs",
                    ContactPerson = "Dr. Anita Roy",
                    IsActive = true,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow
                };
                context.Stores.AddRange(storeCentral, storeScience);
                await context.SaveChangesAsync();

                // 6. Purchase Requisitions & Items
                var dept = await context.Departments.IgnoreQueryFilters().FirstOrDefaultAsync(d => d.SchoolRegistrationId == schoolId);
                int deptId = dept?.Id ?? 1;

                var req = new PurchaseRequisition
                {
                    RequisitionNo = "PR-2026-001",
                    RequestedBy = "Prof. Ramesh Chandra",
                    RequestDate = DateTime.UtcNow.AddDays(-10),
                    DepartmentId = deptId,
                    Remarks = "Urgent requirement for A4 papers and Borosilicate Lab beakers.",
                    Status = "Approved",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-10)
                };
                context.PurchaseRequisitions.Add(req);
                await context.SaveChangesAsync();

                var reqItem1 = new PurchaseRequisitionItem
                {
                    PurchaseRequisitionId = req.Id,
                    ItemId = notepad.Id,
                    Quantity = 50,
                    EstimatedCost = 45.00m,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-10)
                };
                var reqItem2 = new PurchaseRequisitionItem
                {
                    PurchaseRequisitionId = req.Id,
                    ItemId = beaker.Id,
                    Quantity = 15,
                    EstimatedCost = 120.00m,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-10)
                };
                context.PurchaseRequisitionItems.AddRange(reqItem1, reqItem2);
                await context.SaveChangesAsync();

                // 7. Request For Quotation (RFQ)
                var rfq = new RequestForQuotation
                {
                    RfqNo = "RFQ-2026-001",
                    RequisitionId = req.Id,
                    RfqDate = DateTime.UtcNow.AddDays(-8),
                    Status = "Closed",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-8)
                };
                context.RequestForQuotations.Add(rfq);
                await context.SaveChangesAsync();

                // 8. Vendor Quotations
                var bid = new VendorQuotation
                {
                    RfqId = rfq.Id,
                    VendorId = vendorStationery.Id,
                    QuoteDate = DateTime.UtcNow.AddDays(-6),
                    TotalAmount = 4050.00m, // (50 * 45) + (15 * 120) = 4050
                    Status = "Accepted",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-6)
                };
                context.VendorQuotations.Add(bid);
                await context.SaveChangesAsync();

                // 9. Purchase Order (PO)
                var po = new PurchaseOrder
                {
                    PoNumber = "PO-2026-001",
                    VendorId = vendorStationery.Id,
                    OrderDate = DateTime.UtcNow.AddDays(-5),
                    DeliveryDate = DateTime.UtcNow.AddDays(2),
                    TaxPercentage = 18.00m,
                    DiscountAmount = 0.00m,
                    TotalAmount = 4779.00m, // 4050 + 18% Tax
                    Status = "Approved",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-5)
                };
                context.PurchaseOrders.Add(po);
                await context.SaveChangesAsync();

                var poItem1 = new PurchaseOrderItem
                {
                    PurchaseOrderId = po.Id,
                    ItemId = notepad.Id,
                    QuantityOrdered = 50,
                    QuantityReceived = 50,
                    UnitPrice = 45.00m,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-5)
                };
                var poItem2 = new PurchaseOrderItem
                {
                    PurchaseOrderId = po.Id,
                    ItemId = beaker.Id,
                    QuantityOrdered = 15,
                    QuantityReceived = 15,
                    UnitPrice = 120.00m,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-5)
                };
                context.PurchaseOrderItems.AddRange(poItem1, poItem2);
                await context.SaveChangesAsync();

                // 10. Goods Receipt Note (GRN)
                var grn = new GoodsReceiptNote
                {
                    GrnNumber = "GRN-2026-001",
                    PurchaseOrderId = po.Id,
                    ReceivedDate = DateTime.UtcNow.AddDays(-2),
                    InvoiceNo = "INV-DELHI-4587",
                    ReceivedBy = "Mohan Lal",
                    Status = "Accepted",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-2)
                };
                context.GoodsReceiptNotes.Add(grn);
                await context.SaveChangesAsync();

                var grnItem1 = new GoodsReceiptNoteItem
                {
                    GoodsReceiptNoteId = grn.Id,
                    ItemId = notepad.Id,
                    QuantityAccepted = 50,
                    QuantityRejected = 0,
                    UnitPrice = 45.00m,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-2)
                };
                var grnItem2 = new GoodsReceiptNoteItem
                {
                    GoodsReceiptNoteId = grn.Id,
                    ItemId = beaker.Id,
                    QuantityAccepted = 15,
                    QuantityRejected = 0,
                    UnitPrice = 120.00m,
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-2)
                };
                context.GoodsReceiptNoteItems.AddRange(grnItem1, grnItem2);
                await context.SaveChangesAsync();

                // 11. Quality Inspection
                var qa = new QualityInspection
                {
                    GrnId = grn.Id,
                    ItemId = beaker.Id,
                    QuantityInspected = 15,
                    QuantityAccepted = 15,
                    QuantityRejected = 0,
                    InspectionReport = "Borosilicate glass materials checked. Fully intact and certified.",
                    QualityScore = 95,
                    InspectedBy = "Dr. Anita Roy",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-2)
                };
                context.QualityInspections.Add(qa);

                // 12. Purchase Return
                var ret = new PurchaseReturn
                {
                    ReturnNo = "RTN-2026-001",
                    GrnId = grn.Id,
                    ReturnDate = DateTime.UtcNow.AddDays(-1),
                    TotalAmount = 0.00m,
                    Reason = "No damaged products. Logged as sample receipt return template.",
                    CreditNoteNo = "CN-DELHI-00",
                    RefundAmount = 0.00m,
                    Status = "Completed",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-1)
                };
                context.PurchaseReturns.Add(ret);

                // 13. Stock Issue (Internal Allocation)
                var issue = new StockIssue
                {
                    IssueNo = "SIS-2026-001",
                    ItemId = beaker.Id,
                    Quantity = 5,
                    IssuedToType = "Staff",
                    IssuedToId = 101,
                    IssueDate = DateTime.UtcNow.AddDays(-1),
                    Returnable = true,
                    DueDate = DateTime.UtcNow.AddDays(30),
                    Status = "Issued",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-1)
                };
                context.StockIssues.Add(issue);

                // 14. Stock Transactions Logs
                var tr1 = new StockTransaction
                {
                    ItemId = notepad.Id,
                    TransactionType = "Inward (GRN)",
                    Quantity = 50,
                    ReferenceNo = "GRN-2026-001",
                    TransactionDate = DateTime.UtcNow.AddDays(-2),
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-2)
                };
                var tr2 = new StockTransaction
                {
                    ItemId = beaker.Id,
                    TransactionType = "Inward (GRN)",
                    Quantity = 15,
                    ReferenceNo = "GRN-2026-001",
                    TransactionDate = DateTime.UtcNow.AddDays(-2),
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-2)
                };
                var tr3 = new StockTransaction
                {
                    ItemId = beaker.Id,
                    TransactionType = "Outward (Issue)",
                    Quantity = 5,
                    ReferenceNo = "SIS-2026-001",
                    TransactionDate = DateTime.UtcNow.AddDays(-1),
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-1)
                };
                context.StockTransactions.AddRange(tr1, tr2, tr3);

                // 15. Fixed Asset Depreciation Log
                var dep = new AssetDepreciationLog
                {
                    ItemId = laptop.Id,
                    DepreciationDate = DateTime.UtcNow.AddDays(-30),
                    DepreciationAmount = 4500.00m,
                    BookValueAfterDepreciation = 40500.00m,
                    Remarks = "Monthly depreciation post for IT laptops.",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-30)
                };
                context.AssetDepreciationLogs.Add(dep);

                // 16. Asset Maintenance Log
                var maint = new AssetMaintenanceLog
                {
                    ItemId = laptop.Id,
                    MaintenanceDate = DateTime.UtcNow.AddDays(-15),
                    MaintenanceType = "Routine Service",
                    Cost = 1200.00m,
                    PerformedBy = "Suresh Kumar",
                    ServiceDetails = "RAM upgrade and software cleanups completed successfully (Lenovo Support Desk).",
                    SchoolRegistrationId = schoolId,
                    CreatedDate = DateTime.UtcNow.AddDays(-15)
                };
                context.AssetMaintenanceLogs.Add(maint);

                await context.SaveChangesAsync();
            }
        }
    }
}
