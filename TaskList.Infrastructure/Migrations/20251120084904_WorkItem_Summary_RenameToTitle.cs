using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WorkItem_Summary_RenameToTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "WorkItems",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "WorkItems",
                newName: "Summary");
        }
    }
}
