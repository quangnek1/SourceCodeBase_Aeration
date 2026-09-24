using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Delete_BoxingJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_BoxingJobs_BoxingJobId",
                table: "Boxes");

            migrationBuilder.DropTable(
                name: "BoxingJobs");

            migrationBuilder.DropColumn(
                name: "BoxingJobId",
                table: "WorkTableAssignments");

            migrationBuilder.RenameColumn(
                name: "BoxingJobId",
                table: "Boxes",
                newName: "PackingPositionId");

            migrationBuilder.RenameIndex(
                name: "IX_Boxes_BoxingJobId",
                table: "Boxes",
                newName: "IX_Boxes_PackingPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTableAssignments_PackingPositionId",
                table: "WorkTableAssignments",
                column: "PackingPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTableAssignments_WorkTableId",
                table: "WorkTableAssignments",
                column: "WorkTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_PackingPosition_PackingPositionId",
                table: "Boxes",
                column: "PackingPositionId",
                principalTable: "PackingPosition",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTableAssignments_PackingPosition_PackingPositionId",
                table: "WorkTableAssignments",
                column: "PackingPositionId",
                principalTable: "PackingPosition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTableAssignments_WorkTables_WorkTableId",
                table: "WorkTableAssignments",
                column: "WorkTableId",
                principalTable: "WorkTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_PackingPosition_PackingPositionId",
                table: "Boxes");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkTableAssignments_PackingPosition_PackingPositionId",
                table: "WorkTableAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkTableAssignments_WorkTables_WorkTableId",
                table: "WorkTableAssignments");

            migrationBuilder.DropIndex(
                name: "IX_WorkTableAssignments_PackingPositionId",
                table: "WorkTableAssignments");

            migrationBuilder.DropIndex(
                name: "IX_WorkTableAssignments_WorkTableId",
                table: "WorkTableAssignments");

            migrationBuilder.RenameColumn(
                name: "PackingPositionId",
                table: "Boxes",
                newName: "BoxingJobId");

            migrationBuilder.RenameIndex(
                name: "IX_Boxes_PackingPositionId",
                table: "Boxes",
                newName: "IX_Boxes_BoxingJobId");

            migrationBuilder.AddColumn<int>(
                name: "BoxingJobId",
                table: "WorkTableAssignments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BoxingJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchItemId = table.Column<int>(type: "int", nullable: true),
                    PackingPositionId = table.Column<int>(type: "int", nullable: false),
                    ActualQty = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    JobNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxingJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoxingJobs_BatchItem_BatchItemId",
                        column: x => x.BatchItemId,
                        principalTable: "BatchItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BoxingJobs_PackingPosition_PackingPositionId",
                        column: x => x.PackingPositionId,
                        principalTable: "PackingPosition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoxingJobs_BatchItemId",
                table: "BoxingJobs",
                column: "BatchItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxingJobs_PackingPositionId",
                table: "BoxingJobs",
                column: "PackingPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_BoxingJobs_BoxingJobId",
                table: "Boxes",
                column: "BoxingJobId",
                principalTable: "BoxingJobs",
                principalColumn: "Id");
        }
    }
}
