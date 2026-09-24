using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations;

/// <inheritdoc />
public partial class Update_Section_UserId : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_WorkTableSessions_AppUsers_AppUserId1",
            table: "WorkTableSessions");

        migrationBuilder.DropIndex(
            name: "IX_WorkTableSessions_AppUserId1",
            table: "WorkTableSessions");

        migrationBuilder.DropColumn(
            name: "AppUserId1",
            table: "WorkTableSessions");

        migrationBuilder.AlterColumn<Guid>(
            name: "AppUserId",
            table: "WorkTableSessions",
            type: "uniqueidentifier",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.CreateIndex(
            name: "IX_WorkTableSessions_AppUserId",
            table: "WorkTableSessions",
            column: "AppUserId");

        migrationBuilder.AddForeignKey(
            name: "FK_WorkTableSessions_AppUsers_AppUserId",
            table: "WorkTableSessions",
            column: "AppUserId",
            principalTable: "AppUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_WorkTableSessions_AppUsers_AppUserId",
            table: "WorkTableSessions");

        migrationBuilder.DropIndex(
            name: "IX_WorkTableSessions_AppUserId",
            table: "WorkTableSessions");

        migrationBuilder.AlterColumn<string>(
            name: "AppUserId",
            table: "WorkTableSessions",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AddColumn<Guid>(
            name: "AppUserId1",
            table: "WorkTableSessions",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateIndex(
            name: "IX_WorkTableSessions_AppUserId1",
            table: "WorkTableSessions",
            column: "AppUserId1");

        migrationBuilder.AddForeignKey(
            name: "FK_WorkTableSessions_AppUsers_AppUserId1",
            table: "WorkTableSessions",
            column: "AppUserId1",
            principalTable: "AppUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
