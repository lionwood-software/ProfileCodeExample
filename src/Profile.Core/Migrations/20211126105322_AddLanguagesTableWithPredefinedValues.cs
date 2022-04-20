using Microsoft.EntityFrameworkCore.Migrations;

namespace Profile.Core.Migrations
{
    public partial class AddLanguagesTableWithPredefinedValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Languages", x => x.Id); });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Akhan" },
                    { 2, "Amharic" },
                    { 3, "Arabic" },
                    { 4, "Assamese" },
                    { 5, "Awadhi" },
                    { 6, "Azerbaijani" },
                    { 7, "Balochi" },
                    { 8, "Belarusian" },
                    { 9, "Bengali" },
                    { 10, "Bhojpuri" },
                    { 11, "Burmese" },
                    { 12, "Cebuano" },
                    { 13, "Chewa" },
                    { 14, "Chhattisgarhi" },
                    { 15, "Chittagonian" },
                    { 16, "Czech" },
                    { 17, "Deccan" },
                    { 18, "Dhundhari" },
                    { 19, "Dutch" },
                    { 20, "Eastern" },
                    { 21, "English" },
                    { 22, "French" },
                    { 23, "Fula" },
                    { 24, "Gan Chinese" },
                    { 25, "German" },
                    { 26, "Greek" },
                    { 27, "Gujarati" },
                    { 28, "Haitian" },
                    { 29, "Hakka" },
                    { 30, "Haryanvi" },
                    { 31, "Hausa" },
                    { 32, "Hiligaynon" },
                    { 33, "Hindi" },
                    { 34, "Hmong" },
                    { 35, "Hungarian" },
                    { 36, "Igbo" },
                    { 37, "Ilocano" },
                    { 38, "Italian" },
                    { 39, "Japanese" },
                    { 40, "Javanes" },
                    { 41, "Jin" },
                    { 42, "Kannada" },
                    { 43, "Kazakh" },
                    { 44, "Khme" },
                    { 45, "Kinyarwanda" },
                    { 46, "Kirundi" },
                    { 47, "Konkani" },
                    { 48, "Korean" },
                    { 49, "Kurdish" },
                    { 50, "Madures" },
                    { 51, "Magahi" },
                    { 52, "Maithili" },
                    { 53, "Malagas" },
                    { 54, "Malay" },
                    { 55, "Malayalam" },
                    { 56, "Mandarin" },
                    { 57, "Marathi" },
                    { 58, "Marwari" },
                    { 59, "Mossi" },
                    { 60, "Nepali" },
                    { 61, "Northern Min" },
                    { 62, "Odia" },
                    { 63, "Orom" },
                    { 64, "Pashto" },
                    { 65, "Persian" },
                    { 66, "Polish" },
                    { 67, "Portuguese" },
                    { 68, "Punjabi" },
                    { 69, "Quechua" },
                    { 70, "Romanian" },
                    { 71, "Russian" },
                    { 72, "Saraiki" },
                    { 73, "Serbo - Croatian" },
                    { 74, "Shona" },
                    { 75, "Sindhi" },
                    { 76, "Sinhalese" },
                    { 77, "Somali" },
                    { 78, "Southern Min" },
                    { 79, "Spanish" },
                    { 80, "Sundanese" },
                    { 81, "Swedish" },
                    { 82, "Sylheti" },
                    { 83, "Tagalog / Filipino" },
                    { 84, "Tamil" },
                    { 85, "Telugu" },
                    { 86, "Thai" },
                    { 87, "Turkish" },
                    { 88, "Turkmen" },
                    { 89, "Ukrainian" },
                    { 90, "Urdu" },
                    { 91, "Uyghur" },
                    { 92, "Uzbek" },
                    { 93, "Vietnamese" },
                    { 94, "Wu" },
                    { 95, "Xhosa" },
                    { 96, "Xiang" },
                    { 97, "Yoruba" },
                    { 98, "Yue" },
                    { 99, "Zhuang" },
                    { 100, "Zulu" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
