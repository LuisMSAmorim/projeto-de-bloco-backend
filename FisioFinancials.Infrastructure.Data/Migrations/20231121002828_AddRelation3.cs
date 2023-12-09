using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FisioFinancials.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelation3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Receiveds",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Receiveds_UserId",
                table: "Receiveds",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receiveds_AspNetUsers_UserId",
                table: "Receiveds",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receiveds_AspNetUsers_UserId",
                table: "Receiveds");

            migrationBuilder.DropIndex(
                name: "IX_Receiveds_UserId",
                table: "Receiveds");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Receiveds");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
