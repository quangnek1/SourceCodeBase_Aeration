using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_BoxingSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoxingSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxingPositionId = table.Column<int>(type: "int", nullable: false),
                    BoxingJobId = table.Column<int>(type: "int", nullable: true),
                    CurrentBoxId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxingSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoxingSessions_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoxingSessions_Boxes_CurrentBoxId",
                        column: x => x.CurrentBoxId,
                        principalTable: "Boxes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BoxingSessions_BoxingJobs_BoxingJobId",
                        column: x => x.BoxingJobId,
                        principalTable: "BoxingJobs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BoxingSessions_BoxingPositions_BoxingPositionId",
                        column: x => x.BoxingPositionId,
                        principalTable: "BoxingPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoxingSessions_AppUserId",
                table: "BoxingSessions",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxingSessions_BoxingJobId",
                table: "BoxingSessions",
                column: "BoxingJobId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxingSessions_BoxingPositionId",
                table: "BoxingSessions",
                column: "BoxingPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxingSessions_CurrentBoxId",
                table: "BoxingSessions",
                column: "CurrentBoxId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoxingSessions");
        }
    }
}
