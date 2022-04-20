using System.Threading.Tasks;
using Profile.Core.SharedKernel.Exceptions;

namespace Profile.Core.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<T> ThrowIfNotFound<T>(this Task<T> task)
        {
            var result = await task;
            return result ?? throw new NotFoundObjectException("Object not found");
        }

        public static async Task<T> ThrowIfNotFound<T>(this ValueTask<T> task)
        {
            var result = await task;
            return result ?? throw new NotFoundObjectException("Object not found");
        }
    }
}
