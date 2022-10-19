﻿// <auto-generated />
using System;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DAL.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("DAL.Models.Cell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CellStateId")
                        .HasColumnType("int")
                        .HasColumnName("cellStateId");

                    b.Property<int>("X")
                        .HasColumnType("int")
                        .HasColumnName("X");

                    b.Property<int>("Y")
                        .HasColumnType("int")
                        .HasColumnName("Y");

                    b.HasKey("Id");

                    b.HasIndex("CellStateId");

                    b.ToTable("Cell");
                });

            modelBuilder.Entity("DAL.Models.CellState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CellStateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("cellStateName");

                    b.HasKey("Id");

                    b.ToTable("CellState");
                });

            modelBuilder.Entity("DAL.Models.Direction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("DirectionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("directionName");

                    b.HasKey("Id");

                    b.ToTable("Direction");
                });

            modelBuilder.Entity("DAL.Models.Field", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("PlayerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("appUserId");

                    b.Property<int>("Size")
                        .HasColumnType("int")
                        .HasColumnName("size");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Field");
                });

            modelBuilder.Entity("DAL.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("GameStateId")
                        .HasColumnType("int")
                        .HasColumnName("gameStateId");

                    b.HasKey("Id");

                    b.HasIndex("GameStateId");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("DAL.Models.GameField", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("FirstFieldId")
                        .HasColumnType("int")
                        .HasColumnName("firstFieldId");

                    b.Property<int>("GameId")
                        .HasColumnType("int")
                        .HasColumnName("gameId");

                    b.Property<int?>("SecondFieldId")
                        .HasColumnType("int")
                        .HasColumnName("secondFieldId");

                    b.HasKey("Id");

                    b.HasIndex("FirstFieldId");

                    b.HasIndex("GameId");

                    b.HasIndex("SecondFieldId");

                    b.ToTable("GameField");
                });

            modelBuilder.Entity("DAL.Models.GameState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("GameStateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("gameStateName");

                    b.HasKey("Id");

                    b.ToTable("GameState");
                });

            modelBuilder.Entity("DAL.Models.PlayerGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FirstPlayerId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("firstPlayerId");

                    b.Property<int>("GameId")
                        .HasColumnType("int")
                        .HasColumnName("gameId");

                    b.Property<string>("SecondPlayerId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("secondPlayerId");

                    b.HasKey("Id");

                    b.HasIndex("FirstPlayerId");

                    b.HasIndex("GameId");

                    b.HasIndex("SecondPlayerId");

                    b.ToTable("PlayerGame");
                });

            modelBuilder.Entity("DAL.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CellId")
                        .HasColumnType("int")
                        .HasColumnName("cellId");

                    b.Property<int>("ShipWrapperId")
                        .HasColumnType("int")
                        .HasColumnName("shipWrapperId");

                    b.HasKey("Id");

                    b.HasIndex("CellId");

                    b.HasIndex("ShipWrapperId");

                    b.ToTable("Position");
                });

            modelBuilder.Entity("DAL.Models.Ship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DirectionId")
                        .HasColumnType("int")
                        .HasColumnName("directionId");

                    b.Property<int>("ShipSizeId")
                        .HasColumnType("int")
                        .HasColumnName("shipSizeId");

                    b.Property<int>("ShipStateId")
                        .HasColumnType("int")
                        .HasColumnName("shipStateId");

                    b.HasKey("Id");

                    b.HasIndex("DirectionId");

                    b.HasIndex("ShipSizeId");

                    b.HasIndex("ShipStateId");

                    b.ToTable("Ship");
                });

            modelBuilder.Entity("DAL.Models.ShipSize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ShipSizeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("shipSizeName");

                    b.HasKey("Id");

                    b.ToTable("ShipSize");
                });

            modelBuilder.Entity("DAL.Models.ShipState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ShipStateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("shipStateName");

                    b.HasKey("Id");

                    b.ToTable("ShipState");
                });

            modelBuilder.Entity("DAL.Models.ShipWrapper", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("FieldId")
                        .HasColumnType("int")
                        .HasColumnName("fieldId");

                    b.Property<int>("ShipId")
                        .HasColumnType("int")
                        .HasColumnName("shipId");

                    b.HasKey("Id");

                    b.HasIndex("FieldId")
                        .IsUnique();

                    b.HasIndex("ShipId")
                        .IsUnique();

                    b.ToTable("ShipWrapper");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DAL.Models.Cell", b =>
                {
                    b.HasOne("DAL.Models.CellState", "State")
                        .WithMany()
                        .HasForeignKey("CellStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("State");
                });

            modelBuilder.Entity("DAL.Models.Field", b =>
                {
                    b.HasOne("DAL.Models.AppUser", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("DAL.Models.Game", b =>
                {
                    b.HasOne("DAL.Models.GameState", "GameState")
                        .WithMany()
                        .HasForeignKey("GameStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameState");
                });

            modelBuilder.Entity("DAL.Models.GameField", b =>
                {
                    b.HasOne("DAL.Models.Field", "FirstField")
                        .WithMany()
                        .HasForeignKey("FirstFieldId");

                    b.HasOne("DAL.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.Field", "SecondField")
                        .WithMany()
                        .HasForeignKey("SecondFieldId");

                    b.Navigation("FirstField");

                    b.Navigation("Game");

                    b.Navigation("SecondField");
                });

            modelBuilder.Entity("DAL.Models.PlayerGame", b =>
                {
                    b.HasOne("DAL.Models.AppUser", "FirstPlayer")
                        .WithMany()
                        .HasForeignKey("FirstPlayerId");

                    b.HasOne("DAL.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.AppUser", "SecondPlayer")
                        .WithMany()
                        .HasForeignKey("SecondPlayerId");

                    b.Navigation("FirstPlayer");

                    b.Navigation("Game");

                    b.Navigation("SecondPlayer");
                });

            modelBuilder.Entity("DAL.Models.Position", b =>
                {
                    b.HasOne("DAL.Models.Cell", "Cell")
                        .WithMany()
                        .HasForeignKey("CellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.ShipWrapper", "ShipWrapper")
                        .WithMany()
                        .HasForeignKey("ShipWrapperId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cell");

                    b.Navigation("ShipWrapper");
                });

            modelBuilder.Entity("DAL.Models.Ship", b =>
                {
                    b.HasOne("DAL.Models.Direction", "Direction")
                        .WithMany()
                        .HasForeignKey("DirectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.ShipSize", "ShipSize")
                        .WithMany()
                        .HasForeignKey("ShipSizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.ShipState", "ShipState")
                        .WithMany()
                        .HasForeignKey("ShipStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Direction");

                    b.Navigation("ShipSize");

                    b.Navigation("ShipState");
                });

            modelBuilder.Entity("DAL.Models.ShipWrapper", b =>
                {
                    b.HasOne("DAL.Models.Field", "Field")
                        .WithOne("ShipWrapper")
                        .HasForeignKey("DAL.Models.ShipWrapper", "FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.Ship", "Ship")
                        .WithOne("ShipWrapper")
                        .HasForeignKey("DAL.Models.ShipWrapper", "ShipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");

                    b.Navigation("Ship");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DAL.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DAL.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DAL.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DAL.Models.Field", b =>
                {
                    b.Navigation("ShipWrapper");
                });

            modelBuilder.Entity("DAL.Models.Ship", b =>
                {
                    b.Navigation("ShipWrapper")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
