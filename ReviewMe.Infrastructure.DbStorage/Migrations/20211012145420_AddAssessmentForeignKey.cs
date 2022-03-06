namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class AddAssessmentForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assesments_Employees_EmployeeDoId",
                table: "Assesments");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeDoId",
                table: "Assesments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assesments_Employees_EmployeeDoId",
                table: "Assesments",
                column: "EmployeeDoId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assesments_Employees_EmployeeDoId",
                table: "Assesments");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeDoId",
                table: "Assesments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Assesments_Employees_EmployeeDoId",
                table: "Assesments",
                column: "EmployeeDoId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
