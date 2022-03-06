#nullable disable

namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class AreasForImprovements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AreasForImprovements",
                table: "AssessmentReviewers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreasForImprovements",
                table: "AssessmentReviewers");
        }
    }
}
