using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace day1lab.Migrations
{
    /// <inheritdoc />
    public partial class nullablenot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Departments_DeptID",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "DeptID",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Departments_DeptID",
                table: "Students",
                column: "DeptID",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Departments_DeptID",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "DeptID",
                table: "Students",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Departments_DeptID",
                table: "Students",
                column: "DeptID",
                principalTable: "Departments",
                principalColumn: "Id");
        }
    }
}
