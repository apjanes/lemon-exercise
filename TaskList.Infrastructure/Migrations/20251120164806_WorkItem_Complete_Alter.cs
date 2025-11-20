using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WorkItem_Complete_Alter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "WorkItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "WorkItems",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "WorkItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "WorkItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
