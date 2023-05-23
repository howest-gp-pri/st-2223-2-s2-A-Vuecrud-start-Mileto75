using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pri.Ca.Infrastructure.Migrations
{
    public partial class usersWithClaims : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "Firstname", "Lastname", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "666", 0, "af5e2e53-b9f9-41bc-94d7-d73707ec4466", new DateTime(1975, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "mileto@games.com", false, "Mileto", "Di Marco", false, null, "MILETO@GAMES.COM", "MILETO@GAMES.COM", "AQAAAAEAACcQAAAAENCCL8kcrgH3lzyy0pSBwbMx1wi0u0mcFGWYlnW2qRwilZh1IGkF7sD6I7tZWtXhhg==", null, false, "78656d55-78dd-47c2-b15c-54c9563b7636", false, "mileto@games.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "Firstname", "Lastname", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "667", 0, "1e10dfc1-0d69-4cbb-8c18-6b7a0f8a60e2", new DateTime(2010, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "luca@games.com", false, "Luca", "Di Marco", false, null, "LUCA@GAMES.COM", "LUCA@GAMES.COM", "AQAAAAEAACcQAAAAEHaX2+YAiZJa6LpFjjWBbY+c8WW9fqyQZWbT0HeWZIfcMdkQLSfZ2EXBJTh4hxVNqA==", null, false, "9dad6d32-c732-41dc-8e4f-0f384ffb24a2", false, "luca@games.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 101, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", "666" },
                    { 102, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth", "19/02/1975 0:00:00", "666" },
                    { 103, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "User", "667" },
                    { 104, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth", "19/02/2010 0:00:00", "667" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "666");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "667");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");
        }
    }
}
