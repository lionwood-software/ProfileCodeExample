using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.Core.Migrations
{
    public partial class AddTranslationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobCategoryTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCategoryId = table.Column<int>(type: "int", nullable: false),
                    Culture = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCategoryTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCategoryTranslations_JobCategories_JobCategoryId",
                        column: x => x.JobCategoryId,
                        principalTable: "JobCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobRoleTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobRoleId = table.Column<int>(type: "int", nullable: false),
                    Culture = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRoleTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobRoleTranslations_JobRoles_JobRoleId",
                        column: x => x.JobRoleId,
                        principalTable: "JobRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobCategoryTranslations_Culture",
                table: "JobCategoryTranslations",
                column: "Culture");

            migrationBuilder.CreateIndex(
                name: "IX_JobCategoryTranslations_JobCategoryId",
                table: "JobCategoryTranslations",
                column: "JobCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRoleTranslations_Culture",
                table: "JobRoleTranslations",
                column: "Culture");

            migrationBuilder.CreateIndex(
                name: "IX_JobRoleTranslations_JobRoleId",
                table: "JobRoleTranslations",
                column: "JobRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobCategoryTranslations");

            migrationBuilder.DropTable(
                name: "JobRoleTranslations");
        }
    }
}
