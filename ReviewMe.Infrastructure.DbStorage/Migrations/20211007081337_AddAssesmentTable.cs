namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class AddAssesmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assesments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentDueDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PerformanceReviewDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AssesmentState = table.Column<int>(type: "int", nullable: false),
                    EmployeeDoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assesments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assesments_Employees_EmployeeDoId",
                        column: x => x.EmployeeDoId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assesments_EmployeeDoId",
                table: "Assesments",
                column: "EmployeeDoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assesments");
        }
    }
}
