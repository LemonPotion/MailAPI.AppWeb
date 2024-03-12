using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MailAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_MessageHistory_MessageID",
                table: "MessageHistory",
                column: "MessageID");

            migrationBuilder.CreateIndex(
                name: "IX_MessageHistory_UserID",
                table: "MessageHistory",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserID",
                table: "Message",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_MailToken_UserID",
                table: "MailToken",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_MailToken_Users_UserID",
                table: "MailToken",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Users_UserID",
                table: "Message",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageHistory_Message_MessageID",
                table: "MessageHistory",
                column: "MessageID",
                principalTable: "Message",
                principalColumn: "MessageID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageHistory_Users_UserID",
                table: "MessageHistory",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Role_RoleID",
                table: "Users",
                column: "RoleID",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MailToken_Users_UserID",
                table: "MailToken");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Users_UserID",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageHistory_Message_MessageID",
                table: "MessageHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageHistory_Users_UserID",
                table: "MessageHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Role_RoleID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_MessageHistory_MessageID",
                table: "MessageHistory");

            migrationBuilder.DropIndex(
                name: "IX_MessageHistory_UserID",
                table: "MessageHistory");

            migrationBuilder.DropIndex(
                name: "IX_Message_UserID",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_MailToken_UserID",
                table: "MailToken");
        }
    }
}
