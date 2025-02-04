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
    [Migration("20240709034938_add")]
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
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("PaidById")
                        .HasColumnType("int");

                    b.HasKey("ExpenseId");

                    b.HasIndex("GroupId");

                    b.HasIndex("PaidById");

                    b.ToTable("Expenses");
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
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

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

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            AccessFailedCount = 0,
                            Balance = 0m,
                            ConcurrencyStamp = "89bf7f1b-c1c1-4379-9c2a-12aed86ea8e1",
                            Email = "admin@gmail.com",
                            EmailConfirmed = false,
                            Id = "b6c2b9fb-098d-41ea-bebd-10e2df31f276",
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@GMAIL.COM",
                            Password = "string",
                            PhoneNumberConfirmed = false,
                            Role = "admin",
                            SecurityStamp = "",
                            TwoFactorEnabled = false
                        },
                        new
                        {
                            UserId = 2,
                            AccessFailedCount = 0,
                            Balance = 0m,
                            ConcurrencyStamp = "c4fa2220-28f3-4bd8-9957-e34e4ad1467e",
                            Email = "rahul1@gmail.com",
                            EmailConfirmed = false,
                            Id = "1c602df1-dfef-41bd-87c1-5d37b1daccb4",
                            LockoutEnabled = false,
                            NormalizedEmail = "RAHUL1@GMAIL.COM",
                            Password = "string",
                            PhoneNumberConfirmed = false,
                            Role = "normal",
                            SecurityStamp = "",
                            TwoFactorEnabled = false
                        },
                        new
                        {
                            UserId = 3,
                            AccessFailedCount = 0,
                            Balance = 0m,
                            ConcurrencyStamp = "507b6902-ee74-4588-aa43-6b047dd7fef4",
                            Email = "rahul2@gmail.com",
                            EmailConfirmed = false,
                            Id = "6ef761dd-ec07-4bd4-b859-7b00bf84ff77",
                            LockoutEnabled = false,
                            NormalizedEmail = "RAHUL2@GMAIL.COM",
                            Password = "string",
                            PhoneNumberConfirmed = false,
                            Role = "normal",
                            SecurityStamp = "",
                            TwoFactorEnabled = false
                        },
                        new
                        {
                            UserId = 4,
                            AccessFailedCount = 0,
                            Balance = 0m,
                            ConcurrencyStamp = "8111bbcc-044b-4749-beaa-74d19d501f3b",
                            Email = "rahul3@gmail.com",
                            EmailConfirmed = false,
                            Id = "864ea69c-3a69-4401-9770-e18ed03e76e9",
                            LockoutEnabled = false,
                            NormalizedEmail = "RAHUL3@GMAIL.COM",
                            Password = "string",
                            PhoneNumberConfirmed = false,
                            Role = "normal",
                            SecurityStamp = "",
                            TwoFactorEnabled = false
                        },
                        new
                        {
                            UserId = 5,
                            AccessFailedCount = 0,
                            Balance = 0m,
                            ConcurrencyStamp = "03b5cb09-0b9d-4563-bc11-de6d7254ba68",
                            Email = "rahul4@gmail.com",
                            EmailConfirmed = false,
                            Id = "7c04bfef-3adc-45dd-9af0-43be4d5193ad",
                            LockoutEnabled = false,
                            NormalizedEmail = "RAHUL4@GMAIL.COM",
                            Password = "string",
                            PhoneNumberConfirmed = false,
                            Role = "normal",
                            SecurityStamp = "",
                            TwoFactorEnabled = false
                        },
                        new
                        {
                            UserId = 6,
                            AccessFailedCount = 0,
                            Balance = 0m,
                            ConcurrencyStamp = "4f19a8ab-169e-4f71-85a3-68d49d17cb41",
                            Email = "rahul5@gmail.com",
                            EmailConfirmed = false,
                            Id = "42d044a3-766f-4bf9-bde4-033e7fb4840b",
                            LockoutEnabled = false,
                            NormalizedEmail = "RAHUL5@GMAIL.COM",
                            Password = "string",
                            PhoneNumberConfirmed = false,
                            Role = "normal",
                            SecurityStamp = "",
                            TwoFactorEnabled = false
                        },
                        new
                        {
                            UserId = 7,
                            AccessFailedCount = 0,
                            Balance = 0m,
                            ConcurrencyStamp = "a108f602-ae4a-47c3-857c-b3d5fb483853",
                            Email = "rahul6@gmail.com",
                            EmailConfirmed = false,
                            Id = "d5dcfb5d-3cd3-4a4b-94ca-f64c76d57b1e",
                            LockoutEnabled = false,
                            NormalizedEmail = "RAHUL6@GMAIL.COM",
                            Password = "string",
                            PhoneNumberConfirmed = false,
                            Role = "normal",
                            SecurityStamp = "",
                            TwoFactorEnabled = false
                        },
                        new
                        {
                            UserId = 8,
                            AccessFailedCount = 0,
                            Balance = 0m,
                            ConcurrencyStamp = "4e947479-ba05-4a8a-aea2-d8a8aed5d917",
                            Email = "rahul7@gmail.com",
                            EmailConfirmed = false,
                            Id = "bd5dbc16-e40e-4f49-a745-eae03bf9793c",
                            LockoutEnabled = false,
                            NormalizedEmail = "RAHUL7@GMAIL.COM",
                            Password = "string",
                            PhoneNumberConfirmed = false,
                            Role = "normal",
                            SecurityStamp = "",
                            TwoFactorEnabled = false
                        },
                        new
                        {
                            UserId = 9,
                            AccessFailedCount = 0,
                            Balance = 0m,
                            ConcurrencyStamp = "bd825bd6-d5cb-49e0-89bd-27ecbe85659f",
                            Email = "rahul8@gmail.com",
                            EmailConfirmed = false,
                            Id = "62ff1a78-3e7b-41bc-af93-a9ccd73b2ecf",
                            LockoutEnabled = false,
                            NormalizedEmail = "RAHUL8@GMAIL.COM",
                            Password = "string",
                            PhoneNumberConfirmed = false,
                            Role = "normal",
                            SecurityStamp = "",
                            TwoFactorEnabled = false
                        });
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

            modelBuilder.Entity("Data_Access_Layer.Models.Group", b =>
                {
                    b.Navigation("Expenses");

                    b.Navigation("Members");
                });

            modelBuilder.Entity("Data_Access_Layer.Models.User", b =>
                {
                    b.Navigation("GroupMembers");
                });
#pragma warning restore 612, 618
        }
    }
}
