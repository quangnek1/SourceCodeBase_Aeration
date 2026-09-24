using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations;

/// <inheritdoc />
public partial class UpdateBatchItem : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "A",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "B",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "Biobudent",
            table: "BatchItem",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "C",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "D",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "DeliveryDatePlan",
            table: "BatchItem",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Destination",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "DrawingListNo",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "Endotoxin",
            table: "BatchItem",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "ExternalLot1",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Family",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "GaugeNoSealing",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Group",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "InternalLot",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ItemName",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "KeepAeration",
            table: "BatchItem",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "KeepSample",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Material",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "NumberRank",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "Particle",
            table: "BatchItem",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "QtyFeaturesTest",
            table: "BatchItem",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "QtyInput",
            table: "BatchItem",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "QtyOutputSealing",
            table: "BatchItem",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "QtyOutputSterilize",
            table: "BatchItem",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "Status",
            table: "BatchItem",
            type: "nvarchar(max)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "A",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "B",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "Biobudent",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "C",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "D",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "DeliveryDatePlan",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "Destination",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "DrawingListNo",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "Endotoxin",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "ExternalLot1",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "Family",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "GaugeNoSealing",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "Group",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "InternalLot",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "ItemName",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "KeepAeration",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "KeepSample",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "Material",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "NumberRank",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "Particle",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "QtyFeaturesTest",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "QtyInput",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "QtyOutputSealing",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "QtyOutputSterilize",
            table: "BatchItem");

        migrationBuilder.DropColumn(
            name: "Status",
            table: "BatchItem");
    }
}
