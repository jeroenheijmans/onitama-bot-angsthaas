namespace Onitama.LuCHEF.Angsthaas.Server
{
    public class GameState : BoardPosition, IServerInfo
    {
        public MessageType Type => MessageType.NewGameState;

        public int BoardRevision { get; set; }

        public Card[] MyHand { get; set; }
        public Card[] OpponentsHand { get; set; }
        public Card FifthCard { get; set; }
        public PlayerIdentity CurrentlyPlaying { get; set; }
    }

    public class BoardPosition
    {
        public Piece[] Pieces { get; set; }

        public static BoardPosition Clone(BoardPosition existing)
        {
            var fresh = new BoardPosition { Pieces = new Piece[existing.Pieces.Length] };

            for (int i = 0; i < existing.Pieces.Length; i++)
            {
                fresh.Pieces[i] = Piece.Clone(existing.Pieces[i]);
            }

            return fresh;
        }
    }
}
