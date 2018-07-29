using System;
using System.Linq;
using System.Threading;
using Onitama.LuCHEF.Angsthaas.Server;
using RemoteBotClient;

namespace Onitama.LuCHEF.Angsthaas
{
    public class AngsthaasBot
    {
        private readonly IBotInterface _botProxy;

        public AngsthaasBot(IBotInterface botProxy)
        {
            _botProxy = botProxy;
        }

        public void RunGameLoop(CancellationToken cancellationToken)
        {
            var gameInfo = _botProxy.Read<GameInfo>();

            while(true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var state = _botProxy.Read<GameState>();

                Console.WriteLine(CondensedUtf8Formatter.Instance.Format(state));
                
                // TODO: Determine an actual move.
                var move = Move.Pass(state.MyHand.First().Type);

                _botProxy.Write(move);
            }
        }
    }
}
