#nullable disable

namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class AddTeamLeaderPropertyToEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeamLeaderLogin",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamLeaderLogin",
                table: "Employees");
        }
    }
}
