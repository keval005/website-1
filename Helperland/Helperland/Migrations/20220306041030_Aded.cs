using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Helperland.Migrations
{
    public partial class Aded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserTypeId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Zipcode",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<double>(type: "float", nullable: false),
                    PostalCode = table.Column<int>(type: "int", maxLength: 7, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequestAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServicesRequestId = table.Column<int>(type: "int", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zipcode = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<double>(type: "float", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequestAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequestExtras",
                columns: table => new
                {
                    ServiceRequestExtraId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServicesRequestId = table.Column<int>(type: "int", nullable: false),
                    ServicesExtraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequestExtras", x => x.ServiceRequestExtraId);
                });

            migrationBuilder.CreateTable(
                name: "ServicesRequests",
                columns: table => new
                {
                    ServiceRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    ServiceStartDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zipcode = table.Column<int>(type: "int", nullable: false),
                    ServiceFrequency = table.Column<int>(type: "int", nullable: false),
                    ServiceHourlyrate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServicesHours = table.Column<float>(type: "real", maxLength: 5, nullable: false),
                    ExtraHours = table.Column<float>(type: "real", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", maxLength: 5, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", maxLength: 5, nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", maxLength: 5, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaymentTransactionrefNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentDue = table.Column<int>(type: "int", nullable: false),
                    JobStatus = table.Column<bool>(type: "bit", nullable: false),
                    ServicesProviderId = table.Column<int>(type: "int", nullable: false),
                    SPAcceptedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasPets = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CraetedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: false),
                    RefundedAmount = table.Column<decimal>(type: "decimal(18,2)", maxLength: 5, nullable: false),
                    Haslssue = table.Column<int>(type: "int", nullable: false),
                    PaymentDone = table.Column<bool>(type: "bit", nullable: false),
                    uniqueidentifier = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicesRequests", x => x.ServiceRequestId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "ServiceRequestAddress");

            migrationBuilder.DropTable(
                name: "ServiceRequestExtras");

            migrationBuilder.DropTable(
                name: "ServicesRequests");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Zipcode",
                table: "AspNetUsers");
        }
    }
}
