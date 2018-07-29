namespace Onitama.LuCHEF.Angsthaas.Server
{
    public class GameState : IServerInfo
    {
        public MessageType Type => MessageType.NewGameState;

        public int BoardRevision { get; set; }

        public Card[] MyHand { get; set; }
        public Card[] OpponentsHand { get; set; }
        public Card FifthCard { get; set; }
        public Piece[] Pieces { get; set; }
        public PlayerIdentity CurrentlyPlaying { get; set; }
    }
}
