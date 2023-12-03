using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_Authentication.Migrations
{
    public partial class RemoveAllAwardADDISWinner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommunityImpactAwardWinner",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ExCellenceAwardWinner",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PeopleChoiceAwardRunnerUp",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "PeopleChoiceAwardWinner",
                table: "Projects",
                newName: "IsWinner");

            migrationBuilder.RenameColumn(
                name: "Introduction",
                table: "Projects",
                newName: "ProjectOverView");

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnerID",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "ProjectOverView",
                table: "Projects",
                newName: "Introduction");

            migrationBuilder.RenameColumn(
                name: "IsWinner",
                table: "Projects",
                newName: "PeopleChoiceAwardWinner");

            migrationBuilder.AddColumn<bool>(
                name: "CommunityImpactAwardWinner",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExCellenceAwardWinner",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PeopleChoiceAwardRunnerUp",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
