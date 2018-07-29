using System.Linq;
using System.Text;

namespace Onitama.LuCHEF.Angsthaas.Server
{
    public class CondensedUtf8Formatter : IGameStateFormatter
    {
        public static CondensedUtf8Formatter Instance => new CondensedUtf8Formatter();

        public string Format(GameState state)
        {
            var sb = new StringBuilder();

            sb.AppendLine("┌─────┐");

            for (var y = 0; y < 5; y++)
            {
                sb.Append("│");

                for (var x = 0; x < 5; x++)
                {
                    var piece = state.Pieces.FirstOrDefault(p => p.PositionOnBoard.X == x && p.PositionOnBoard.Y == y);
                    sb.Append(Format(piece));
                }

                sb.Append("│");
                sb.AppendLine();
            }

            sb.AppendLine("└─────┘");

            return sb.ToString();
        }

        private string Format(Piece piece)
        {
            if (piece == null)
            {
                return "·";
            }
            else if (piece.Owner == PlayerIdentity.Player1)
            {
                return piece.IsMaster ? "X" : "x";
            }
            else
            {
                return piece.IsMaster ? "O" : "o";
            }
        }
    }
}
