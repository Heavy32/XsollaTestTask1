using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XsollaTestTask1.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentSessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(nullable: false),
                    Cost = table.Column<double>(nullable: false),
                    PaymentAppointment = table.Column<string>(nullable: true),
                    SessionRegistrationTime = table.Column<DateTime>(nullable: false),
                    LifeSpanInMinute = table.Column<int>(nullable: false),
                    Seller = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentSessions", x => x.SessionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentSessions");
        }
    }
}
