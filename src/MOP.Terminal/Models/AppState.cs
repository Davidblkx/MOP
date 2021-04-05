using System.Threading;

namespace MOP.Terminal.Models
{
    public static class AppState
    {
        public static int Result { get; set; } = 0;
        public static CancellationTokenSource Life => _token;

        private static readonly CancellationTokenSource _token = new();
    }
}
