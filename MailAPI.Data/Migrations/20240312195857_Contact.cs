using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MailAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class Contact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "ContactHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ContactHistory_UserID",
                table: "ContactHistory",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactHistory_Users_UserID",
                table: "ContactHistory",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactHistory_Users_UserID",
                table: "ContactHistory");

            migrationBuilder.DropIndex(
                name: "IX_ContactHistory_UserID",
                table: "ContactHistory");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "ContactHistory");
        }
    }
}
