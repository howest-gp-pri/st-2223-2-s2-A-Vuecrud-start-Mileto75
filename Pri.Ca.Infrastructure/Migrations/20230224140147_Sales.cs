using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pri.Ca.Infrastructure.Migrations
{
    public partial class Sales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "Id", "GameId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 10 },
                    { 3, 3, 2 },
                    { 4, 1, 8 },
                    { 5, 2, 12 },
                    { 6, 3, 4 },
                    { 7, 1, 10 },
                    { 8, 2, 1 },
                    { 9, 3, 5 },
                    { 10, 1, 13 },
                    { 12, 2, 1 },
                    { 13, 3, 4 },
                    { 14, 1, 8 },
                    { 15, 2, 4 },
                    { 16, 3, 23 },
                    { 17, 1, 2 },
                    { 18, 2, 7 },
                    { 19, 3, 9 },
                    { 20, 1, 10 },
                    { 21, 2, 2 },
                    { 22, 3, 5 },
                    { 23, 1, 9 },
                    { 24, 2, 8 },
                    { 25, 3, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_GameId",
                table: "Sales",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sales");
        }
    }
}
