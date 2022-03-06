namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class AddAssessmentReviewerState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssessmentDoId",
                table: "Assessments");

            migrationBuilder.AddColumn<int>(
                name: "AssessmentReviewerState",
                table: "AssessmentReviewers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssessmentReviewerState",
                table: "AssessmentReviewers");

            migrationBuilder.AddColumn<int>(
                name: "AssessmentDoId",
                table: "Assessments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
