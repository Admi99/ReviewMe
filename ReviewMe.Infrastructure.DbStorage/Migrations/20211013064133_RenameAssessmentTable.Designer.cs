﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReviewMe.Infrastructure.DbStorage;

namespace ReviewMe.Infrastructure.DbStorage.Migrations
{
    [DbContext(typeof(ReviewMeDbContext))]
    [Migration("20211013064133_RenameAssessmentTable")]
    partial class RenameAssessmentTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ReviewMe.Infrastructure.DbStorage.DatabaseObjects.AssessmentDo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("AssessmentDueDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("AssessmentState")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeDoId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("PerformanceReviewDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeDoId");

                    b.ToTable("Assessments");
                });

            modelBuilder.Entity("ReviewMe.Infrastructure.DbStorage.DatabaseObjects.EmployeeDo", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Branch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChangeDescriptionCJP")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContractId")
                        .HasColumnType("int");

                    b.Property<int>("ContractStatus")
                        .HasColumnType("int");

                    b.Property<int>("CostCenter")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateFromC")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateToC")
                        .HasColumnType("datetime2");

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DepartmentIsProduction")
                        .HasColumnType("bit");

                    b.Property<DateTime>("EffectiveDateCJP")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EffectiveDateCSH")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EffectiveDateToCJP")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EffectiveDateToCSH")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EmploymentEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EmploymentStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmploymentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmploymentTypeShortcut")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FKm01_JobPositionParent")
                        .HasColumnType("int");

                    b.Property<string>("FKm12_EmploymentDetailTermination")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("FTE")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("LastCJP")
                        .HasColumnType("int");

                    b.Property<int>("LastCSH")
                        .HasColumnType("int");

                    b.Property<int>("LastContract")
                        .HasColumnType("int");

                    b.Property<string>("LineManager")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LmLogin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LoyalityDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Mail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("MaternityLeave")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("PerformanceReviewMonth")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PpId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ProbationExpireDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReasonForLeave")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RecordStatusCJP")
                        .HasColumnType("int");

                    b.Property<int>("RecordStatusCSH")
                        .HasColumnType("int");

                    b.Property<string>("SurnameFirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TimurId")
                        .HasColumnType("int");

                    b.Property<double>("WeekJobTime")
                        .HasColumnType("float");

                    b.Property<double>("WeekJobTimeFull")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("ReviewMe.Infrastructure.DbStorage.DatabaseObjects.AssessmentDo", b =>
                {
                    b.HasOne("ReviewMe.Infrastructure.DbStorage.DatabaseObjects.EmployeeDo", "EmployeeDo")
                        .WithMany("Assessments")
                        .HasForeignKey("EmployeeDoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EmployeeDo");
                });

            modelBuilder.Entity("ReviewMe.Infrastructure.DbStorage.DatabaseObjects.EmployeeDo", b =>
                {
                    b.Navigation("Assessments");
                });
#pragma warning restore 612, 618
        }
    }
}
