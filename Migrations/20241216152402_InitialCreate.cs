using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatkaReservation.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CottageId",
                table: "Cottages",
                newName: "CottageID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CottageID",
                table: "Cottages",
                newName: "CottageId");
        }
    }
}
