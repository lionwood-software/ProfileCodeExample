using System.Text;
using System.Text.Json;

namespace Profile.Migrator
{
    public static class FileExtensions
    {
        public static async Task<T> ReadJson<T>(string path)
        {
            var serialized = await File.ReadAllTextAsync(Path.Combine(Environment.CurrentDirectory, path));
            var serializedUTF8 = Encoding.UTF8.GetString(Encoding.Default.GetBytes(serialized));
            return JsonSerializer.Deserialize<T>(serializedUTF8) ?? throw new InvalidOperationException();
        }
    }
}