using System.Threading.Tasks;

namespace Profile.Core.FileManager
{
    public interface IAvatarStorage
    {
        Task<string> Upload(byte[] file, string fileName);
        Task Delete(string fileName);
    }
}
