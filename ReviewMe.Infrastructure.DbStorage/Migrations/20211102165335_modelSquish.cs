namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    public partial class modelSquish : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeDescriptionCJP",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ContractStatus",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CostCenter",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DateFromC",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DateToC",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DepartmentIsProduction",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EffectiveDateCJP",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EffectiveDateCSH",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EffectiveDateToCJP",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EffectiveDateToCSH",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmploymentEnd",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmploymentStart",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmploymentType",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmploymentTypeShortcut",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "FKm01_JobPositionParent",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "FKm12_EmploymentDetailTermination",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "FTE",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LastCJP",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LastCSH",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LastContract",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LineManager",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LmLogin",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LoyalityDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Mail",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "MaternityLeave",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ProbationExpireDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ReasonForLeave",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RecordStatusCJP",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "WeekJobTime",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "WeekJobTimeFull",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "RecordStatusCSH",
                table: "Employees",
                newName: "IsActive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Employees",
                newName: "RecordStatusCSH");

            migrationBuilder.AddColumn<string>(
                name: "ChangeDescriptionCJP",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContractStatus",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CostCenter",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFromC",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateToC",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "DepartmentIsProduction",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDateCJP",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDateCSH",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDateToCJP",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDateToCSH",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EmploymentEnd",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EmploymentStart",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EmploymentType",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmploymentTypeShortcut",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FKm01_JobPositionParent",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FKm12_EmploymentDetailTermination",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FTE",
                table: "Employees",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "LastCJP",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastCSH",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastContract",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LineManager",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LmLogin",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LoyalityDate",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MaternityLeave",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProbationExpireDate",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ReasonForLeave",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecordStatusCJP",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "WeekJobTime",
                table: "Employees",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WeekJobTimeFull",
                table: "Employees",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
