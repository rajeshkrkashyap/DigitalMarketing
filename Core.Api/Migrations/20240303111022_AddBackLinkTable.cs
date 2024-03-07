using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddBackLinkTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnalysisStatus",
                table: "Crawleds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotlaPages",
                table: "Crawleds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BackLinks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AnchorText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkDestination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoFollowOrNoFollow = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkJuice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContextualRelevance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkDiversity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkVelocity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NaturalOrArtificial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnalysisStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackLinks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "CCAFEA64-3A85-49AA-BA4B-7D2E296226FA",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "marketing", "MARKETING" });

            migrationBuilder.CreateIndex(
                name: "IX_BackLinks_ProjectId",
                table: "BackLinks",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackLinks");

            migrationBuilder.DropColumn(
                name: "AnalysisStatus",
                table: "Crawleds");

            migrationBuilder.DropColumn(
                name: "TotlaPages",
                table: "Crawleds");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "CCAFEA64-3A85-49AA-BA4B-7D2E296226FA",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "patient", "PATIENT" });
        }
    }
}
