using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.Domain.Inventory;
using School.Domain.Finance;
using School.Infrastructure;
using School.Services.Interfaces;
using School_DTOs;
using School_DTOs.Inventory;

namespace School.Services.Inventory
{
    public class ProcurementService : IProcurementService
    {
        private readonly SchoolDbContext _context;

        public ProcurementService(SchoolDbContext context)
        {
            _context = context;
        }

        // --- Item Categories ---
        public async Task<APIResponse<List<ItemCategoryDto>>> GetCategoriesAsync(int schoolId)
        {
            var cats = await _context.ItemCategories
                .Include(c => c.ParentCategory)
                .Where(c => c.SchoolRegistrationId == schoolId && !c.IsDeleted)
                .ToListAsync();

            var list = cats.Select(c => new ItemCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentCategoryId = c.ParentCategoryId,
                ParentCategoryName = c.ParentCategory?.Name
            }).ToList();

            return new APIResponse<List<ItemCategoryDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = list };
        }

        // --- Inventory Items ---
        public async Task<APIResponse<List<InventoryItemDto>>> GetInventoryItemsAsync(int schoolId)
        {
            var items = await _context.InventoryItems
                .Include(i => i.Category)
                .Include(i => i.AssetAccount)
                .Include(i => i.ExpenseAccount)
                .Where(i => i.SchoolRegistrationId == schoolId && !i.IsDeleted)
                .ToListAsync();

            var list = items.Select(i => new InventoryItemDto
            {
                Id = i.Id,
                Sku = i.Sku,
                Name = i.Name,
                CategoryId = i.CategoryId,
                CategoryName = i.Category.Name,
                Uom = i.Uom,
                MinStockLevel = i.MinStockLevel,
                CurrentStock = i.CurrentStock,
                UnitPrice = i.UnitPrice,
                AssetAccountId = i.AssetAccountId,
                AssetAccountName = i.AssetAccount?.Name,
                ExpenseAccountId = i.ExpenseAccountId,
                ExpenseAccountName = i.ExpenseAccount?.Name
            }).ToList();

            return new APIResponse<List<InventoryItemDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = list };
        }

        public async Task<APIResponse<InventoryItemDto>> CreateInventoryItemAsync(int schoolId, CreateInventoryItemDto dto, string user)
        {
            var exists = await _context.InventoryItems
                .AnyAsync(i => i.SchoolRegistrationId == schoolId && i.Sku == dto.Sku && !i.IsDeleted);

            if (exists)
            {
                return new APIResponse<InventoryItemDto> { Success = false, Message = $"SKU '{dto.Sku}' already exists.", StatusCode = HttpStatusCode.BadRequest };
            }

            var item = new InventoryItem
            {
                Sku = dto.Sku,
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                Uom = dto.Uom,
                MinStockLevel = dto.MinStockLevel,
                CurrentStock = 0,
                UnitPrice = dto.UnitPrice,
                AssetAccountId = dto.AssetAccountId,
                ExpenseAccountId = dto.ExpenseAccountId,
                SchoolRegistrationId = schoolId,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };

            _context.InventoryItems.Add(item);
            await _context.SaveChangesAsync();

            return new APIResponse<InventoryItemDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = "Inventory item cataloged",
                Data = new InventoryItemDto { Id = item.Id, Sku = item.Sku, Name = item.Name }
            };
        }

        // --- Vendors ---
        public async Task<APIResponse<List<VendorDto>>> GetVendorsAsync(int schoolId)
        {
            var vendors = await _context.Vendors
                .Include(v => v.CreditorAccount)
                .Where(v => v.SchoolRegistrationId == schoolId && !v.IsDeleted)
                .ToListAsync();

            var list = vendors.Select(v => new VendorDto
            {
                Id = v.Id,
                Code = v.Code,
                Name = v.Name,
                ContactPerson = v.ContactPerson,
                Email = v.Email,
                Phone = v.Phone,
                TaxRegistrationNo = v.TaxRegistrationNo,
                CreditorAccountId = v.CreditorAccountId,
                CreditorAccountName = v.CreditorAccount?.Name
            }).ToList();

            return new APIResponse<List<VendorDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = list };
        }

        public async Task<APIResponse<VendorDto>> CreateVendorAsync(int schoolId, CreateVendorDto dto, string user)
        {
            var vendor = new Vendor
            {
                Code = dto.Code,
                Name = dto.Name,
                ContactPerson = dto.ContactPerson,
                Email = dto.Email,
                Phone = dto.Phone,
                TaxRegistrationNo = dto.TaxRegistrationNo,
                CreditorAccountId = dto.CreditorAccountId,
                SchoolRegistrationId = schoolId,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };

            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return new APIResponse<VendorDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = "Vendor registered successfully",
                Data = new VendorDto { Id = vendor.Id, Code = vendor.Code, Name = vendor.Name }
            };
        }

        // --- Purchase Requisitions ---
        public async Task<APIResponse<List<PurchaseRequisitionDto>>> GetRequisitionsAsync(int schoolId)
        {
            var reqs = await _context.PurchaseRequisitions
                .Include(r => r.Department)
                .Include(r => r.Items)
                .ThenInclude(ri => ri.Item)
                .Where(r => r.SchoolRegistrationId == schoolId && !r.IsDeleted)
                .ToListAsync();

            var list = reqs.Select(r => new PurchaseRequisitionDto
            {
                Id = r.Id,
                RequisitionNo = r.RequisitionNo,
                RequestedBy = r.RequestedBy,
                RequestDate = r.RequestDate,
                DepartmentId = r.DepartmentId,
                DepartmentName = r.Department.Name,
                Remarks = r.Remarks,
                Status = r.Status,
                Items = r.Items.Select(ri => new PurchaseRequisitionItemDto
                {
                    Id = ri.Id,
                    ItemId = ri.ItemId,
                    ItemName = ri.Item.Name,
                    ItemSku = ri.Item.Sku,
                    Quantity = ri.Quantity,
                    EstimatedCost = ri.EstimatedCost
                }).ToList()
            }).ToList();

            return new APIResponse<List<PurchaseRequisitionDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = list };
        }

        public async Task<APIResponse<PurchaseRequisitionDto>> CreateRequisitionAsync(int schoolId, CreatePurchaseRequisitionDto dto, string requester, string user)
        {
            var count = await _context.PurchaseRequisitions.IgnoreQueryFilters().CountAsync(r => r.SchoolRegistrationId == schoolId);
            string reqNo = $"REQ-{DateTime.UtcNow.Year}-{(count + 1).ToString().PadLeft(5, '0')}";

            var req = new PurchaseRequisition
            {
                RequisitionNo = reqNo,
                RequestedBy = requester,
                RequestDate = DateTime.UtcNow,
                DepartmentId = dto.DepartmentId,
                Remarks = dto.Remarks,
                Status = "Draft",
                SchoolRegistrationId = schoolId,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };

            foreach (var item in dto.Items)
            {
                req.Items.Add(new PurchaseRequisitionItem
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    EstimatedCost = item.EstimatedCost,
                    SchoolRegistrationId = schoolId,
                    CreatedBy = user,
                    CreatedDate = DateTime.UtcNow
                });
            }

            _context.PurchaseRequisitions.Add(req);
            await _context.SaveChangesAsync();

            return new APIResponse<PurchaseRequisitionDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = $"Requisition {reqNo} created",
                Data = new PurchaseRequisitionDto { Id = req.Id, RequisitionNo = req.RequisitionNo }
            };
        }

        public async Task<APIResponse<bool>> UpdateRequisitionStatusAsync(int schoolId, int reqId, string status, string user)
        {
            var req = await _context.PurchaseRequisitions
                .FirstOrDefaultAsync(r => r.SchoolRegistrationId == schoolId && r.Id == reqId && !r.IsDeleted);

            if (req == null)
            {
                return new APIResponse<bool> { Success = false, Message = "Requisition not found", StatusCode = HttpStatusCode.NotFound };
            }

            req.Status = status;
            req.UpdatedBy = user;
            req.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = $"Requisition status marked as {status}", Data = true };
        }

        // --- Purchase Orders ---
        public async Task<APIResponse<List<PurchaseOrderDto>>> GetPurchaseOrdersAsync(int schoolId)
        {
            var pos = await _context.PurchaseOrders
                .Include(p => p.Vendor)
                .Include(p => p.Items)
                .ThenInclude(pi => pi.Item)
                .Where(p => p.SchoolRegistrationId == schoolId && !p.IsDeleted)
                .ToListAsync();

            var list = pos.Select(p => new PurchaseOrderDto
            {
                Id = p.Id,
                PoNumber = p.PoNumber,
                VendorId = p.VendorId,
                VendorName = p.Vendor.Name,
                OrderDate = p.OrderDate,
                DeliveryDate = p.DeliveryDate,
                TaxPercentage = p.TaxPercentage,
                DiscountAmount = p.DiscountAmount,
                TotalAmount = p.TotalAmount,
                Status = p.Status,
                Items = p.Items.Select(pi => new PurchaseOrderItemDto
                {
                    Id = pi.Id,
                    ItemId = pi.ItemId,
                    ItemName = pi.Item.Name,
                    QuantityOrdered = pi.QuantityOrdered,
                    QuantityReceived = pi.QuantityReceived,
                    UnitPrice = pi.UnitPrice
                }).ToList()
            }).ToList();

            return new APIResponse<List<PurchaseOrderDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = list };
        }

        public async Task<APIResponse<PurchaseOrderDto>> CreatePurchaseOrderAsync(int schoolId, CreatePurchaseOrderDto dto, string user)
        {
            var count = await _context.PurchaseOrders.IgnoreQueryFilters().CountAsync(p => p.SchoolRegistrationId == schoolId);
            string poNo = $"PO-{DateTime.UtcNow.Year}-{(count + 1).ToString().PadLeft(5, '0')}";

            decimal subtotal = dto.Items.Sum(i => i.QuantityOrdered * i.UnitPrice);
            decimal taxAmount = subtotal * (dto.TaxPercentage / 100);
            decimal grandTotal = subtotal + taxAmount - dto.DiscountAmount;

            var po = new PurchaseOrder
            {
                PoNumber = poNo,
                VendorId = dto.VendorId,
                OrderDate = DateTime.UtcNow,
                DeliveryDate = dto.DeliveryDate,
                TaxPercentage = dto.TaxPercentage,
                DiscountAmount = dto.DiscountAmount,
                TotalAmount = grandTotal,
                Status = "Draft",
                SchoolRegistrationId = schoolId,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };

            foreach (var item in dto.Items)
            {
                po.Items.Add(new PurchaseOrderItem
                {
                    ItemId = item.ItemId,
                    QuantityOrdered = item.QuantityOrdered,
                    QuantityReceived = 0,
                    UnitPrice = item.UnitPrice,
                    SchoolRegistrationId = schoolId,
                    CreatedBy = user,
                    CreatedDate = DateTime.UtcNow
                });
            }

            _context.PurchaseOrders.Add(po);
            await _context.SaveChangesAsync();

            return new APIResponse<PurchaseOrderDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = $"Purchase Order {poNo} drafted",
                Data = new PurchaseOrderDto { Id = po.Id, PoNumber = po.PoNumber }
            };
        }

        public async Task<APIResponse<bool>> UpdatePoStatusAsync(int schoolId, int poId, string status, string user)
        {
            var po = await _context.PurchaseOrders
                .FirstOrDefaultAsync(p => p.SchoolRegistrationId == schoolId && p.Id == poId && !p.IsDeleted);

            if (po == null)
            {
                return new APIResponse<bool> { Success = false, Message = "PO not found", StatusCode = HttpStatusCode.NotFound };
            }

            po.Status = status;
            po.UpdatedBy = user;
            po.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new APIResponse<bool> { Success = true, StatusCode = HttpStatusCode.OK, Message = $"PO marked as {status}", Data = true };
        }

        // --- Goods Receipt Notes & Accounting Integration ---
        public async Task<APIResponse<List<GoodsReceiptNoteDto>>> GetGrnsAsync(int schoolId)
        {
            var grns = await _context.GoodsReceiptNotes
                .Include(g => g.PurchaseOrder)
                .ThenInclude(po => po.Vendor)
                .Include(g => g.Items)
                .ThenInclude(gi => gi.Item)
                .Where(g => g.SchoolRegistrationId == schoolId && !g.IsDeleted)
                .ToListAsync();

            var list = grns.Select(g => new GoodsReceiptNoteDto
            {
                Id = g.Id,
                GrnNumber = g.GrnNumber,
                PurchaseOrderId = g.PurchaseOrderId,
                PoNumber = g.PurchaseOrder.PoNumber,
                VendorName = g.PurchaseOrder.Vendor.Name,
                ReceivedDate = g.ReceivedDate,
                InvoiceNo = g.InvoiceNo,
                ReceivedBy = g.ReceivedBy,
                Status = g.Status,
                Items = g.Items.Select(gi => new GoodsReceiptNoteItemDto
                {
                    Id = gi.Id,
                    ItemId = gi.ItemId,
                    ItemName = gi.Item.Name,
                    QuantityAccepted = gi.QuantityAccepted,
                    QuantityRejected = gi.QuantityRejected,
                    UnitPrice = gi.UnitPrice
                }).ToList()
            }).ToList();

            return new APIResponse<List<GoodsReceiptNoteDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = list };
        }

        public async Task<APIResponse<GoodsReceiptNoteDto>> ReceiveGoodsAsync(int schoolId, CreateGrnDto dto, string receiver, string user)
        {
            var po = await _context.PurchaseOrders
                .Include(p => p.Vendor)
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.SchoolRegistrationId == schoolId && p.Id == dto.PurchaseOrderId && !p.IsDeleted);

            if (po == null)
            {
                return new APIResponse<GoodsReceiptNoteDto> { Success = false, Message = "Purchase Order not found", StatusCode = HttpStatusCode.NotFound };
            }

            var count = await _context.GoodsReceiptNotes.IgnoreQueryFilters().CountAsync(g => g.SchoolRegistrationId == schoolId);
            string grnNo = $"GRN-{DateTime.UtcNow.Year}-{(count + 1).ToString().PadLeft(5, '0')}";

            var grn = new GoodsReceiptNote
            {
                GrnNumber = grnNo,
                PurchaseOrderId = dto.PurchaseOrderId,
                ReceivedDate = DateTime.UtcNow,
                InvoiceNo = dto.InvoiceNo,
                ReceivedBy = receiver,
                Status = "Accepted",
                SchoolRegistrationId = schoolId,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };

            decimal totalCost = 0;

            foreach (var item in dto.Items)
            {
                // Increment current inventory quantity
                var dbItem = await _context.InventoryItems.FindAsync(item.ItemId);
                if (dbItem != null)
                {
                    dbItem.CurrentStock += item.QuantityAccepted;
                    dbItem.UpdatedBy = user;
                    dbItem.UpdatedDate = DateTime.UtcNow;

                    // Log stock transactions ledger
                    _context.StockTransactions.Add(new StockTransaction
                    {
                        ItemId = item.ItemId,
                        TransactionType = "Inward",
                        Quantity = item.QuantityAccepted,
                        ReferenceNo = grnNo,
                        TransactionDate = DateTime.UtcNow,
                        WarehouseLoc = "Main Store",
                        SchoolRegistrationId = schoolId,
                        CreatedBy = user,
                        CreatedDate = DateTime.UtcNow
                    });
                }

                // Update PO received quantity
                var poItem = po.Items.FirstOrDefault(pi => pi.ItemId == item.ItemId);
                if (poItem != null)
                {
                    poItem.QuantityReceived += item.QuantityAccepted;
                    poItem.UpdatedBy = user;
                    poItem.UpdatedDate = DateTime.UtcNow;
                }

                grn.Items.Add(new GoodsReceiptNoteItem
                {
                    ItemId = item.ItemId,
                    QuantityAccepted = item.QuantityAccepted,
                    QuantityRejected = item.QuantityRejected,
                    UnitPrice = item.UnitPrice,
                    SchoolRegistrationId = schoolId,
                    CreatedBy = user,
                    CreatedDate = DateTime.UtcNow
                });

                totalCost += (item.QuantityAccepted * item.UnitPrice);
            }

            // Mark PO as received
            bool allReceived = po.Items.All(pi => pi.QuantityReceived >= pi.QuantityOrdered);
            po.Status = allReceived ? "FullyReceived" : "PartiallyReceived";
            po.UpdatedBy = user;
            po.UpdatedDate = DateTime.UtcNow;

            _context.GoodsReceiptNotes.Add(grn);

            // --- DOUBLE ENTRY JOURNAL INTEGRATION ---
            // If the first received item has Asset/Expense account mapped & Vendor has Creditor account mapped,
            // we write balanced Debit/Credit journal postings!
            var firstItem = dto.Items.FirstOrDefault();
            if (firstItem != null)
            {
                var dbItem = await _context.InventoryItems.FindAsync(firstItem.ItemId);
                int? debitAccId = dbItem?.AssetAccountId ?? dbItem?.ExpenseAccountId;
                int? creditAccId = po.Vendor.CreditorAccountId;

                if (debitAccId.HasValue && creditAccId.HasValue && totalCost > 0)
                {
                    var jCount = await _context.JournalEntries.IgnoreQueryFilters().CountAsync(j => j.SchoolRegistrationId == schoolId);
                    string voucherNo = $"JV-{DateTime.UtcNow.Year}-{(jCount + 1).ToString().PadLeft(5, '0')}";

                    var journal = new JournalEntry
                    {
                        VoucherNo = voucherNo,
                        EntryDate = DateTime.UtcNow,
                        Narration = $"Inventory received via GRN {grnNo} for PO {po.PoNumber}. Vendor: {po.Vendor.Name}.",
                        Reference = grnNo,
                        Source = "Procurement",
                        IsPosted = true,
                        SchoolRegistrationId = schoolId,
                        CreatedBy = user,
                        CreatedDate = DateTime.UtcNow
                    };

                    // Debit: Asset Ledger
                    var dAccount = await _context.CoaAccounts.FindAsync(debitAccId.Value);
                    if (dAccount != null)
                    {
                        dAccount.CurrentBalance += totalCost;
                        journal.Lines.Add(new JournalEntryLine
                        {
                            AccountId = debitAccId.Value,
                            DebitAmount = totalCost,
                            CreditAmount = 0,
                            Description = $"Inward stock asset capitalization - {grnNo}",
                            SchoolRegistrationId = schoolId,
                            CreatedBy = user,
                            CreatedDate = DateTime.UtcNow
                        });
                    }

                    // Credit: Vendor Accounts Payable Ledger
                    var cAccount = await _context.CoaAccounts.FindAsync(creditAccId.Value);
                    if (cAccount != null)
                    {
                        cAccount.CurrentBalance += totalCost;
                        journal.Lines.Add(new JournalEntryLine
                        {
                            AccountId = creditAccId.Value,
                            DebitAmount = 0,
                            CreditAmount = totalCost,
                            Description = $"Accounts payable credit to supplier - {po.Vendor.Name}",
                            SchoolRegistrationId = schoolId,
                            CreatedBy = user,
                            CreatedDate = DateTime.UtcNow
                        });
                    }

                    _context.JournalEntries.Add(journal);
                }
            }

            await _context.SaveChangesAsync();

            return new APIResponse<GoodsReceiptNoteDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = $"Goods receipted successfully. GRN: {grnNo}.",
                Data = new GoodsReceiptNoteDto { Id = grn.Id, GrnNumber = grn.GrnNumber }
            };
        }

        // --- Stock Transactions ---
        public async Task<APIResponse<List<StockTransactionDto>>> GetStockTransactionsAsync(int schoolId, int itemId)
        {
            var txns = await _context.StockTransactions
                .Include(t => t.Item)
                .Where(t => t.SchoolRegistrationId == schoolId && t.ItemId == itemId && !t.IsDeleted)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            var list = txns.Select(t => new StockTransactionDto
            {
                Id = t.Id,
                ItemId = t.ItemId,
                ItemName = t.Item.Name,
                TransactionType = t.TransactionType,
                Quantity = t.Quantity,
                ReferenceNo = t.ReferenceNo,
                TransactionDate = t.TransactionDate,
                WarehouseLoc = t.WarehouseLoc
            }).ToList();

            return new APIResponse<List<StockTransactionDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = list };
        }

        // --- Fixed Asset Depreciation ---
        public async Task<APIResponse<List<AssetDepreciationLogDto>>> GetAssetDepreciationsAsync(int schoolId, int itemId)
        {
            var logs = await _context.AssetDepreciationLogs
                .Include(d => d.Item)
                .Where(d => d.SchoolRegistrationId == schoolId && d.ItemId == itemId && !d.IsDeleted)
                .OrderByDescending(d => d.DepreciationDate)
                .ToListAsync();

            var list = logs.Select(d => new AssetDepreciationLogDto
            {
                Id = d.Id,
                ItemId = d.ItemId,
                ItemName = d.Item.Name,
                DepreciationDate = d.DepreciationDate,
                DepreciationAmount = d.DepreciationAmount,
                BookValueAfterDepreciation = d.BookValueAfterDepreciation,
                Remarks = d.Remarks
            }).ToList();

            return new APIResponse<List<AssetDepreciationLogDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = list };
        }

        public async Task<APIResponse<AssetDepreciationLogDto>> CalculateDepreciationAsync(int schoolId, int itemId, decimal ratePercent, string remarks, string user)
        {
            var item = await _context.InventoryItems.FindAsync(itemId);
            if (item == null || item.SchoolRegistrationId != schoolId || !item.AssetAccountId.HasValue)
            {
                return new APIResponse<AssetDepreciationLogDto> { Success = false, Message = "Asset item not found or lacks asset ledger details.", StatusCode = HttpStatusCode.BadRequest };
            }

            decimal initialValue = item.UnitPrice * item.CurrentStock;
            if (initialValue <= 0)
            {
                return new APIResponse<AssetDepreciationLogDto> { Success = false, Message = "No asset value found to calculate depreciation.", StatusCode = HttpStatusCode.BadRequest };
            }

            decimal depAmount = Math.Round(initialValue * (ratePercent / 100), 2);
            decimal netBookValue = initialValue - depAmount;

            var log = new AssetDepreciationLog
            {
                ItemId = itemId,
                DepreciationDate = DateTime.UtcNow,
                DepreciationAmount = depAmount,
                BookValueAfterDepreciation = netBookValue,
                Remarks = remarks,
                SchoolRegistrationId = schoolId,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };

            // Write Balanced Journal Vouchers:
            // Debit: Depreciation Expense Account
            // Credit: Asset Account (reducing asset valuation ledger balance)
            var expenseAccount = await _context.CoaAccounts
                .FirstOrDefaultAsync(a => a.SchoolRegistrationId == schoolId && a.AccountType.Equals("Expense", StringComparison.OrdinalIgnoreCase) && a.Name.Contains("Depreciation"));

            if (expenseAccount != null && item.AssetAccountId.HasValue && depAmount > 0)
            {
                var jCount = await _context.JournalEntries.IgnoreQueryFilters().CountAsync(j => j.SchoolRegistrationId == schoolId);
                string voucherNo = $"JV-{DateTime.UtcNow.Year}-{(jCount + 1).ToString().PadLeft(5, '0')}";

                var journal = new JournalEntry
                {
                    VoucherNo = voucherNo,
                    EntryDate = DateTime.UtcNow,
                    Narration = $"Depreciation booked for asset {item.Name} at rate {ratePercent}%.",
                    Reference = $"DEP-ITEM-{itemId}",
                    Source = "Procurement",
                    IsPosted = true,
                    SchoolRegistrationId = schoolId,
                    CreatedBy = user,
                    CreatedDate = DateTime.UtcNow
                };

                // Debit: Depreciation Expense Ledger
                expenseAccount.CurrentBalance += depAmount;
                journal.Lines.Add(new JournalEntryLine
                {
                    AccountId = expenseAccount.Id,
                    DebitAmount = depAmount,
                    CreditAmount = 0,
                    Description = $"Depreciation Expense - {item.Name}",
                    SchoolRegistrationId = schoolId,
                    CreatedBy = user,
                    CreatedDate = DateTime.UtcNow
                });

                // Credit: Capital Stock Asset Ledger
                var assetAccount = await _context.CoaAccounts.FindAsync(item.AssetAccountId.Value);
                if (assetAccount != null)
                {
                    assetAccount.CurrentBalance -= depAmount;
                    journal.Lines.Add(new JournalEntryLine
                    {
                        AccountId = item.AssetAccountId.Value,
                        DebitAmount = 0,
                        CreditAmount = depAmount,
                        Description = $"Accumulated Asset Valuation Reduction",
                        SchoolRegistrationId = schoolId,
                        CreatedBy = user,
                        CreatedDate = DateTime.UtcNow
                    });
                }

                _context.JournalEntries.Add(journal);
            }

            _context.AssetDepreciationLogs.Add(log);
            await _context.SaveChangesAsync();

            return new APIResponse<AssetDepreciationLogDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = "Asset depreciation written",
                Data = new AssetDepreciationLogDto
                {
                    Id = log.Id,
                    ItemId = log.ItemId,
                    DepreciationAmount = log.DepreciationAmount,
                    BookValueAfterDepreciation = log.BookValueAfterDepreciation
                }
            };
        }

        // --- Asset Maintenance ---
        public async Task<APIResponse<List<AssetMaintenanceLogDto>>> GetMaintenanceLogsAsync(int schoolId, int itemId)
        {
            var logs = await _context.AssetMaintenanceLogs
                .Include(m => m.Item)
                .Where(m => m.SchoolRegistrationId == schoolId && m.ItemId == itemId && !m.IsDeleted)
                .OrderByDescending(m => m.MaintenanceDate)
                .ToListAsync();

            var list = logs.Select(m => new AssetMaintenanceLogDto
            {
                Id = m.Id,
                ItemId = m.ItemId,
                ItemName = m.Item.Name,
                MaintenanceDate = m.MaintenanceDate,
                MaintenanceType = m.MaintenanceType,
                Cost = m.Cost,
                ServiceDetails = m.ServiceDetails,
                PerformedBy = m.PerformedBy
            }).ToList();

            return new APIResponse<List<AssetMaintenanceLogDto>> { Success = true, StatusCode = HttpStatusCode.OK, Data = list };
        }

        public async Task<APIResponse<AssetMaintenanceLogDto>> LogMaintenanceAsync(int schoolId, CreateAssetMaintenanceDto dto, string user)
        {
            var log = new AssetMaintenanceLog
            {
                ItemId = dto.ItemId,
                MaintenanceDate = dto.MaintenanceDate,
                MaintenanceType = dto.MaintenanceType,
                Cost = dto.Cost,
                ServiceDetails = dto.ServiceDetails,
                PerformedBy = dto.PerformedBy,
                SchoolRegistrationId = schoolId,
                CreatedBy = user,
                CreatedDate = DateTime.UtcNow
            };

            _context.AssetMaintenanceLogs.Add(log);
            await _context.SaveChangesAsync();

            return new APIResponse<AssetMaintenanceLogDto>
            {
                Success = true,
                StatusCode = HttpStatusCode.Created,
                Message = "Maintenance ticket logged",
                Data = new AssetMaintenanceLogDto
                {
                    Id = log.Id,
                    ItemId = log.ItemId,
                    MaintenanceDate = log.MaintenanceDate,
                    Cost = log.Cost
                }
            };
        }
    }
}
