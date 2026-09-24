using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitDBAeration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AerationColumn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColumnName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AerationColumn", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Batch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QRCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SterilizeDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    BatchNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataAmiQ411",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductInformation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DrawingNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CatalogCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChamberA = table.Column<int>(type: "int", nullable: true),
                    ChamberB = table.Column<int>(type: "int", nullable: true),
                    ChamberC = table.Column<int>(type: "int", nullable: true),
                    ChamberD = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataAmiQ411", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PONo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaterialTypeC = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PldOrd = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Material = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ItemCodeTypeC = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ItemCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DrawingListNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InternalLot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Phase = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ExternalLot1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Qty = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    QtyInput = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    KeepAeration = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QATest = table.Column<int>(type: "int", nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Label = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Bioburden = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Seal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ster = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ME = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BOXING = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    TestEndotoxin = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TestParticle = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LabelData = table.Column<double>(type: "float", nullable: true),
                    SealData = table.Column<double>(type: "float", nullable: true),
                    BoxingData = table.Column<double>(type: "float", nullable: true),
                    ETD = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeepStorage = table.Column<int>(type: "int", nullable: true),
                    MEChia = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPlan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AerationPosition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    AerationColumnId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AerationPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AerationPosition_AerationColumn_AerationColumnId",
                        column: x => x.AerationColumnId,
                        principalTable: "AerationColumn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BatchItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    DataPlanId = table.Column<int>(type: "int", nullable: false),
                    DataAmiQ411Id = table.Column<int>(type: "int", nullable: true),
                    InputAerationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PlanOutputAerationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ActualOutputAerationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    InputBoxingDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    OutputBoxingDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    InputPackingDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    OutputPackingDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchItem_Batch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BatchItem_DataAmiQ411_DataAmiQ411Id",
                        column: x => x.DataAmiQ411Id,
                        principalTable: "DataAmiQ411",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BatchItem_DataPlan_DataPlanId",
                        column: x => x.DataPlanId,
                        principalTable: "DataPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BatchInAerationPosition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    AerationPositionId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchInAerationPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchInAerationPosition_AerationPosition_AerationPositionId",
                        column: x => x.AerationPositionId,
                        principalTable: "AerationPosition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BatchInAerationPosition_Batch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AerationPosition_AerationColumnId",
                table: "AerationPosition",
                column: "AerationColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchInAerationPosition_AerationPositionId",
                table: "BatchInAerationPosition",
                column: "AerationPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchInAerationPosition_BatchId_AerationPositionId",
                table: "BatchInAerationPosition",
                columns: new[] { "BatchId", "AerationPositionId" });

            migrationBuilder.CreateIndex(
                name: "IX_BatchItem_BatchId",
                table: "BatchItem",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchItem_DataAmiQ411Id",
                table: "BatchItem",
                column: "DataAmiQ411Id");

            migrationBuilder.CreateIndex(
                name: "IX_BatchItem_DataPlanId",
                table: "BatchItem",
                column: "DataPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchInAerationPosition");

            migrationBuilder.DropTable(
                name: "BatchItem");

            migrationBuilder.DropTable(
                name: "AerationPosition");

            migrationBuilder.DropTable(
                name: "Batch");

            migrationBuilder.DropTable(
                name: "DataAmiQ411");

            migrationBuilder.DropTable(
                name: "DataPlan");

            migrationBuilder.DropTable(
                name: "AerationColumn");
        }
    }
}
