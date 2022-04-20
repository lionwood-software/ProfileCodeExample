using CommandLine;

namespace Profile.Migrator
{
    public class Options
    {
        [Option("seedDb", Required = false, Default = true, HelpText = "Populates database with basic data")]
        public bool SeedDb { get; set; }
    }
}