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
    [Migration("20211206121651_AddAssessmentDoIdAsPrimaryKey")]
    partial class AddAssessmentDoIdAsPrimaryKey
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

            modelBuilder.Entity("ReviewMe.Infrastructure.DbStorage.DatabaseObjects.AssessmentReviewerDo", b =>
                {
                    b.Property<int>("TimurId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("AssessmentDoId")
                        .HasColumnType("int");

                    b.HasKey("TimurId", "ProjectId", "AssessmentDoId");

                    b.HasIndex("AssessmentDoId");

                    b.ToTable("AssessmentReviewers");
                });

            modelBuilder.Entity("ReviewMe.Infrastructure.DbStorage.DatabaseObjects.EmployeeDo", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("PerformanceReviewMonth")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PpId")
                        .HasColumnType("int");

                    b.Property<string>("SurnameFirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TimurId")
                        .HasColumnType("int");

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

            modelBuilder.Entity("ReviewMe.Infrastructure.DbStorage.DatabaseObjects.AssessmentReviewerDo", b =>
                {
                    b.HasOne("ReviewMe.Infrastructure.DbStorage.DatabaseObjects.AssessmentDo", "AssessmentDo")
                        .WithMany("AssessmentReviewers")
                        .HasForeignKey("AssessmentDoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssessmentDo");
                });

            modelBuilder.Entity("ReviewMe.Infrastructure.DbStorage.DatabaseObjects.AssessmentDo", b =>
                {
                    b.Navigation("AssessmentReviewers");
                });

            modelBuilder.Entity("ReviewMe.Infrastructure.DbStorage.DatabaseObjects.EmployeeDo", b =>
                {
                    b.Navigation("Assessments");
                });
#pragma warning restore 612, 618
        }
    }
}
