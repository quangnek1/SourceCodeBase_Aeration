using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations;

/// <inheritdoc />
public partial class Update_Box : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "INT",
            table: "Boxes",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "SEQ",
            table: "Boxes",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "INT",
            table: "Boxes");

        migrationBuilder.DropColumn(
            name: "SEQ",
            table: "Boxes");
    }
}
