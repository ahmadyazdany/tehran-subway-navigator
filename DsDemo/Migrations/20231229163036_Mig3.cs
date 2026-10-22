using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DsDemo.Migrations
{
    /// <inheritdoc />
    public partial class Mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ways_From_To",
                table: "Ways",
                columns: new[] { "From", "To" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ways_From_To",
                table: "Ways");
        }
    }
}
