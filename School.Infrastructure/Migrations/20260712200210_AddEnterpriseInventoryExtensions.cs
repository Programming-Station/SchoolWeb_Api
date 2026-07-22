using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEnterpriseInventoryExtensions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseReturns",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GrnId = table.Column<int>(type: "int", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreditNoteNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseReturns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseReturns_GoodsReceiptNotes_GrnId",
                        column: x => x.GrnId,
                        principalSchema: "Inventory",
                        principalTable: "GoodsReceiptNotes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseReturns_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QualityInspections",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrnId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    QuantityInspected = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    QuantityAccepted = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    QuantityRejected = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    InspectionReport = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    QualityScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    InspectedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityInspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityInspections_GoodsReceiptNotes_GrnId",
                        column: x => x.GrnId,
                        principalSchema: "Inventory",
                        principalTable: "GoodsReceiptNotes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QualityInspections_InventoryItems_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "InventoryItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QualityInspections_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RequestForQuotations",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RfqNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequisitionId = table.Column<int>(type: "int", nullable: false),
                    RfqDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestForQuotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestForQuotations_PurchaseRequisitions_RequisitionId",
                        column: x => x.RequisitionId,
                        principalSchema: "Inventory",
                        principalTable: "PurchaseRequisitions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestForQuotations_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockIssues",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    IssuedToType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssuedToId = table.Column<int>(type: "int", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Returnable = table.Column<bool>(type: "bit", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockIssues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockIssues_InventoryItems_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "InventoryItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockIssues_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    StoreType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Capacity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warehouses_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VendorQuotations",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RfqId = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    QuoteDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorQuotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorQuotations_RequestForQuotations_RfqId",
                        column: x => x.RfqId,
                        principalSchema: "Inventory",
                        principalTable: "RequestForQuotations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VendorQuotations_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VendorQuotations_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalSchema: "Inventory",
                        principalTable: "Vendors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WarehouseBins",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    Zone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Rack = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Shelf = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BinCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SchoolRegistrationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseBins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseBins_SchoolRegistrations_SchoolRegistrationId",
                        column: x => x.SchoolRegistrationId,
                        principalTable: "SchoolRegistrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WarehouseBins_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "Inventory",
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturns_GrnId",
                schema: "Inventory",
                table: "PurchaseReturns",
                column: "GrnId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturns_SchoolRegistrationId",
                schema: "Inventory",
                table: "PurchaseReturns",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityInspections_GrnId",
                schema: "Inventory",
                table: "QualityInspections",
                column: "GrnId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityInspections_ItemId",
                schema: "Inventory",
                table: "QualityInspections",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityInspections_SchoolRegistrationId",
                schema: "Inventory",
                table: "QualityInspections",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForQuotations_RequisitionId",
                schema: "Inventory",
                table: "RequestForQuotations",
                column: "RequisitionId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForQuotations_SchoolRegistrationId",
                schema: "Inventory",
                table: "RequestForQuotations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockIssues_ItemId",
                schema: "Inventory",
                table: "StockIssues",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockIssues_SchoolRegistrationId",
                schema: "Inventory",
                table: "StockIssues",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_SchoolRegistrationId",
                schema: "Inventory",
                table: "Stores",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorQuotations_RfqId",
                schema: "Inventory",
                table: "VendorQuotations",
                column: "RfqId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorQuotations_SchoolRegistrationId",
                schema: "Inventory",
                table: "VendorQuotations",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorQuotations_VendorId",
                schema: "Inventory",
                table: "VendorQuotations",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseBins_SchoolRegistrationId",
                schema: "Inventory",
                table: "WarehouseBins",
                column: "SchoolRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseBins_WarehouseId",
                schema: "Inventory",
                table: "WarehouseBins",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_SchoolRegistrationId",
                schema: "Inventory",
                table: "Warehouses",
                column: "SchoolRegistrationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseReturns",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "QualityInspections",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "StockIssues",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "Stores",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "VendorQuotations",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "WarehouseBins",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "RequestForQuotations",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "Warehouses",
                schema: "Inventory");
        }
    }
}
