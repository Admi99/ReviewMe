namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class NewPropertyFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "AssessmentReviewers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "AssessmentReviewers");
        }
    }
}
