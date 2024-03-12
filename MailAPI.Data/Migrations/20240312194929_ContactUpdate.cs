using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MailAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class ContactUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactHistory_Users_ContactUSerID",
                table: "ContactHistory");

            migrationBuilder.DropIndex(
                name: "IX_ContactHistory_ContactUSerID",
                table: "ContactHistory");

            migrationBuilder.DropColumn(
                name: "ContactUSerID",
                table: "ContactHistory");

            migrationBuilder.AddColumn<string>(
                name: "ContactMail",
                table: "ContactHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ContactHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactMail",
                table: "ContactHistory");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ContactHistory");

            migrationBuilder.AddColumn<int>(
                name: "ContactUSerID",
                table: "ContactHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ContactHistory_ContactUSerID",
                table: "ContactHistory",
                column: "ContactUSerID");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactHistory_Users_ContactUSerID",
                table: "ContactHistory",
                column: "ContactUSerID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
