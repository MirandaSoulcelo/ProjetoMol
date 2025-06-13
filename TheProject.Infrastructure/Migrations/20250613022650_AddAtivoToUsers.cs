using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAtivoToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Users");
        }
    }
}
