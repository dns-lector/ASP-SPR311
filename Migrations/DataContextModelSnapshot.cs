﻿// <auto-generated />
using System;
using ASP_SPR311.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ASP_SPR311.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("ASP")
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ASP_SPR311.Data.Entities.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ActionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CloseAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IsCanceled")
                        .HasColumnType("int");

                    b.Property<DateTime>("OpenAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(15, 2)");

                    b.Property<Guid>("UserAccessId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserAccessId");

                    b.ToTable("Carts", "ASP");
                });

            modelBuilder.Entity("ASP_SPR311.Data.Entities.CartItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ActionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(14, 2)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartItems", "ASP");
                });

            modelBuilder.Entity("ASP_SPR311.Data.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Categories", "ASP");
                });

            modelBuilder.Entity("ASP_SPR311.Data.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagesCsv")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(12, 2)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasFilter("[Slug] IS NOT NULL");

                    b.ToTable("Products", "ASP");
                });

            modelBuilder.Entity("ASP_SPR311.Data.Entities.UserAccess", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Dk")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAccesses", "ASP");
                });

            modelBuilder.Entity("ASP_SPR311.Data.Entities.UserData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UsersData", "ASP");
                });

            modelBuilder.Entity("ASP_SPR311.Data.Entities.UserRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CanCreate")
                        .HasColumnType("int");

                    b.Property<int>("CanDelete")
                        .HasColumnType("int");

                    b.Property<int>("CanRead")
                        .HasColumnType("int");

                    b.Property<int>("CanUpdate")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserRoles", "ASP");

                    b.HasData(
                        new
                        {
                            Id = "guest",
                            CanCreate = 0,
                            CanDelete = 0,
                            CanRead = 0,
                            CanUpdate = 0,
                            Description = "Самостійно зареєстрований користувач"
                        },
                        new
                        {
                            Id = "editor",
                            CanCreate = 0,
                            CanDelete = 0,
                            CanRead = 1,
                            CanUpdate = 1,
                            Description = "З правом редагування контенту"
                        },
                        new
                        {
                            Id = "admin",
                            CanCreate = 1,
                            CanDelete = 1,
                            CanRead = 1,
                            CanUpdate = 1,
                            Description = "Адміністратор БД"
                        },
                        new
                        {
                            Id = "moderator",
                            CanCreate = 0,
                            CanDelete = 1,
                            CanRead = 1,
                            CanUpdate = 0,
                            Description = "З правом блокування контенту"
                        });
                });

            modelBuilder.Entity("ASP_SPR311.Data.Entities.Cart", b =>
                {
                    b.HasOne("ASP_SPR311.Data.Entities.UserAccess", "UserAccess")
                        .WithMany()
                        .HasForeignKey("UserAccessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserAccess");
                });

            modelBuilder.Entity("ASP_SPR311.Data.Entities.CartItem", b =>
                {
                    b.HasOne("ASP_SPR311.Data.Entities.Cart", "Cart")
                        .WithMany("CartItems")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASP_SPR311.Data.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ASP_SPR311.Data.Entities.Category", b =>
                {
                    b.HasOne("ASP_SPR311.Data.Entities.Category", "ParentCategory")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("ASP_SPR311.Data.Entities.Product", b =>
                {
                    b.HasOne("ASP_SPR311.Data.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ASP_SPR311.Data.Entities.UserAccess", b =>
                {
                    b.HasOne("ASP_SPR311.Data.Entities.UserRole", "UserRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASP_SPR311.Data.Entities.UserData", "UserData")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserData");

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("ASP_SPR311.Data.Entities.Cart", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("ASP_SPR311.Data.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
