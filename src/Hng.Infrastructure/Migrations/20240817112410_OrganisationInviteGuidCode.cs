using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hng.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrganisationInviteGuidCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InviteLink",
                table: "OrganizationInvites");

            migrationBuilder.AddColumn<Guid>(
                name: "InviteCode",
                table: "OrganizationInvites",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationInvites_InviteCode",
                table: "OrganizationInvites",
                column: "InviteCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrganizationInvites_InviteCode",
                table: "OrganizationInvites");

            migrationBuilder.DropColumn(
                name: "InviteCode",
                table: "OrganizationInvites");

            migrationBuilder.AddColumn<string>(
                name: "InviteLink",
                table: "OrganizationInvites",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
