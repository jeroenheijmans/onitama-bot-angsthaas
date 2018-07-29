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
                Move move = DetermineMove(state);

                _botProxy.Write(move);
            }
        }

        private static Move DetermineMove(GameState state)
        {
            // TODO: Determine an actual move.
            //
            // 1. See if there's an instant-win move. Even Angsthaas takes that.
            // 2. Run through all piece/card combinations
            // 3. Determine summed manhattan distance for each possible move
            // 4. Choose move with highest summed distance (break ties randomly)

            return Move.Pass(state.MyHand.First().Type);
        }
    }
}
