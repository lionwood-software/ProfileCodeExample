using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.Core.Migrations
{
    public partial class AddLanguageProficienciesTranslations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF(EXISTS(SELECT TOP 1 * FROM [dbo].[UserLanguageProficiencies]))
                BEGIN
                    RAISERROR('Can not apply migration because UserLanguageProficiencies table is not empty', 16, 1)
                END"
            );

            migrationBuilder.Sql(@"
                DELETE FROM Proficiencies
                DBCC CHECKIDENT ('[dbo].[Proficiencies]', RESEED, 0)
            ");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Proficiencies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProficiencyTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProficiencyId = table.Column<int>(type: "int", nullable: false),
                    Culture = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProficiencyTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProficiencyTranslations_Proficiencies_ProficiencyId",
                        column: x => x.ProficiencyId,
                        principalTable: "Proficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProficiencyTranslations_Culture",
                table: "ProficiencyTranslations",
                column: "Culture");

            migrationBuilder.CreateIndex(
                name: "IX_ProficiencyTranslations_ProficiencyId",
                table: "ProficiencyTranslations",
                column: "ProficiencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProficiencyTranslations");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Proficiencies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Proficiencies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Beginner" },
                    { 2, "Intermediate" },
                    { 3, "Advanced" },
                    { 4, "Native" }
                });
        }
    }
}
