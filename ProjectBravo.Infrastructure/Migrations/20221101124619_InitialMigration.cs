using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBravo.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "repos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FrequencyOutput = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorOutput = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LatestCommitDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_repos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "repos");
        }
    }
}
