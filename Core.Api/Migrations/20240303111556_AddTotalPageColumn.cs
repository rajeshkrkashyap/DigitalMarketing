using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalPageColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotlaPages",
                table: "Crawleds");

            migrationBuilder.AddColumn<int>(
                name: "TotlaPages",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotlaPages",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "TotlaPages",
                table: "Crawleds",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
