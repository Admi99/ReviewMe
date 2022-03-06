namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class RemoveTimurId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimurId",
                table: "AssessmentReviewers",
                newName: "EmployeeDoId");

            migrationBuilder.AddColumn<int>(
                name: "AssessmentDoId",
                table: "Assessments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentReviewers_Employees_EmployeeDoId",
                table: "AssessmentReviewers",
                column: "EmployeeDoId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentReviewers_Employees_EmployeeDoId",
                table: "AssessmentReviewers");

            migrationBuilder.DropColumn(
                name: "AssessmentDoId",
                table: "Assessments");

            migrationBuilder.RenameColumn(
                name: "EmployeeDoId",
                table: "AssessmentReviewers",
                newName: "TimurId");
        }
    }
}
