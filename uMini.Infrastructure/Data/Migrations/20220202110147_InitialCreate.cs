using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uMini.Infrastructure.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "shorturl");

            migrationBuilder.CreateSequence(
                name: "shorturlseq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "ShortUrls",
                schema: "shorturl",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortUrls", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_CreatorId",
                schema: "shorturl",
                table: "ShortUrls",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_Key",
                schema: "shorturl",
                table: "ShortUrls",
                column: "Key",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortUrls",
                schema: "shorturl");

            migrationBuilder.DropSequence(
                name: "shorturlseq");
        }
    }
}
