using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBravo.Infrastructure.Migrations
{
    public partial class Dillerdaller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Repos",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Repos",
                newName: "name");
        }
    }
}
