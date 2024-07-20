using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hng.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrgUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganisationUser_Organisations_OrganisationsId",
                table: "OrganisationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganisationUser_Users_UsersId",
                table: "OrganisationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganisationUser",
                table: "OrganisationUser");

            migrationBuilder.DropIndex(
                name: "IX_OrganisationUser_UsersId",
                table: "OrganisationUser");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "OrganisationUser",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "OrganisationsId",
                table: "OrganisationUser",
                newName: "OrganisationId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "OrganisationUser",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "OrganisationUser",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganisationUser",
                table: "OrganisationUser",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationUser_OrganisationId",
                table: "OrganisationUser",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationUser_UserId_OrganisationId",
                table: "OrganisationUser",
                columns: new[] { "UserId", "OrganisationId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganisationUser_Organisations_OrganisationId",
                table: "OrganisationUser",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganisationUser_Users_UserId",
                table: "OrganisationUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganisationUser_Organisations_OrganisationId",
                table: "OrganisationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganisationUser_Users_UserId",
                table: "OrganisationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganisationUser",
                table: "OrganisationUser");

            migrationBuilder.DropIndex(
                name: "IX_OrganisationUser_OrganisationId",
                table: "OrganisationUser");

            migrationBuilder.DropIndex(
                name: "IX_OrganisationUser_UserId_OrganisationId",
                table: "OrganisationUser");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrganisationUser");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "OrganisationUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "OrganisationUser",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "OrganisationId",
                table: "OrganisationUser",
                newName: "OrganisationsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganisationUser",
                table: "OrganisationUser",
                columns: new[] { "OrganisationsId", "UsersId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationUser_UsersId",
                table: "OrganisationUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganisationUser_Organisations_OrganisationsId",
                table: "OrganisationUser",
                column: "OrganisationsId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganisationUser_Users_UsersId",
                table: "OrganisationUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
