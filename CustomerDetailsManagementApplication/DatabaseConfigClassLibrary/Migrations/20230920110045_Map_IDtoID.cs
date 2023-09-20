using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseConfigClassLibrary.Migrations
{
    public partial class Map_IDtoID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_id",
                table: "UserDatas",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserDatas",
                newName: "_id");
        }
    }
}
