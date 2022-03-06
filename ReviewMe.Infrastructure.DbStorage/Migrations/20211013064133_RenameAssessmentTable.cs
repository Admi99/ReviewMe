namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class RenameAssessmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assesments");

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentDueDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PerformanceReviewDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AssessmentState = table.Column<int>(type: "int", nullable: false),
                    EmployeeDoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessments_Employees_EmployeeDoId",
                        column: x => x.EmployeeDoId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_EmployeeDoId",
                table: "Assessments",
                column: "EmployeeDoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.CreateTable(
                name: "Assesments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssesmentState = table.Column<int>(type: "int", nullable: false),
                    AssessmentDueDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EmployeeDoId = table.Column<int>(type: "int", nullable: false),
                    PerformanceReviewDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assesments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assesments_Employees_EmployeeDoId",
                        column: x => x.EmployeeDoId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assesments_EmployeeDoId",
                table: "Assesments",
                column: "EmployeeDoId");
        }
    }
}
