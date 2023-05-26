using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.App.Migrations
{
	public partial class ChangePropertyIsCompletedToCompletedAt : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "IsCompleted",
				table: "Tasks");

			migrationBuilder.AlterColumn<string>(
				name: "Title",
				table: "Tasks",
				type: "varchar(20)",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "varchar(20)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Description",
				table: "Tasks",
				type: "varchar(100)",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "varchar(100)",
				oldNullable: true);

			migrationBuilder.AddColumn<DateTime>(
				name: "CompletedAt",
				table: "Tasks",
				type: "datetime2",
				nullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "CompletedAt",
				table: "Tasks");

			migrationBuilder.AlterColumn<string>(
				name: "Title",
				table: "Tasks",
				type: "varchar(20)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "varchar(20)");

			migrationBuilder.AlterColumn<string>(
				name: "Description",
				table: "Tasks",
				type: "varchar(100)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "varchar(100)");

			migrationBuilder.AddColumn<bool>(
				name: "IsCompleted",
				table: "Tasks",
				type: "bit",
				nullable: false,
				defaultValue: false);
		}
	}
}
