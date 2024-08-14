using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hng.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class faqUpdatedClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "category",
                table: "FAQ",
                newName: "Category");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FAQ",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "FAQ",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "FAQ",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FAQ");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "FAQ");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "FAQ");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "FAQ",
                newName: "category");
        }
    }
}
