using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Api.Migrations
{
    /// <inheritdoc />
    public partial class SEOScoreTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasValidSSL",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlacklisted",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMalicious",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WhoIsDomain",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArticleTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Intent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserProvidedKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleTypes_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SmallDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Auther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaProperty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blogs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContentQualities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CrawledId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PageTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRelevanceToTheTopic = table.Column<bool>(type: "bit", nullable: false),
                    IsAccuracyAndCredibility = table.Column<bool>(type: "bit", nullable: false),
                    IsClarityAndReadability = table.Column<bool>(type: "bit", nullable: false),
                    IsDepthAndBreadthOfCoverage = table.Column<bool>(type: "bit", nullable: false),
                    UniquenessAndOriginality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngagementAndInteractivity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserExperience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReputationAndAuthority = table.Column<bool>(type: "bit", nullable: false),
                    IsPurposeAndIntent = table.Column<bool>(type: "bit", nullable: false),
                    IsFeedbackAndMetrics = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentQualities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentQualities_Crawleds_CrawledId",
                        column: x => x.CrawledId,
                        principalTable: "Crawleds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ImagesAndMultimedias",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CrawledId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AltText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SizeAndFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaSitemap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageContext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponsiveDesign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MultimediaURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesAndMultimedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagesAndMultimedias_Crawleds_CrawledId",
                        column: x => x.CrawledId,
                        principalTable: "Crawleds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InternalLinkings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CrawledId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LinkText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInternal = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalLinkings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternalLinkings_Crawleds_CrawledId",
                        column: x => x.CrawledId,
                        principalTable: "Crawleds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KeywordUsages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CrawledId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Keyword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeywordsInContent = table.Column<int>(type: "int", nullable: false),
                    KeywordsInMetaDescription = table.Column<int>(type: "int", nullable: false),
                    KeywordsInTitle = table.Column<int>(type: "int", nullable: false),
                    KeywordsInHeading = table.Column<int>(type: "int", nullable: false),
                    IsMainKeyword = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeywordUsages_Crawleds_CrawledId",
                        column: x => x.CrawledId,
                        principalTable: "Crawleds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MetaTags",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CrawledId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Viewport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Charset = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Robots = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Canonical = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OgTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OgDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OgImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OgType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetaTags_Crawleds_CrawledId",
                        column: x => x.CrawledId,
                        principalTable: "Crawleds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MobileFriendliness",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CrawledId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Viewport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextReadabilityAndFontSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TouchFriendlyElements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptimizedImagesAndMedia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvoidanceOfFlashAndPopUps = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileFriendlyNavigation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsistentContentAndFunctionality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileFriendliness", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MobileFriendliness_Crawleds_CrawledId",
                        column: x => x.CrawledId,
                        principalTable: "Crawleds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PageLoadingSpeeds",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CrawledId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageLoadTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageLoadingSpeeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageLoadingSpeeds_Crawleds_CrawledId",
                        column: x => x.CrawledId,
                        principalTable: "Crawleds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Securities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CrawledId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsWebSiteHTTPSSecure = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Securities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Securities_Crawleds_CrawledId",
                        column: x => x.CrawledId,
                        principalTable: "Crawleds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SocialSignals",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CrawledId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AnalyzeSocialSignals = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialSignals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialSignals_Crawleds_CrawledId",
                        column: x => x.CrawledId,
                        principalTable: "Crawleds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TechnicalSEOs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CrawledId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalSEOs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalSEOs_Crawleds_CrawledId",
                        column: x => x.CrawledId,
                        principalTable: "Crawleds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "URLStructures",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CrawledId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCleanURL = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_URLStructures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_URLStructures_Crawleds_CrawledId",
                        column: x => x.CrawledId,
                        principalTable: "Crawleds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ArticleTitles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArticleTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTitles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleTitles_ArticleTypes_ArticleTypeId",
                        column: x => x.ArticleTypeId,
                        principalTable: "ArticleTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTitles_ArticleTypeId",
                table: "ArticleTitles",
                column: "ArticleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTypes_ProjectId",
                table: "ArticleTypes",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_ProjectId",
                table: "Blogs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentQualities_CrawledId",
                table: "ContentQualities",
                column: "CrawledId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagesAndMultimedias_CrawledId",
                table: "ImagesAndMultimedias",
                column: "CrawledId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalLinkings_CrawledId",
                table: "InternalLinkings",
                column: "CrawledId");

            migrationBuilder.CreateIndex(
                name: "IX_KeywordUsages_CrawledId",
                table: "KeywordUsages",
                column: "CrawledId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaTags_CrawledId",
                table: "MetaTags",
                column: "CrawledId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileFriendliness_CrawledId",
                table: "MobileFriendliness",
                column: "CrawledId");

            migrationBuilder.CreateIndex(
                name: "IX_PageLoadingSpeeds_CrawledId",
                table: "PageLoadingSpeeds",
                column: "CrawledId");

            migrationBuilder.CreateIndex(
                name: "IX_Securities_CrawledId",
                table: "Securities",
                column: "CrawledId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialSignals_CrawledId",
                table: "SocialSignals",
                column: "CrawledId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSEOs_CrawledId",
                table: "TechnicalSEOs",
                column: "CrawledId");

            migrationBuilder.CreateIndex(
                name: "IX_URLStructures_CrawledId",
                table: "URLStructures",
                column: "CrawledId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTitles");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "ContentQualities");

            migrationBuilder.DropTable(
                name: "ImagesAndMultimedias");

            migrationBuilder.DropTable(
                name: "InternalLinkings");

            migrationBuilder.DropTable(
                name: "KeywordUsages");

            migrationBuilder.DropTable(
                name: "MetaTags");

            migrationBuilder.DropTable(
                name: "MobileFriendliness");

            migrationBuilder.DropTable(
                name: "PageLoadingSpeeds");

            migrationBuilder.DropTable(
                name: "Securities");

            migrationBuilder.DropTable(
                name: "SocialSignals");

            migrationBuilder.DropTable(
                name: "TechnicalSEOs");

            migrationBuilder.DropTable(
                name: "URLStructures");

            migrationBuilder.DropTable(
                name: "ArticleTypes");

            migrationBuilder.DropColumn(
                name: "HasValidSSL",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsBlacklisted",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsMalicious",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "WhoIsDomain",
                table: "Projects");
        }
    }
}
