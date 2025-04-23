using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFielToSubtaskTablet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "Subtasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "Subtasks");
        }
    }
}
