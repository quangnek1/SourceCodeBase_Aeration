using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_BatchItemIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BatchInAerationPosition_PackingPosition_PackingPositionId",
                table: "BatchInAerationPosition");

            migrationBuilder.DropIndex(
                name: "IX_BatchInAerationPosition_PackingPositionId",
                table: "BatchInAerationPosition");

            migrationBuilder.DropColumn(
                name: "PackingPositionId",
                table: "BatchInAerationPosition");

            migrationBuilder.AddColumn<int>(
                name: "PackingPositionId",
                table: "BatchItem",
                type: "int",
                nullable: true);

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
                name: "IX_BatchItem_PackingPositionId",
                table: "BatchItem",
                column: "PackingPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchItemInPackingPosition_BatchItemId",
                table: "BatchItemInPackingPosition",
                column: "BatchItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchItemInPackingPosition_PackingPositionId",
                table: "BatchItemInPackingPosition",
                column: "PackingPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchItem_PackingPosition_PackingPositionId",
                table: "BatchItem",
                column: "PackingPositionId",
                principalTable: "PackingPosition",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BatchItem_PackingPosition_PackingPositionId",
                table: "BatchItem");

            migrationBuilder.DropTable(
                name: "BatchItemInPackingPosition");

            migrationBuilder.DropIndex(
                name: "IX_BatchItem_PackingPositionId",
                table: "BatchItem");

            migrationBuilder.DropColumn(
                name: "PackingPositionId",
                table: "BatchItem");

            migrationBuilder.AddColumn<int>(
                name: "PackingPositionId",
                table: "BatchInAerationPosition",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BatchInAerationPosition_PackingPositionId",
                table: "BatchInAerationPosition",
                column: "PackingPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchInAerationPosition_PackingPosition_PackingPositionId",
                table: "BatchInAerationPosition",
                column: "PackingPositionId",
                principalTable: "PackingPosition",
                principalColumn: "Id");
        }
    }
}
