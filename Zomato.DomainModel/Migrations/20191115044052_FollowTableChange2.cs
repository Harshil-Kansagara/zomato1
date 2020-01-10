using Microsoft.EntityFrameworkCore.Migrations;

namespace Zomato.DomainModel.Migrations
{
    public partial class FollowTableChange2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follow_AspNetUsers_FollowerId",
                table: "Follow");

            migrationBuilder.RenameColumn(
                name: "FollowerId",
                table: "Follow",
                newName: "FollowingId");

            migrationBuilder.RenameIndex(
                name: "IX_Follow_FollowerId",
                table: "Follow",
                newName: "IX_Follow_FollowingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Follow_AspNetUsers_FollowingId",
                table: "Follow",
                column: "FollowingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Follow_AspNetUsers_FollowingId",
            //    table: "Follow");

            //migrationBuilder.RenameColumn(
            //    name: "FollowingId",
            //    table: "Follow",
            //    newName: "FollowerId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Follow_FollowingId",
            //    table: "Follow",
            //    newName: "IX_Follow_FollowerId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Follow_AspNetUsers_FollowerId",
            //    table: "Follow",
            //    column: "FollowerId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
