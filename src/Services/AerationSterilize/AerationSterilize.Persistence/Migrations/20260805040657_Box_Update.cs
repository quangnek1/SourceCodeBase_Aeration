using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Box_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_BoxingJobs_BoxingJobId",
                table: "Boxes");

            migrationBuilder.AlterColumn<int>(
                name: "Capacity",
                table: "Boxes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BoxingJobId",
                table: "Boxes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_BoxingJobs_BoxingJobId",
                table: "Boxes",
                column: "BoxingJobId",
                principalTable: "BoxingJobs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_BoxingJobs_BoxingJobId",
                table: "Boxes");

            migrationBuilder.AlterColumn<int>(
                name: "Capacity",
                table: "Boxes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BoxingJobId",
                table: "Boxes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_BoxingJobs_BoxingJobId",
                table: "Boxes",
                column: "BoxingJobId",
                principalTable: "BoxingJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
