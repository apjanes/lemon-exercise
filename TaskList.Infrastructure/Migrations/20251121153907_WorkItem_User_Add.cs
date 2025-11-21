using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WorkItem_User_Add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "WorkItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "WorkItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkItems_CreatedById",
                table: "WorkItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItems_UserId",
                table: "WorkItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Users_CreatedById",
                table: "WorkItems",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Users_UserId",
                table: "WorkItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Users_CreatedById",
                table: "WorkItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Users_UserId",
                table: "WorkItems");

            migrationBuilder.DropIndex(
                name: "IX_WorkItems_CreatedById",
                table: "WorkItems");

            migrationBuilder.DropIndex(
                name: "IX_WorkItems_UserId",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkItems");
        }
    }
}
