using Microsoft.EntityFrameworkCore.Migrations;

namespace Cards.Migrations
{
    public partial class CardsAudioFileUri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AudioFileUri",
                table: "Cards",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioFileUri",
                table: "Cards");
        }
    }
}
