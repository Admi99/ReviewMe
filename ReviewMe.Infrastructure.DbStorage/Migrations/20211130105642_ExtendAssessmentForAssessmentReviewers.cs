namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class ExtendAssessmentForAssessmentReviewers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssessmentReviewerDo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimurId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    AssessmentDoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentReviewerDo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentReviewerDo_Assessments_AssessmentDoId",
                        column: x => x.AssessmentDoId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentReviewerDo_AssessmentDoId",
                table: "AssessmentReviewerDo",
                column: "AssessmentDoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentReviewerDo");
        }
    }
}
