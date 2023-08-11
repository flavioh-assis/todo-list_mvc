using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.App.Migrations
{
    public partial class MakeColumnDescritionNotRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tasks",
                type: "varchar(100)",
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(100)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tasks",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true,
                oldDefaultValue: "");
        }
    }
}
