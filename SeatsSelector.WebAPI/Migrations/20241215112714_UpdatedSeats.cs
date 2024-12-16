using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeatsSelector.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Col",
                table: "Seats",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Row",
                table: "Seats",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Col",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "Row",
                table: "Seats");
        }
    }
}
