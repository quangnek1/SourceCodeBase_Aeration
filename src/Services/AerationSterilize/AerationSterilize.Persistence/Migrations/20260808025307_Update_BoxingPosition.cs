using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AerationSterilize.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_BoxingPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatchItemId",
                table: "BoxingJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TargetQty",
                table: "BoxingJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BoxingJobId",
                table: "Boxes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoxingJobs_BatchItemId",
                table: "BoxingJobs",
                column: "BatchItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_BoxingJobId",
                table: "Boxes",
                column: "BoxingJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_BoxingJobs_BoxingJobId",
                table: "Boxes",
                column: "BoxingJobId",
                principalTable: "BoxingJobs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BoxingJobs_BatchItem_BatchItemId",
                table: "BoxingJobs",
                column: "BatchItemId",
                principalTable: "BatchItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_BoxingJobs_BoxingJobId",
                table: "Boxes");

            migrationBuilder.DropForeignKey(
                name: "FK_BoxingJobs_BatchItem_BatchItemId",
                table: "BoxingJobs");

            migrationBuilder.DropIndex(
                name: "IX_BoxingJobs_BatchItemId",
                table: "BoxingJobs");

            migrationBuilder.DropIndex(
                name: "IX_Boxes_BoxingJobId",
                table: "Boxes");

            migrationBuilder.DropColumn(
                name: "BatchItemId",
                table: "BoxingJobs");

            migrationBuilder.DropColumn(
                name: "TargetQty",
                table: "BoxingJobs");

            migrationBuilder.DropColumn(
                name: "BoxingJobId",
                table: "Boxes");
        }
    }
}
