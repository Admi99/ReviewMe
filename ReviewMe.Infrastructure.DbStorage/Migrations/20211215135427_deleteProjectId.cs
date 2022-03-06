namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class deleteProjectId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AssessmentReviewers",
                table: "AssessmentReviewers");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "AssessmentReviewers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssessmentReviewers",
                table: "AssessmentReviewers",
                columns: new[] { "EmployeeDoId", "AssessmentDoId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AssessmentReviewers",
                table: "AssessmentReviewers");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "AssessmentReviewers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssessmentReviewers",
                table: "AssessmentReviewers",
                columns: new[] { "EmployeeDoId", "ProjectId", "AssessmentDoId" });
        }
    }
}
