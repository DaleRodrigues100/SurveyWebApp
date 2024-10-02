using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XSLearning.Migrations
{
    public partial class FixingresponsesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Responses",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "NumberId",
                table: "Responses");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Responses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Responses",
                table: "Responses",
                columns: new[] { "Username", "SurveyId", "QuestionId", "OptionId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Responses",
                table: "Responses");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Responses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "NumberId",
                table: "Responses",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Responses",
                table: "Responses",
                column: "NumberId");
        }
    }
}
