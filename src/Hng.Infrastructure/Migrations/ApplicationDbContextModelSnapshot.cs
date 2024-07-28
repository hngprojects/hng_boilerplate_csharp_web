﻿// <auto-generated />
using System;
using Hng.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hng.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Hng.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("ParentId")
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Hng.Domain.Entities.Job", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Company")
                        .HasColumnType("text");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<double>("Salary")
                        .HasColumnType("double precision");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("Hng.Domain.Entities.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Industry")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Slug")
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Hng.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Available")
                        .HasColumnType("boolean");

                    b.Property<string>("Category")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Products");

                    b.HasAnnotation("Relational:JsonPropertyName", "product");
                });

            modelBuilder.Entity("Hng.Domain.Entities.Profile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("Hng.Domain.Entities.Subscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Frequency")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("OrganizationId")
                        .HasColumnType("uuid");

                    b.Property<int>("Plan")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("TransactionId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("TransactionId");

                    b.HasIndex("UserId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("Hng.Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasAnnotation("Relational:JsonPropertyName", "amount");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "created_at");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "modified_at");

                    b.Property<DateTime?>("PaidAt")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "paid_at");

                    b.Property<int>("Partners")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "partners");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "product_id");

                    b.Property<string>("Reference")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "reference");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "status");

                    b.Property<Guid?>("SubscriptionId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "type");


                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "user_id");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SubscriptionId");

                    b.HasIndex("UserId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Hng.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("PasswordSalt")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasAnnotation("Relational:JsonPropertyName", "user");
                });

            modelBuilder.Entity("OrganizationUser", b =>
                {
                    b.Property<Guid>("OrganizationsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid");

                    b.HasKey("OrganizationsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("OrganizationUser");
                });

            modelBuilder.Entity("Hng.Domain.Entities.Product", b =>
                {
                    b.HasOne("Hng.Domain.Entities.User", "User")
                        .WithMany("Products")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Hng.Domain.Entities.Profile", b =>
                {
                    b.HasOne("Hng.Domain.Entities.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("Hng.Domain.Entities.Profile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Hng.Domain.Entities.Subscription", b =>
                {
                    b.HasOne("Hng.Domain.Entities.Organization", "Organization")
                        .WithMany("Subscriptions")
                        .HasForeignKey("OrganizationId");

                    b.HasOne("Hng.Domain.Entities.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId");

                    b.HasOne("Hng.Domain.Entities.User", "User")
                        .WithMany("Subscriptions")
                        .HasForeignKey("UserId");

                    b.Navigation("Organization");

                    b.Navigation("Transaction");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Hng.Domain.Entities.Transaction", b =>
                {
                    b.HasOne("Hng.Domain.Entities.Product", "Product")
                        .WithMany("Transactions")
                        .HasForeignKey("ProductId");

                    b.HasOne("Hng.Domain.Entities.Subscription", "Subscription")
                        .WithMany()
                        .HasForeignKey("SubscriptionId");

                    b.HasOne("Hng.Domain.Entities.User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("UserId");

                    b.Navigation("Product");

                    b.Navigation("Subscription");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OrganizationUser", b =>
                {
                    b.HasOne("Hng.Domain.Entities.Organization", null)
                        .WithMany()
                        .HasForeignKey("OrganizationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hng.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hng.Domain.Entities.Organization", b =>
                {
                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("Hng.Domain.Entities.Product", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Hng.Domain.Entities.User", b =>
                {
                    b.Navigation("Products");

                    b.Navigation("Profile");

                    b.Navigation("Subscriptions");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
