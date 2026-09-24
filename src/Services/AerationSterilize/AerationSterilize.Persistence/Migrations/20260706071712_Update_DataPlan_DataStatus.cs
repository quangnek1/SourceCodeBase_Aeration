using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations;

/// <inheritdoc />
public partial class Update_DataPlan_DataStatus : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AerationStatus",
            table: "Batch");

        migrationBuilder.DropColumn(
            name: "BatchStatus",
            table: "Batch");

        migrationBuilder.AlterColumn<byte>(
            name: "Status",
            table: "BatchItem",
            type: "tinyint",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AddColumn<byte>(
            name: "Status",
            table: "Batch",
            type: "tinyint",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Status",
            table: "Batch");

        migrationBuilder.AlterColumn<string>(
            name: "Status",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(byte),
            oldType: "tinyint",
            oldNullable: true);

        migrationBuilder.AddColumn<int>(
            name: "AerationStatus",
            table: "Batch",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "BatchStatus",
            table: "Batch",
            type: "int",
            nullable: true);
    }
}
