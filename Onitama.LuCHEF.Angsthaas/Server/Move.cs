namespace Onitama.LuCHEF.Angsthaas.Server
{
    public abstract class Move
    {
        public CardType UsedCard { get; }

        protected Move(CardType usedCard)
        {
            UsedCard = usedCard;
        }
    }

    public class PlayMove : Move
    {
        public Position From { get; }
        public Position To { get; }

        public PlayMove(CardType usedCard, Position from, Position to)
            : base(usedCard)
        {
            From = from;
            To = to;
        }
    }

    public class PassMove : Move
    {
        public PassMove(CardType usedCard) 
            : base(usedCard)
        { }
    }
}
