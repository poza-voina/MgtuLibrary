using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MgtuLibrary.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Author = table.Column<string>(type: "text", nullable: false),
                    NameBook = table.Column<string>(type: "text", nullable: false),
                    Town = table.Column<string>(type: "text", nullable: false),
                    Publisher = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Readers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    Group = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanOfBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateLoan = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateReturn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Tenure = table.Column<int>(type: "integer", nullable: false),
                    CurrentTenure = table.Column<int>(type: "integer", nullable: true),
                    BookId = table.Column<long>(type: "bigint", nullable: false),
                    ReaderId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanOfBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanOfBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoanOfBooks_Readers_ReaderId",
                        column: x => x.ReaderId,
                        principalTable: "Readers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "NameBook", "Publisher", "Town" },
                values: new object[,]
                {
                    { 1L, "А.С. Пушкин", "Капитанская дочка", "Publisher1", "Москва" },
                    { 2L, "Есенин С.А.", "Черный человек", "Publisher1", "Москва" },
                    { 3L, "Иван Тургенев", "Муму", "Publisher1", "Орел" }
                });

            migrationBuilder.InsertData(
                table: "Readers",
                columns: new[] { "Id", "Gender", "Group", "LastName", "Name" },
                values: new object[,]
                {
                    { 1L, "муж", "АВБ-21-2", "Петров", "Валера" },
                    { 2L, "жен", "АВБ-21-1", "Сидоров", "Семен" },
                    { 3L, "жен", "АВб-20-2", "Столешникова", "Валерия" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanOfBooks_BookId",
                table: "LoanOfBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanOfBooks_ReaderId",
                table: "LoanOfBooks",
                column: "ReaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanOfBooks");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Readers");
        }
    }
}
