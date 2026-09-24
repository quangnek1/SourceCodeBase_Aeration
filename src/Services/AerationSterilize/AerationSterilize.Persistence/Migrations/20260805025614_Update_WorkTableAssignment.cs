using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_WorkTableAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoxingJobs_BatchItem_BatchItemId",
                table: "BoxingJobs");

            migrationBuilder.AlterColumn<int>(
                name: "BoxingJobId",
                table: "WorkTableAssignments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PackingPositionId",
                table: "WorkTableAssignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "BatchItemId",
                table: "BoxingJobs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ActualQty",
                table: "BoxingJobs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_BoxingJobs_BatchItem_BatchItemId",
                table: "BoxingJobs",
                column: "BatchItemId",
                principalTable: "BatchItem",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoxingJobs_BatchItem_BatchItemId",
                table: "BoxingJobs");

            migrationBuilder.DropColumn(
                name: "PackingPositionId",
                table: "WorkTableAssignments");

            migrationBuilder.AlterColumn<int>(
                name: "BoxingJobId",
                table: "WorkTableAssignments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BatchItemId",
                table: "BoxingJobs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ActualQty",
                table: "BoxingJobs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BoxingJobs_BatchItem_BatchItemId",
                table: "BoxingJobs",
                column: "BatchItemId",
                principalTable: "BatchItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
