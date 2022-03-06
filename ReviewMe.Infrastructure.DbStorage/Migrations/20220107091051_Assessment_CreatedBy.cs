#nullable disable

namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class Assessment_CreatedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Assessments",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "CreatedByEmployeeDoId",
                table: "Assessments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_CreatedByEmployeeDoId",
                table: "Assessments",
                column: "CreatedByEmployeeDoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Employees_CreatedByEmployeeDoId",
                table: "Assessments",
                column: "CreatedByEmployeeDoId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Employees_CreatedByEmployeeDoId",
                table: "Assessments");

            migrationBuilder.DropIndex(
                name: "IX_Assessments_CreatedByEmployeeDoId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "CreatedByEmployeeDoId",
                table: "Assessments");
        }
    }
}
