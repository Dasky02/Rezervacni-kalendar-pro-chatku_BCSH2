﻿// <auto-generated />
using System;
using ChatkaReservation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChatkaReservation.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("ChatkaReservation.Models.Chatka", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Kapacita")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nazev")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Chatky");
                });

            modelBuilder.Entity("ChatkaReservation.Models.Rezervace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ChatkaId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DatumDo")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DatumOd")
                        .HasColumnType("TEXT");

                    b.Property<string>("Uzivatel")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChatkaId");

                    b.ToTable("Rezervace");
                });

            modelBuilder.Entity("ChatkaReservation.Models.Rezervace", b =>
                {
                    b.HasOne("ChatkaReservation.Models.Chatka", "Chatka")
                        .WithMany("Rezervace")
                        .HasForeignKey("ChatkaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chatka");
                });

            modelBuilder.Entity("ChatkaReservation.Models.Chatka", b =>
                {
                    b.Navigation("Rezervace");
                });
#pragma warning restore 612, 618
        }
    }
}
