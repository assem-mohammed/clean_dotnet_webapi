using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LegacyVendorCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Group = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    Name2 = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    Name3 = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    Name4 = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    FirstSearchTerm = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SecondSearchTerm = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Language = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    FaxNumber = table.Column<string>(type: "nvarchar(31)", maxLength: 31, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(241)", maxLength: 241, nullable: false),
                    VatRegNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IndustryKey = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    CentralBlock = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SSOUserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRemoved = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("VendorCode", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vendors");
        }
    }
}
