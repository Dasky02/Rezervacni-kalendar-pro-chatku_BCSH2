using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatkaReservation.Migrations
{
    /// <inheritdoc />
    public partial class MakeN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Cottages_CottageID",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Cottages");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Cottages_CottageID",
                table: "Reservations",
                column: "CottageID",
                principalTable: "Cottages",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Cottages_CottageID",
                table: "Reservations");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Cottages",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Cottages_CottageID",
                table: "Reservations",
                column: "CottageID",
                principalTable: "Cottages",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
