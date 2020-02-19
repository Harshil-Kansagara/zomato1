using Microsoft.EntityFrameworkCore.Migrations;

namespace Zomato.DomainModel.Migrations
{
    public partial class columnAddNotificationHub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "475f819d-e851-4f9e-92b5-b57829710bbe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52e4629d-d7a3-4ae3-a404-cc50d0c86792");

            migrationBuilder.AddColumn<string>(
                name: "UserRole",
                table: "NotificationHub",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3bc81d3c-54e6-468e-8a61-e28eed777e3b", "fcd3e68d-b525-441e-8164-8ec080160683", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4dd6bb39-ee55-4ec4-adb4-6c9829c59948", "d66b355a-8959-48c7-9e49-9fef53a31039", "user", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3bc81d3c-54e6-468e-8a61-e28eed777e3b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4dd6bb39-ee55-4ec4-adb4-6c9829c59948");

            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "NotificationHub");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "475f819d-e851-4f9e-92b5-b57829710bbe", "0fcc5880-643d-4c85-ab1c-01a4888902b1", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "52e4629d-d7a3-4ae3-a404-cc50d0c86792", "08ac0351-819c-45e6-a83d-76ec48db3c7c", "user", "USER" });
        }
    }
}
