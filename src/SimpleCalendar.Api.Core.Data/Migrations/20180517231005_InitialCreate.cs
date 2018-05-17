using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleCalendar.Api.Core.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    DataJson = table.Column<string>(nullable: true),
                    DataJsonVersion = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regions_Regions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    RegionId = table.Column<string>(nullable: true),
                    IsPublic = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    DataJson = table.Column<string>(nullable: true),
                    DataJsonVersion = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegionRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    RegionId = table.Column<string>(nullable: false),
                    Role = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionRoles", x => x.Id);
                    table.UniqueConstraint("AK_RegionRoles_RegionId_UserId", x => new { x.RegionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_RegionRoles_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "DataJson", "DataJsonVersion", "ParentId" },
                values: new object[] { "ROOT", null, null, 0, null });

            migrationBuilder.InsertData(
                table: "RegionRoles",
                columns: new[] { "Id", "RegionId", "Role", "UserId" },
                values: new object[] { "ROOT_ADMIN", "ROOT", 3, "google-oauth2|103074202427969604113" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_RegionId",
                table: "Events",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ParentId",
                table: "Regions",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "RegionRoles");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
