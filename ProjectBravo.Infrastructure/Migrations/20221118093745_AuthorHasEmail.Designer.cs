﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectBravo.Infrastructure;

#nullable disable

namespace ProjectBravo.Infrastructure.Migrations
{
    [DbContext(typeof(GitContext))]
    [Migration("20221118093745_AuthorHasEmail")]
    partial class AuthorHasEmail
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ProjectBravo.Infrastructure.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GitRepositoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GitRepositoryId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("ProjectBravo.Infrastructure.Commit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("GitRepositoryId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RepositoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("GitRepositoryId");

                    b.ToTable("Commits");
                });

            modelBuilder.Entity("ProjectBravo.Infrastructure.GitRepository", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("LatestCommitDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Repos");
                });

            modelBuilder.Entity("ProjectBravo.Infrastructure.Author", b =>
                {
                    b.HasOne("ProjectBravo.Infrastructure.GitRepository", null)
                        .WithMany("Authors")
                        .HasForeignKey("GitRepositoryId");
                });

            modelBuilder.Entity("ProjectBravo.Infrastructure.Commit", b =>
                {
                    b.HasOne("ProjectBravo.Infrastructure.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectBravo.Infrastructure.GitRepository", null)
                        .WithMany("Commits")
                        .HasForeignKey("GitRepositoryId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("ProjectBravo.Infrastructure.GitRepository", b =>
                {
                    b.Navigation("Authors");

                    b.Navigation("Commits");
                });
#pragma warning restore 612, 618
        }
    }
}
