using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.Core.Migrations
{
    public partial class UpdateLanguagesStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguageProficiencies_Languages_LanguageId",
                table: "UserLanguageProficiencies");

            migrationBuilder.DropIndex(
                name: "IX_UserLanguageProficiencies_LanguageId",
                table: "UserLanguageProficiencies");

            migrationBuilder.DropIndex(
                name: "IX_UserLanguageProficiencies_UserId_LanguageId",
                table: "UserLanguageProficiencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Languages",
                table: "Languages");

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 100);

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "UserLanguageProficiencies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Languages");

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "UserLanguageProficiencies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Languages",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Languages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Languages",
                table: "Languages",
                column: "Code");

            migrationBuilder.CreateTable(
                name: "LanguageTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Culture = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageTranslations_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguageProficiencies_LanguageCode",
                table: "UserLanguageProficiencies",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguageProficiencies_UserId_LanguageCode",
                table: "UserLanguageProficiencies",
                columns: new[] { "UserId", "LanguageCode" },
                unique: true,
                filter: "[LanguageCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageTranslations_Culture",
                table: "LanguageTranslations",
                column: "Culture");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageTranslations_LanguageCode",
                table: "LanguageTranslations",
                column: "LanguageCode");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguageProficiencies_Languages_LanguageCode",
                table: "UserLanguageProficiencies",
                column: "LanguageCode",
                principalTable: "Languages",
                principalColumn: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguageProficiencies_Languages_LanguageCode",
                table: "UserLanguageProficiencies");

            migrationBuilder.DropTable(
                name: "LanguageTranslations");

            migrationBuilder.DropIndex(
                name: "IX_UserLanguageProficiencies_LanguageCode",
                table: "UserLanguageProficiencies");

            migrationBuilder.DropIndex(
                name: "IX_UserLanguageProficiencies_UserId_LanguageCode",
                table: "UserLanguageProficiencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Languages",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "UserLanguageProficiencies");

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "UserLanguageProficiencies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Languages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Languages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Languages",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Languages",
                table: "Languages",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "AA", "Akhan" },
                    { 2, "AA", "Amharic" },
                    { 3, "AA", "Arabic" },
                    { 4, "AA", "Assamese" },
                    { 5, "AA", "Awadhi" },
                    { 6, "AA", "Azerbaijani" },
                    { 7, "AA", "Balochi" },
                    { 8, "AA", "Belarusian" },
                    { 9, "AA", "Bengali" },
                    { 10, "AA", "Bhojpuri" },
                    { 11, "AA", "Burmese" },
                    { 12, "AA", "Cebuano" },
                    { 13, "AA", "Chewa" },
                    { 14, "AA", "Chhattisgarhi" },
                    { 15, "AA", "Chittagonian" },
                    { 16, "AA", "Czech" },
                    { 17, "AA", "Deccan" },
                    { 18, "AA", "Dhundhari" },
                    { 19, "AA", "Dutch" },
                    { 20, "AA", "Eastern" },
                    { 21, "en", "English" },
                    { 22, "fr", "French" },
                    { 23, "AA", "Fula" },
                    { 24, "AA", "Gan Chinese" },
                    { 25, "de", "German" },
                    { 26, "AA", "Greek" },
                    { 27, "AA", "Gujarati" },
                    { 28, "AA", "Haitian" },
                    { 29, "AA", "Hakka" },
                    { 30, "AA", "Haryanvi" },
                    { 31, "AA", "Hausa" },
                    { 32, "AA", "Hiligaynon" },
                    { 33, "AA", "Hindi" },
                    { 34, "AA", "Hmong" },
                    { 35, "AA", "Hungarian" },
                    { 36, "AA", "Igbo" },
                    { 37, "AA", "Ilocano" },
                    { 38, "AA", "Italian" },
                    { 39, "AA", "Japanese" },
                    { 40, "AA", "Javanes" },
                    { 41, "AA", "Jin" },
                    { 42, "AA", "Kannada" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 43, "AA", "Kazakh" },
                    { 44, "AA", "Khme" },
                    { 45, "AA", "Kinyarwanda" },
                    { 46, "AA", "Kirundi" },
                    { 47, "AA", "Konkani" },
                    { 48, "AA", "Korean" },
                    { 49, "AA", "Kurdish" },
                    { 50, "AA", "Madures" },
                    { 51, "AA", "Magahi" },
                    { 52, "AA", "Maithili" },
                    { 53, "AA", "Malagas" },
                    { 54, "AA", "Malay" },
                    { 55, "AA", "Malayalam" },
                    { 56, "AA", "Mandarin" },
                    { 57, "AA", "Marathi" },
                    { 58, "AA", "Marwari" },
                    { 59, "AA", "Mossi" },
                    { 60, "AA", "Nepali" },
                    { 61, "AA", "Northern Min" },
                    { 62, "AA", "Odia" },
                    { 63, "AA", "Orom" },
                    { 64, "AA", "Pashto" },
                    { 65, "AA", "Persian" },
                    { 66, "AA", "Polish" },
                    { 67, "AA", "Portuguese" },
                    { 68, "AA", "Punjabi" },
                    { 69, "AA", "Quechua" },
                    { 70, "AA", "Romanian" },
                    { 71, "AA", "Russian" },
                    { 72, "AA", "Saraiki" },
                    { 73, "AA", "Serbo - Croatian" },
                    { 74, "AA", "Shona" },
                    { 75, "AA", "Sindhi" },
                    { 76, "AA", "Sinhalese" },
                    { 77, "AA", "Somali" },
                    { 78, "AA", "Southern Min" },
                    { 79, "AA", "Spanish" },
                    { 80, "AA", "Sundanese" },
                    { 81, "AA", "Swedish" },
                    { 82, "AA", "Sylheti" },
                    { 83, "AA", "Tagalog / Filipino" },
                    { 84, "AA", "Tamil" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 85, "AA", "Telugu" },
                    { 86, "AA", "Thai" },
                    { 87, "AA", "Turkish" },
                    { 88, "AA", "Turkmen" },
                    { 89, "AA", "Ukrainian" },
                    { 90, "AA", "Urdu" },
                    { 91, "AA", "Uyghur" },
                    { 92, "AA", "Uzbek" },
                    { 93, "AA", "Vietnamese" },
                    { 94, "AA", "Wu" },
                    { 95, "AA", "Xhosa" },
                    { 96, "AA", "Xiang" },
                    { 97, "AA", "Yoruba" },
                    { 98, "AA", "Yue" },
                    { 99, "AA", "Zhuang" },
                    { 100, "AA", "Zulu" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguageProficiencies_LanguageId",
                table: "UserLanguageProficiencies",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguageProficiencies_UserId_LanguageId",
                table: "UserLanguageProficiencies",
                columns: new[] { "UserId", "LanguageId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguageProficiencies_Languages_LanguageId",
                table: "UserLanguageProficiencies",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
