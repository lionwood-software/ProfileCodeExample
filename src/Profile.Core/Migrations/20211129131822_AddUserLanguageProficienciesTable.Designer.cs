// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Profile.Core;

namespace Profile.Core.Migrations
{
    [DbContext(typeof(ProfileDbContext))]
    [Migration("20211129131822_AddUserLanguageProficienciesTable")]
    partial class AddUserLanguageProficienciesTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Profile.Core.Entities.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Languages");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Akhan"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Amharic"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Arabic"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Assamese"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Awadhi"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Azerbaijani"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Balochi"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Belarusian"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Bengali"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Bhojpuri"
                        },
                        new
                        {
                            Id = 11,
                            Name = "Burmese"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Cebuano"
                        },
                        new
                        {
                            Id = 13,
                            Name = "Chewa"
                        },
                        new
                        {
                            Id = 14,
                            Name = "Chhattisgarhi"
                        },
                        new
                        {
                            Id = 15,
                            Name = "Chittagonian"
                        },
                        new
                        {
                            Id = 16,
                            Name = "Czech"
                        },
                        new
                        {
                            Id = 17,
                            Name = "Deccan"
                        },
                        new
                        {
                            Id = 18,
                            Name = "Dhundhari"
                        },
                        new
                        {
                            Id = 19,
                            Name = "Dutch"
                        },
                        new
                        {
                            Id = 20,
                            Name = "Eastern"
                        },
                        new
                        {
                            Id = 21,
                            Name = "English"
                        },
                        new
                        {
                            Id = 22,
                            Name = "French"
                        },
                        new
                        {
                            Id = 23,
                            Name = "Fula"
                        },
                        new
                        {
                            Id = 24,
                            Name = "Gan Chinese"
                        },
                        new
                        {
                            Id = 25,
                            Name = "German"
                        },
                        new
                        {
                            Id = 26,
                            Name = "Greek"
                        },
                        new
                        {
                            Id = 27,
                            Name = "Gujarati"
                        },
                        new
                        {
                            Id = 28,
                            Name = "Haitian"
                        },
                        new
                        {
                            Id = 29,
                            Name = "Hakka"
                        },
                        new
                        {
                            Id = 30,
                            Name = "Haryanvi"
                        },
                        new
                        {
                            Id = 31,
                            Name = "Hausa"
                        },
                        new
                        {
                            Id = 32,
                            Name = "Hiligaynon"
                        },
                        new
                        {
                            Id = 33,
                            Name = "Hindi"
                        },
                        new
                        {
                            Id = 34,
                            Name = "Hmong"
                        },
                        new
                        {
                            Id = 35,
                            Name = "Hungarian"
                        },
                        new
                        {
                            Id = 36,
                            Name = "Igbo"
                        },
                        new
                        {
                            Id = 37,
                            Name = "Ilocano"
                        },
                        new
                        {
                            Id = 38,
                            Name = "Italian"
                        },
                        new
                        {
                            Id = 39,
                            Name = "Japanese"
                        },
                        new
                        {
                            Id = 40,
                            Name = "Javanes"
                        },
                        new
                        {
                            Id = 41,
                            Name = "Jin"
                        },
                        new
                        {
                            Id = 42,
                            Name = "Kannada"
                        },
                        new
                        {
                            Id = 43,
                            Name = "Kazakh"
                        },
                        new
                        {
                            Id = 44,
                            Name = "Khme"
                        },
                        new
                        {
                            Id = 45,
                            Name = "Kinyarwanda"
                        },
                        new
                        {
                            Id = 46,
                            Name = "Kirundi"
                        },
                        new
                        {
                            Id = 47,
                            Name = "Konkani"
                        },
                        new
                        {
                            Id = 48,
                            Name = "Korean"
                        },
                        new
                        {
                            Id = 49,
                            Name = "Kurdish"
                        },
                        new
                        {
                            Id = 50,
                            Name = "Madures"
                        },
                        new
                        {
                            Id = 51,
                            Name = "Magahi"
                        },
                        new
                        {
                            Id = 52,
                            Name = "Maithili"
                        },
                        new
                        {
                            Id = 53,
                            Name = "Malagas"
                        },
                        new
                        {
                            Id = 54,
                            Name = "Malay"
                        },
                        new
                        {
                            Id = 55,
                            Name = "Malayalam"
                        },
                        new
                        {
                            Id = 56,
                            Name = "Mandarin"
                        },
                        new
                        {
                            Id = 57,
                            Name = "Marathi"
                        },
                        new
                        {
                            Id = 58,
                            Name = "Marwari"
                        },
                        new
                        {
                            Id = 59,
                            Name = "Mossi"
                        },
                        new
                        {
                            Id = 60,
                            Name = "Nepali"
                        },
                        new
                        {
                            Id = 61,
                            Name = "Northern Min"
                        },
                        new
                        {
                            Id = 62,
                            Name = "Odia"
                        },
                        new
                        {
                            Id = 63,
                            Name = "Orom"
                        },
                        new
                        {
                            Id = 64,
                            Name = "Pashto"
                        },
                        new
                        {
                            Id = 65,
                            Name = "Persian"
                        },
                        new
                        {
                            Id = 66,
                            Name = "Polish"
                        },
                        new
                        {
                            Id = 67,
                            Name = "Portuguese"
                        },
                        new
                        {
                            Id = 68,
                            Name = "Punjabi"
                        },
                        new
                        {
                            Id = 69,
                            Name = "Quechua"
                        },
                        new
                        {
                            Id = 70,
                            Name = "Romanian"
                        },
                        new
                        {
                            Id = 71,
                            Name = "Russian"
                        },
                        new
                        {
                            Id = 72,
                            Name = "Saraiki"
                        },
                        new
                        {
                            Id = 73,
                            Name = "Serbo - Croatian"
                        },
                        new
                        {
                            Id = 74,
                            Name = "Shona"
                        },
                        new
                        {
                            Id = 75,
                            Name = "Sindhi"
                        },
                        new
                        {
                            Id = 76,
                            Name = "Sinhalese"
                        },
                        new
                        {
                            Id = 77,
                            Name = "Somali"
                        },
                        new
                        {
                            Id = 78,
                            Name = "Southern Min"
                        },
                        new
                        {
                            Id = 79,
                            Name = "Spanish"
                        },
                        new
                        {
                            Id = 80,
                            Name = "Sundanese"
                        },
                        new
                        {
                            Id = 81,
                            Name = "Swedish"
                        },
                        new
                        {
                            Id = 82,
                            Name = "Sylheti"
                        },
                        new
                        {
                            Id = 83,
                            Name = "Tagalog / Filipino"
                        },
                        new
                        {
                            Id = 84,
                            Name = "Tamil"
                        },
                        new
                        {
                            Id = 85,
                            Name = "Telugu"
                        },
                        new
                        {
                            Id = 86,
                            Name = "Thai"
                        },
                        new
                        {
                            Id = 87,
                            Name = "Turkish"
                        },
                        new
                        {
                            Id = 88,
                            Name = "Turkmen"
                        },
                        new
                        {
                            Id = 89,
                            Name = "Ukrainian"
                        },
                        new
                        {
                            Id = 90,
                            Name = "Urdu"
                        },
                        new
                        {
                            Id = 91,
                            Name = "Uyghur"
                        },
                        new
                        {
                            Id = 92,
                            Name = "Uzbek"
                        },
                        new
                        {
                            Id = 93,
                            Name = "Vietnamese"
                        },
                        new
                        {
                            Id = 94,
                            Name = "Wu"
                        },
                        new
                        {
                            Id = 95,
                            Name = "Xhosa"
                        },
                        new
                        {
                            Id = 96,
                            Name = "Xiang"
                        },
                        new
                        {
                            Id = 97,
                            Name = "Yoruba"
                        },
                        new
                        {
                            Id = 98,
                            Name = "Yue"
                        },
                        new
                        {
                            Id = 99,
                            Name = "Zhuang"
                        },
                        new
                        {
                            Id = 100,
                            Name = "Zulu"
                        });
                });

            modelBuilder.Entity("Profile.Core.Entities.Proficiency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Proficiencies");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Beginner"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Intermediate"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Advanced"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Native"
                        });
                });

            modelBuilder.Entity("Profile.Core.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<int>("InternalId")
                        .HasColumnType("int");

                    b.Property<bool>("IsOnboarded")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("LanguageCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("LastName")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Profile.Core.Entities.UserLanguageProficiency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LanguageId")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProficiencyId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.HasIndex("ProficiencyId");

                    b.HasIndex("UserId", "LanguageId")
                        .IsUnique();

                    b.ToTable("UserLanguageProficiencies");
                });

            modelBuilder.Entity("Profile.Core.Entities.UserLanguageProficiency", b =>
                {
                    b.HasOne("Profile.Core.Entities.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Profile.Core.Entities.Proficiency", "Proficiency")
                        .WithMany()
                        .HasForeignKey("ProficiencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Profile.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("Proficiency");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
