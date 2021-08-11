using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBack.Migrations
{
    public partial class AddedBookPictureModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureUri",
                table: "Books");

            migrationBuilder.CreateTable(
                name: "BookPictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    PictureUri = table.Column<string>(type: "TEXT", nullable: true),
                    BookId = table.Column<int>(type: "INTEGER", nullable: false),
                    BookId1 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookPictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookPictures_Books_BookId1",
                        column: x => x.BookId1,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookPictures_BookId1",
                table: "BookPictures",
                column: "BookId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookPictures");

            migrationBuilder.AddColumn<string>(
                name: "PictureUri",
                table: "Books",
                type: "TEXT",
                nullable: true);
        }
    }
}
