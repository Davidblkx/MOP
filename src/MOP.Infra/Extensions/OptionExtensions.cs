using Optional;
using Optional.Unsafe;
using System;
using System.Threading.Tasks;

namespace MOP.Infra.Extensions
{
    public static class OptionExtensions
    {
        public static bool HasValue<T>(this Option<T> option, out T value)
        {
            value = option.ValueOrDefault();
            return option.HasValue;
        }

        public static async Task<bool> AwaitSome<T>(this Option<T> option, Func<T, Task> action)
        {
            if (!option.HasValue) return false;
            await action(option.ValueOrDefault());
            return true;
        }
    }
}
