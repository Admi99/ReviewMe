namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class InitMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PpId = table.Column<int>(type: "int", nullable: false),
                    TimurId = table.Column<int>(type: "int", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SurnameFirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    EmploymentTypeShortcut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmploymentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractStatus = table.Column<int>(type: "int", nullable: false),
                    DateFromC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateToC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastContract = table.Column<int>(type: "int", nullable: false),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostCenter = table.Column<int>(type: "int", nullable: false),
                    DepartmentIsProduction = table.Column<bool>(type: "bit", nullable: false),
                    ChangeDescriptionCJP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordStatusCJP = table.Column<int>(type: "int", nullable: false),
                    EffectiveDateCJP = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveDateToCJP = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastCJP = table.Column<int>(type: "int", nullable: false),
                    FKm01_JobPositionParent = table.Column<int>(type: "int", nullable: false),
                    LmLogin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineManager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeekJobTime = table.Column<double>(type: "float", nullable: false),
                    WeekJobTimeFull = table.Column<double>(type: "float", nullable: false),
                    FTE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecordStatusCSH = table.Column<int>(type: "int", nullable: false),
                    EffectiveDateCSH = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveDateToCSH = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastCSH = table.Column<int>(type: "int", nullable: false),
                    LoyalityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmploymentStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProbationExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmploymentEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FKm12_EmploymentDetailTermination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonForLeave = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaternityLeave = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
