using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    public partial class InitUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Borrower",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrower", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BorrowingHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BorrowerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowingHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowingHistory_Borrower_BorrowerId",
                        column: x => x.BorrowerId,
                        principalTable: "Borrower",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BorrowingDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BorrowingHistoryId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowingDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowingDetail_BorrowingHistory_BorrowingHistoryId",
                        column: x => x.BorrowingHistoryId,
                        principalTable: "BorrowingHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BorrowingDetail_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingDetail_BorrowingHistoryId",
                table: "BorrowingDetail",
                column: "BorrowingHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingDetail_ItemId",
                table: "BorrowingDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingHistory_BorrowerId",
                table: "BorrowingHistory",
                column: "BorrowerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorrowingDetail");

            migrationBuilder.DropTable(
                name: "BorrowingHistory");

            migrationBuilder.DropTable(
                name: "Borrower");
        }
    }
}
