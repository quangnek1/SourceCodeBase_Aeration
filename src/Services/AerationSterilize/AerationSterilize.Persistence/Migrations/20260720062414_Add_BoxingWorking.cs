using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_BoxingWorking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoxingJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalQty = table.Column<int>(type: "int", nullable: false),
                    ActualQty = table.Column<int>(type: "int", nullable: false),
                    PackingPositionId = table.Column<int>(type: "int", nullable: false),
                    BatchItemId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxingJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoxingJobs_BatchItem_BatchItemId",
                        column: x => x.BatchItemId,
                        principalTable: "BatchItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoxingJobs_PackingPosition_PackingPositionId",
                        column: x => x.PackingPositionId,
                        principalTable: "PackingPosition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Boxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    BoxingJobId = table.Column<int>(type: "int", nullable: false),
                    BatchItemId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boxes_BatchItem_BatchItemId",
                        column: x => x.BatchItemId,
                        principalTable: "BatchItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Boxes_BoxingJobs_BoxingJobId",
                        column: x => x.BoxingJobId,
                        principalTable: "BoxingJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    WorkLineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTables_WorkLines_WorkLineId",
                        column: x => x.WorkLineId,
                        principalTable: "WorkLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemcD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    INT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Ext1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ext2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SEQNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    BoxId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTags_Boxes_BoxId",
                        column: x => x.BoxId,
                        principalTable: "Boxes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkTableSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    WorkTableId = table.Column<int>(type: "int", nullable: false),
                    LoginAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogoutAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AppUserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTableSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTableSessions_AppUsers_AppUserId1",
                        column: x => x.AppUserId1,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkTableSessions_WorkTables_WorkTableId",
                        column: x => x.WorkTableId,
                        principalTable: "WorkTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_BatchItemId",
                table: "Boxes",
                column: "BatchItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_BoxingJobId",
                table: "Boxes",
                column: "BoxingJobId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxingJobs_BatchItemId",
                table: "BoxingJobs",
                column: "BatchItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxingJobs_PackingPositionId",
                table: "BoxingJobs",
                column: "PackingPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTags_BoxId",
                table: "ItemTags",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTables_WorkLineId",
                table: "WorkTables",
                column: "WorkLineId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTableSessions_AppUserId1",
                table: "WorkTableSessions",
                column: "AppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTableSessions_WorkTableId",
                table: "WorkTableSessions",
                column: "WorkTableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemTags");

            migrationBuilder.DropTable(
                name: "WorkTableSessions");

            migrationBuilder.DropTable(
                name: "Boxes");

            migrationBuilder.DropTable(
                name: "WorkTables");

            migrationBuilder.DropTable(
                name: "BoxingJobs");

            migrationBuilder.DropTable(
                name: "WorkLines");
        }
    }
}
