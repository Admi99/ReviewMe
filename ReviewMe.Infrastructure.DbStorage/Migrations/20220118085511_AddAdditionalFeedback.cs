#nullable disable

namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class AddAdditionalFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalFeedback",
                table: "Assessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalFeedback",
                table: "Assessments");
        }
    }
}
