using System;
using System.Threading;
using System.Threading.Tasks;

namespace MOP.Core.Infra
{
    public class MopLifeService
    {
        private readonly CancellationToken _token;

        public MopLifeService(CancellationToken token)
        {
            _token = token;
        }

        public Task WaitForExit()
            => Task.Run(() => { while (!_token.IsCancellationRequested) { } });
    }
}
