using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_BoxingPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchItemInPackingPosition");

            migrationBuilder.AddColumn<int>(
                name: "BoxingPositionId",
                table: "Boxes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BoxingColumns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColumnName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxingColumns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BoxingPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    BoxingColumnId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxingPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoxingPositions_BoxingColumns_BoxingColumnId",
                        column: x => x.BoxingColumnId,
                        principalTable: "BoxingColumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_BoxingPositionId",
                table: "Boxes",
                column: "BoxingPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxingPositions_BoxingColumnId",
                table: "BoxingPositions",
                column: "BoxingColumnId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_BoxingPositions_BoxingPositionId",
                table: "Boxes",
                column: "BoxingPositionId",
                principalTable: "BoxingPositions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_BoxingPositions_BoxingPositionId",
                table: "Boxes");

            migrationBuilder.DropTable(
                name: "BoxingPositions");

            migrationBuilder.DropTable(
                name: "BoxingColumns");

            migrationBuilder.DropIndex(
                name: "IX_Boxes_BoxingPositionId",
                table: "Boxes");

            migrationBuilder.DropColumn(
                name: "BoxingPositionId",
                table: "Boxes");

            migrationBuilder.CreateTable(
                name: "BatchItemInPackingPosition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchItemId = table.Column<int>(type: "int", nullable: false),
                    PackingPositionId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchItemInPackingPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchItemInPackingPosition_BatchItem_BatchItemId",
                        column: x => x.BatchItemId,
                        principalTable: "BatchItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BatchItemInPackingPosition_PackingPosition_PackingPositionId",
                        column: x => x.PackingPositionId,
                        principalTable: "PackingPosition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchItemInPackingPosition_BatchItemId",
                table: "BatchItemInPackingPosition",
                column: "BatchItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchItemInPackingPosition_PackingPositionId",
                table: "BatchItemInPackingPosition",
                column: "PackingPositionId");
        }
    }
}
