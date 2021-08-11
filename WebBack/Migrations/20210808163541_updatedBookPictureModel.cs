using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBack.Migrations
{
    public partial class updatedBookPictureModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureName",
                table: "BookPictures",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureName",
                table: "BookPictures");
        }
    }
}
