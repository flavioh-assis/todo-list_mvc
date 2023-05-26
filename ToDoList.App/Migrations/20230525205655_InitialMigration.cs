using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.App.Migrations
{
	public partial class InitialMigration : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Tasks",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Title = table.Column<string>(type: "varchar(20)", nullable: true),
					Description = table.Column<string>(type: "varchar(100)", nullable: true),
					IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
					CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Tasks", x => x.Id);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Tasks");
		}
	}
}
