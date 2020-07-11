﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using XsollaTestTask1.Contexts;

namespace XsollaTestTask1.Migrations
{
    [DbContext(typeof(PaymentSessionDBContext))]
    partial class PaymentSessionDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("XsollaTestTask1.Models.PaymentSession", b =>
                {
                    b.Property<Guid>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<int>("LifeSpanInMinute")
                        .HasColumnType("int");

                    b.Property<string>("PaymentAppointment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Seller")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SessionRegistrationTime")
                        .HasColumnType("datetime2");

                    b.HasKey("SessionId");

                    b.ToTable("PaymentSessions");
                });
#pragma warning restore 612, 618
        }
    }
}
