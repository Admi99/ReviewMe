namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class AddAssessmentDoIdAsPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AssessmentReviewers",
                table: "AssessmentReviewers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssessmentReviewers",
                table: "AssessmentReviewers",
                columns: new[] { "TimurId", "ProjectId", "AssessmentDoId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AssessmentReviewers",
                table: "AssessmentReviewers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssessmentReviewers",
                table: "AssessmentReviewers",
                columns: new[] { "TimurId", "ProjectId" });
        }
    }
}
