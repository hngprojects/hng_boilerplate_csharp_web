using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hng.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewsLetterSubscriberModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JoinedOn",
                table: "NewsLetterSubscribers",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "NewsLetterSubscribers",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsLetterSubscribers_Email",
                table: "NewsLetterSubscribers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NewsLetterSubscribers_Email",
                table: "NewsLetterSubscribers");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "NewsLetterSubscribers",
                newName: "JoinedOn");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "NewsLetterSubscribers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);
        }
    }
}
