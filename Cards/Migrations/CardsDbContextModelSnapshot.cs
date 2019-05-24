﻿// <auto-generated />
using System;
using Cards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cards.Migrations
{
    [DbContext(typeof(CardsDbContext))]
    partial class CardsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("Cards.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AudioFileUri");

                    b.Property<string>("Definition");

                    b.Property<int>("Frequency");

                    b.Property<int?>("LexicalCategory");

                    b.Property<string>("Translation");

                    b.Property<string>("Word");

                    b.HasKey("Id");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Cards.Example", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Examples");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("Examples");

                    b.ToTable("Examples");
                });

            modelBuilder.Entity("Cards.Example", b =>
                {
                    b.HasOne("Cards.Card")
                        .WithMany("Examples")
                        .HasForeignKey("Examples");
                });
#pragma warning restore 612, 618
        }
    }
}