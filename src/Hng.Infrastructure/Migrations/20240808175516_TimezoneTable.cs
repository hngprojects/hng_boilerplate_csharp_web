using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hng.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TimezoneTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TimezoneId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Timezones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TimezoneValue = table.Column<string>(type: "text", nullable: true),
                    GmtOffset = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timezones", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TimezoneId",
                table: "Users",
                column: "TimezoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Timezones_TimezoneId",
                table: "Users",
                column: "TimezoneId",
                principalTable: "Timezones",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Timezones_TimezoneId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Timezones");

            migrationBuilder.DropIndex(
                name: "IX_Users_TimezoneId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TimezoneId",
                table: "Users");
        }
    }
}
