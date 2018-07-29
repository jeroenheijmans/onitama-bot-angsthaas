namespace Onitama.LuCHEF.Angsthaas.Server
{
    public class Piece
    {
        public PlayerIdentity Owner { get; set; }
        public PieceType Type { get; set; }
        public Position PositionOnBoard { get; set; }

        public bool IsPawn => Type == PieceType.Pawn;
        public bool IsMaster => Type == PieceType.MasterPawn;

        public static Piece Clone(Piece exisiting)
        {
            return new Piece
            {
                Owner = exisiting.Owner,
                Type = exisiting.Type,
                PositionOnBoard = exisiting.PositionOnBoard,
            };
        }
    }
}
