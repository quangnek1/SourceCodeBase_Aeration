using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Modifie_WorktableSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginAt",
                table: "WorkTableSessions");

            migrationBuilder.DropColumn(
                name: "LogoutAt",
                table: "WorkTableSessions");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndedAt",
                table: "WorkTableSessions",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartedAt",
                table: "WorkTableSessions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndedAt",
                table: "WorkTableSessions");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "WorkTableSessions");

            migrationBuilder.AddColumn<DateTime>(
                name: "LoginAt",
                table: "WorkTableSessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LogoutAt",
                table: "WorkTableSessions",
                type: "datetime2",
                nullable: true);
        }
    }
}
