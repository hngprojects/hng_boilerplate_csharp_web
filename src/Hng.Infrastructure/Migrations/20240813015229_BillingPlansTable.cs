using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hng.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BillingPlansTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BillingPlanId",
                table: "Subscriptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BillingPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Frequency = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingPlans", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_BillingPlanId",
                table: "Subscriptions",
                column: "BillingPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_BillingPlans_BillingPlanId",
                table: "Subscriptions",
                column: "BillingPlanId",
                principalTable: "BillingPlans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_BillingPlans_BillingPlanId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "BillingPlans");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_BillingPlanId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "BillingPlanId",
                table: "Subscriptions");
        }
    }
}
