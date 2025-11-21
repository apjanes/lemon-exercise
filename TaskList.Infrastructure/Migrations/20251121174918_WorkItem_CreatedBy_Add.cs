using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WorkItem_CreatedBy_Add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Users_UserId",
                table: "WorkItems");

            migrationBuilder.DropIndex(
                name: "IX_WorkItems_UserId",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "WorkItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkItems_UserId",
                table: "WorkItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Users_UserId",
                table: "WorkItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
