namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class AddAssessmentForeignKeyToAssessmentReviewerDo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentReviewerDo_Assessments_AssessmentDoId",
                table: "AssessmentReviewerDo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssessmentReviewerDo",
                table: "AssessmentReviewerDo");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AssessmentReviewerDo");

            migrationBuilder.RenameTable(
                name: "AssessmentReviewerDo",
                newName: "AssessmentReviewers");

            migrationBuilder.RenameIndex(
                name: "IX_AssessmentReviewerDo_AssessmentDoId",
                table: "AssessmentReviewers",
                newName: "IX_AssessmentReviewers_AssessmentDoId");

            migrationBuilder.AlterColumn<int>(
                name: "AssessmentDoId",
                table: "AssessmentReviewers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssessmentReviewers",
                table: "AssessmentReviewers",
                columns: new[] { "TimurId", "ProjectId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentReviewers_Assessments_AssessmentDoId",
                table: "AssessmentReviewers",
                column: "AssessmentDoId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentReviewers_Assessments_AssessmentDoId",
                table: "AssessmentReviewers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssessmentReviewers",
                table: "AssessmentReviewers");

            migrationBuilder.RenameTable(
                name: "AssessmentReviewers",
                newName: "AssessmentReviewerDo");

            migrationBuilder.RenameIndex(
                name: "IX_AssessmentReviewers_AssessmentDoId",
                table: "AssessmentReviewerDo",
                newName: "IX_AssessmentReviewerDo_AssessmentDoId");

            migrationBuilder.AlterColumn<int>(
                name: "AssessmentDoId",
                table: "AssessmentReviewerDo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AssessmentReviewerDo",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssessmentReviewerDo",
                table: "AssessmentReviewerDo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentReviewerDo_Assessments_AssessmentDoId",
                table: "AssessmentReviewerDo",
                column: "AssessmentDoId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
