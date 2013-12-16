using System;
using System.Linq;
using System.Threading.Tasks;

namespace Abo.Utils.Tasking
{
    public static class AsyncExtensions
    {
        public static async Task<T> WrapWithErrorIgnoring<T>(this Task<T> task, T valueOnError = default(T))
        {
            try
            {
                return await task;
            }
            catch (Exception exc)
            {
                //TODO: Log here
            }
            return valueOnError;
        }

        public static async Task WrapWithErrorIgnoring(this Task task)
        {
            try
            {
                var a = Enumerable.Range(1, 10);
                await task;
            }
            catch (Exception exc)
            {
                //TODO: Log here
            }
        }
    }
}
