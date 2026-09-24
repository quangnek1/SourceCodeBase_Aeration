using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_packingLayout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PackingPositionId",
                table: "BatchInAerationPosition",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PackingColumn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColumnName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingColumn", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanCAG = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PlanPTCA = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DataAmiQ411 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PackingPosition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    PackingColumnId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackingPosition_PackingColumn_PackingColumnId",
                        column: x => x.PackingColumnId,
                        principalTable: "PackingColumn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchInAerationPosition_PackingPositionId",
                table: "BatchInAerationPosition",
                column: "PackingPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_PackingPosition_PackingColumnId",
                table: "PackingPosition",
                column: "PackingColumnId");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchInAerationPosition_PackingPosition_PackingPositionId",
                table: "BatchInAerationPosition",
                column: "PackingPositionId",
                principalTable: "PackingPosition",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BatchInAerationPosition_PackingPosition_PackingPositionId",
                table: "BatchInAerationPosition");

            migrationBuilder.DropTable(
                name: "PackingPosition");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "PackingColumn");

            migrationBuilder.DropIndex(
                name: "IX_BatchInAerationPosition_PackingPositionId",
                table: "BatchInAerationPosition");

            migrationBuilder.DropColumn(
                name: "PackingPositionId",
                table: "BatchInAerationPosition");
        }
    }
}
