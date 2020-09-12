using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ShapeServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shape",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShapeType = table.Column<int>(nullable: false),
                    Identifier = table.Column<int>(nullable: false),
                    Parameters = table.Column<List<decimal>>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    TreePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shape", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shape_Shape",
                        column: x => x.ParentId,
                        principalTable: "Shape",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShapeEJRecord",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShapeId = table.Column<int>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CurrentArea = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShapeEJRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShapeEJRecord_Shape",
                        column: x => x.ShapeId,
                        principalTable: "Shape",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shape_Identifier",
                table: "Shape",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shape_ParentId",
                table: "Shape",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Shape_TreePath",
                table: "Shape",
                column: "TreePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShapeEJRecord_ShapeId",
                table: "ShapeEJRecord",
                column: "ShapeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShapeEJRecord");

            migrationBuilder.DropTable(
                name: "Shape");
        }
    }
}
