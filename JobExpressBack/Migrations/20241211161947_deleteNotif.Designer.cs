﻿// <auto-generated />
using System;
using JobExpressBack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JobExpressBack.Migrations
{
    [DbContext(typeof(ExJobDBContext))]
    [Migration("20241211161947_deleteNotif")]
    partial class deleteNotif
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JobExpressBack.Models.DTOs.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Disponibility")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FcmToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

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

                    b.Property<string>("PhotoProfile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ServiceID")
                        .HasColumnType("int");

                    b.Property<int>("ServicesProvidedCount")
                        .HasColumnType("int");

                    b.Property<string>("Skills")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telephone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("YearsOfExperience")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("ServiceID");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("JobExpressBack.Models.Entities.Avis", b =>
                {
                    b.Property<int>("AvisID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AvisID"));

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateAvis")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateDemande")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProfessionnelId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Rate")
                        .HasColumnType("int");

                    b.HasKey("AvisID");

                    b.HasIndex("ClientId", "ProfessionnelId", "DateDemande");

                    b.ToTable("Avis");
                });

            modelBuilder.Entity("JobExpressBack.Models.Entities.Categorie", b =>
                {
                    b.Property<int>("CategorieID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategorieID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategorieID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("JobExpressBack.Models.Entities.DemandeService", b =>
                {
                    b.Property<string>("ClientId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(0);

                    b.Property<string>("ProfessionnelId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(1);

                    b.Property<DateTime>("DateDemande")
                        .HasColumnType("datetime2")
                        .HasColumnOrder(2);

                    b.Property<DateTime?>("ExecutionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Statut")
                        .HasColumnType("int");

                    b.Property<decimal?>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ClientId", "ProfessionnelId", "DateDemande");

                    b.HasIndex("ProfessionnelId");

                    b.ToTable("DemandeServices");
                });

            modelBuilder.Entity("JobExpressBack.Models.Entities.Service", b =>
                {
                    b.Property<int>("ServiceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ServiceID"));

                    b.Property<int>("CategorieID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreation")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ServiceID");

                    b.HasIndex("CategorieID");

                    b.ToTable("Services");
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

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

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

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

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

            modelBuilder.Entity("JobExpressBack.Models.DTOs.ApplicationUser", b =>
                {
                    b.HasOne("JobExpressBack.Models.Entities.Service", "Service")
                        .WithMany("Professionnels")
                        .HasForeignKey("ServiceID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Service");
                });

            modelBuilder.Entity("JobExpressBack.Models.Entities.Avis", b =>
                {
                    b.HasOne("JobExpressBack.Models.Entities.DemandeService", "DemandeService")
                        .WithMany("Avis")
                        .HasForeignKey("ClientId", "ProfessionnelId", "DateDemande")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("DemandeService");
                });

            modelBuilder.Entity("JobExpressBack.Models.Entities.DemandeService", b =>
                {
                    b.HasOne("JobExpressBack.Models.DTOs.ApplicationUser", "Client")
                        .WithMany("ServicesRequested")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("JobExpressBack.Models.DTOs.ApplicationUser", "Professionnel")
                        .WithMany("ServicesProvided")
                        .HasForeignKey("ProfessionnelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Professionnel");
                });

            modelBuilder.Entity("JobExpressBack.Models.Entities.Service", b =>
                {
                    b.HasOne("JobExpressBack.Models.Entities.Categorie", "Categorie")
                        .WithMany("Services")
                        .HasForeignKey("CategorieID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categorie");
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
                    b.HasOne("JobExpressBack.Models.DTOs.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("JobExpressBack.Models.DTOs.ApplicationUser", null)
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

                    b.HasOne("JobExpressBack.Models.DTOs.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("JobExpressBack.Models.DTOs.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JobExpressBack.Models.DTOs.ApplicationUser", b =>
                {
                    b.Navigation("ServicesProvided");

                    b.Navigation("ServicesRequested");
                });

            modelBuilder.Entity("JobExpressBack.Models.Entities.Categorie", b =>
                {
                    b.Navigation("Services");
                });

            modelBuilder.Entity("JobExpressBack.Models.Entities.DemandeService", b =>
                {
                    b.Navigation("Avis");
                });

            modelBuilder.Entity("JobExpressBack.Models.Entities.Service", b =>
                {
                    b.Navigation("Professionnels");
                });
#pragma warning restore 612, 618
        }
    }
}
