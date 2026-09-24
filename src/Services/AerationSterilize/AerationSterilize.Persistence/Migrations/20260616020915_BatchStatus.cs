using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations;

/// <inheritdoc />
public partial class BatchStatus : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Status",
            table: "Batch",
            newName: "BatchStatus");

        migrationBuilder.AddColumn<int>(
            name: "AerationStatus",
            table: "Batch",
            type: "int",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AerationStatus",
            table: "Batch");

        migrationBuilder.RenameColumn(
            name: "BatchStatus",
            table: "Batch",
            newName: "Status");
    }
}
