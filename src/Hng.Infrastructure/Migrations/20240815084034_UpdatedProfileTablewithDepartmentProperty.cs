using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hng.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedProfileTablewithDepartmentProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Profiles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Profiles");
        }
    }
}
