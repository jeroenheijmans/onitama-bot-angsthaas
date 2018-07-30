using Onitama.LuCHEF.Angsthaas.Server;

namespace Onitama.LuCHEF.Angsthaas
{
    public interface IGameStateFormatter
    {
        string Format(GameState state);
    }
}
