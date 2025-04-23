using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameFieldOnSubtaskTablet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subtasks_Tasks_TaskModelId",
                table: "Subtasks");

            migrationBuilder.RenameColumn(
                name: "TaskModelId",
                table: "Subtasks",
                newName: "TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Subtasks_TaskModelId",
                table: "Subtasks",
                newName: "IX_Subtasks_TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subtasks_Tasks_TaskId",
                table: "Subtasks",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subtasks_Tasks_TaskId",
                table: "Subtasks");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "Subtasks",
                newName: "TaskModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Subtasks_TaskId",
                table: "Subtasks",
                newName: "IX_Subtasks_TaskModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subtasks_Tasks_TaskModelId",
                table: "Subtasks",
                column: "TaskModelId",
                principalTable: "Tasks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
