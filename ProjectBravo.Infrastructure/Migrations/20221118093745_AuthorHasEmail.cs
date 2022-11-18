using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBravo.Infrastructure.Migrations
{
    public partial class AuthorHasEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commits_Repos_BelongsToId",
                table: "Commits");

            migrationBuilder.DropIndex(
                name: "IX_Commits_BelongsToId",
                table: "Commits");

            migrationBuilder.RenameColumn(
                name: "BelongsToId",
                table: "Commits",
                newName: "RepositoryId");

            migrationBuilder.AddColumn<int>(
                name: "GitRepositoryId",
                table: "Commits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Commits_GitRepositoryId",
                table: "Commits",
                column: "GitRepositoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commits_Repos_GitRepositoryId",
                table: "Commits",
                column: "GitRepositoryId",
                principalTable: "Repos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commits_Repos_GitRepositoryId",
                table: "Commits");

            migrationBuilder.DropIndex(
                name: "IX_Commits_GitRepositoryId",
                table: "Commits");

            migrationBuilder.DropColumn(
                name: "GitRepositoryId",
                table: "Commits");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "RepositoryId",
                table: "Commits",
                newName: "BelongsToId");

            migrationBuilder.CreateIndex(
                name: "IX_Commits_BelongsToId",
                table: "Commits",
                column: "BelongsToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commits_Repos_BelongsToId",
                table: "Commits",
                column: "BelongsToId",
                principalTable: "Repos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
