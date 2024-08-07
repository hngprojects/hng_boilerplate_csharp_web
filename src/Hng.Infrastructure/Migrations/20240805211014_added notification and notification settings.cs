using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hng.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addednotificationandnotificationsettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityWorkspaceEmail",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ActivityWorkspaceSlack",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "AnnouncementsUpdateEmails",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "AnnouncementsUpdateSlack",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "EmailDigests",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "EmailNotifications",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "MobilePushNotifications",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "SlackNotifications",
                table: "Notifications",
                newName: "IsRead");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Notifications",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NotificationSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MobilePushNotifications = table.Column<bool>(type: "boolean", nullable: false),
                    ActivityWorkspaceEmail = table.Column<bool>(type: "boolean", nullable: false),
                    EmailNotifications = table.Column<bool>(type: "boolean", nullable: false),
                    EmailDigests = table.Column<bool>(type: "boolean", nullable: false),
                    AnnouncementsUpdateEmails = table.Column<bool>(type: "boolean", nullable: false),
                    ActivityWorkspaceSlack = table.Column<bool>(type: "boolean", nullable: false),
                    SlackNotifications = table.Column<bool>(type: "boolean", nullable: false),
                    AnnouncementsUpdateSlack = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSettings_UserId",
                table: "NotificationSettings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "IsRead",
                table: "Notifications",
                newName: "SlackNotifications");

            migrationBuilder.AddColumn<bool>(
                name: "ActivityWorkspaceEmail",
                table: "Notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ActivityWorkspaceSlack",
                table: "Notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AnnouncementsUpdateEmails",
                table: "Notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AnnouncementsUpdateSlack",
                table: "Notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EmailDigests",
                table: "Notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EmailNotifications",
                table: "Notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MobilePushNotifications",
                table: "Notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
