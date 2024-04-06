using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionSystem.Persistence.Migrations
{
    public partial class initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InvoiceID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoomID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceStatus = table.Column<int>(type: "int", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INITIATOR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INITIATOR_EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DATE_INITIATED = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AUTHORIZER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AUTHORIZER_EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DATE_AUTHORIZED = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AUTHORIZERS_COMMENT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AUTH_STATUS = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INITIATOR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INITIATOR_EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DATE_INITIATED = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AUTHORIZER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AUTHORIZER_EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DATE_AUTHORIZED = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AUTHORIZERS_COMMENT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AUTH_STATUS = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Payment_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "USER",
                keyColumn: "Id",
                keyValue: "7cc5cd62-6240-44e5-b44f-bff0ae73342",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "8d1772c9-b972-4276-aaff-27cdb771a667", "3ecf86bf-7755-49e9-95c5-ceb1781cfbf9" });

            migrationBuilder.UpdateData(
                table: "USER",
                keyColumn: "Id",
                keyValue: "9a6a928b-0e11-4d5d-8a29-b8f04445a29",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "08bbae41-ceb3-42eb-91d0-65c26b55730b", "aced3d9c-36f9-4a4c-92dc-c66fd61c4e64" });

            migrationBuilder.UpdateData(
                table: "USER",
                keyColumn: "Id",
                keyValue: "9a6a928b-0e11-4d5d-8a29-b8f04445e72",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "fe8eadc4-7b0a-43c0-8757-a4d3ce154be4", "5606c5a7-df85-49fc-b588-d6ef64dd8d2e" });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_InvoiceId",
                table: "Payment",
                column: "InvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.UpdateData(
                table: "USER",
                keyColumn: "Id",
                keyValue: "7cc5cd62-6240-44e5-b44f-bff0ae73342",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3527ec28-ea44-4b87-af55-36923d7bb2b7", "c53ae4ac-603e-409f-b677-edb2af7aad11" });

            migrationBuilder.UpdateData(
                table: "USER",
                keyColumn: "Id",
                keyValue: "9a6a928b-0e11-4d5d-8a29-b8f04445a29",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a0971e22-3bfa-4186-b23a-ee570f89d4a4", "198a151c-8b3b-4a82-ac6d-87ebdb0e8c67" });

            migrationBuilder.UpdateData(
                table: "USER",
                keyColumn: "Id",
                keyValue: "9a6a928b-0e11-4d5d-8a29-b8f04445e72",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "22355caf-265b-4d97-b832-edeea1fadcb4", "4a24444b-34ab-4bdb-b648-beefdfed7cdd" });
        }
    }
}
