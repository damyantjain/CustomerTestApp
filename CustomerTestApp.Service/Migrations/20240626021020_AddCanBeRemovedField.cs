using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerTestApp.Service.Migrations
{
    /// <inheritdoc />
    public partial class AddCanBeRemovedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanBeRemoved",
                table: "Customers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanBeRemoved",
                table: "Customers");
        }
    }
}
