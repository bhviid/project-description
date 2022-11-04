using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBravo.Infrastructure.Migrations
{
    public partial class AdvancedDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_repos",
                table: "repos");

            migrationBuilder.DropColumn(
                name: "AuthorOutput",
                table: "repos");

            migrationBuilder.RenameTable(
                name: "repos",
                newName: "Repos");

            migrationBuilder.RenameColumn(
                name: "FrequencyOutput",
                table: "Repos",
                newName: "name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Repos",
                table: "Repos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GitRepositoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authors_Repos_GitRepositoryId",
                        column: x => x.GitRepositoryId,
                        principalTable: "Repos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Commits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BelongsToId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commits_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commits_Repos_BelongsToId",
                        column: x => x.BelongsToId,
                        principalTable: "Repos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_GitRepositoryId",
                table: "Authors",
                column: "GitRepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Commits_AuthorId",
                table: "Commits",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Commits_BelongsToId",
                table: "Commits",
                column: "BelongsToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commits");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Repos",
                table: "Repos");

            migrationBuilder.RenameTable(
                name: "Repos",
                newName: "repos");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "repos",
                newName: "FrequencyOutput");

            migrationBuilder.AddColumn<string>(
                name: "AuthorOutput",
                table: "repos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_repos",
                table: "repos",
                column: "Id");
        }
    }
}
