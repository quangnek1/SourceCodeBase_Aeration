using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations;

/// <inheritdoc />
public partial class Batch_Add_Time : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "ActualOutputAeration",
            table: "Batch",
            type: "datetimeoffset",
            nullable: true);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "InputAerationActual",
            table: "Batch",
            type: "datetimeoffset",
            nullable: true);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "PlanOutputAeration",
            table: "Batch",
            type: "datetimeoffset",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ActualOutputAeration",
            table: "Batch");

        migrationBuilder.DropColumn(
            name: "InputAerationActual",
            table: "Batch");

        migrationBuilder.DropColumn(
            name: "PlanOutputAeration",
            table: "Batch");
    }
}
