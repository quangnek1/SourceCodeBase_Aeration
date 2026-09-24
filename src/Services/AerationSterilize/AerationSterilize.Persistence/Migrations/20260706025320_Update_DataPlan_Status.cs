using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations;

/// <inheritdoc />
public partial class Update_DataPlan_Status : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<byte>(
            name: "Status",
            table: "DataPlan",
            type: "tinyint",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<bool>(
            name: "Status",
            table: "DataPlan",
            type: "bit",
            nullable: true,
            oldClrType: typeof(byte),
            oldType: "tinyint",
            oldNullable: true);
    }
}
