using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorsHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonalEmailProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonalEmail",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalEmail",
                table: "Doctors");
        }
    }
}
