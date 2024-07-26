﻿// <auto-generated />
using System;
using Data_Access_Layer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data_Access_Layer.Migrations
{
    [DbContext(typeof(ExpenseSharingDbContext))]
    [Migration("20240630154511_add")]
    partial class add
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Data_Access_Layer.Models.Expense", b =>
                {
                    b.Property<int>("ExpenseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExpenseId"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("PaidById")
                        .HasColumnType("int");

                    b.HasKey("ExpenseId");

                    b.HasIndex("GroupId");

                    b.HasIndex("PaidById");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Data_Access_Layer.Models.ExpenseSettlement", b =>
                {
                    b.Property<int>("ExpenseSettlementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExpenseSettlementId"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("ExpenseId")
                        .HasColumnType("int");

                    b.Property<int>("PaidById")
                        .HasColumnType("int");

                    b.Property<int>("ReceivedById")
                        .HasColumnType("int");

                    b.HasKey("ExpenseSettlementId");

                    b.HasIndex("ExpenseId");

                    b.HasIndex("PaidById");

                    b.HasIndex("ReceivedById");

                    b.ToTable("ExpenseSettlements");
                });

            modelBuilder.Entity("Data_Access_Layer.Models.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupId"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GroupId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Data_Access_Layer.Models.GroupMember", b =>
                {
                    b.Property<int>("GroupMemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupMemberId"), 1L, 1);

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<bool>("IsSettled")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("GroupMemberId");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupMembers");
                });

            modelBuilder.Entity("Data_Access_Layer.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Data_Access_Layer.Models.Expense", b =>
                {
                    b.HasOne("Data_Access_Layer.Models.Group", "Group")
                        .WithMany("Expenses")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data_Access_Layer.Models.User", "PaidBy")
                        .WithMany()
                        .HasForeignKey("PaidById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("PaidBy");
                });

            modelBuilder.Entity("Data_Access_Layer.Models.ExpenseSettlement", b =>
                {
                    b.HasOne("Data_Access_Layer.Models.Expense", "Expense")
                        .WithMany("ExpenseSettlements")
                        .HasForeignKey("ExpenseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data_Access_Layer.Models.User", "PaidBy")
                        .WithMany("PaidSettlements")
                        .HasForeignKey("PaidById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Data_Access_Layer.Models.User", "ReceivedBy")
                        .WithMany("ReceivedSettlements")
                        .HasForeignKey("ReceivedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Expense");

                    b.Navigation("PaidBy");

                    b.Navigation("ReceivedBy");
                });

            modelBuilder.Entity("Data_Access_Layer.Models.GroupMember", b =>
                {
                    b.HasOne("Data_Access_Layer.Models.Group", "Group")
                        .WithMany("Members")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data_Access_Layer.Models.User", "User")
                        .WithMany("GroupMembers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Data_Access_Layer.Models.Expense", b =>
                {
                    b.Navigation("ExpenseSettlements");
                });

            modelBuilder.Entity("Data_Access_Layer.Models.Group", b =>
                {
                    b.Navigation("Expenses");

                    b.Navigation("Members");
                });

            modelBuilder.Entity("Data_Access_Layer.Models.User", b =>
                {
                    b.Navigation("GroupMembers");

                    b.Navigation("PaidSettlements");

                    b.Navigation("ReceivedSettlements");
                });
#pragma warning restore 612, 618
        }
    }
}
