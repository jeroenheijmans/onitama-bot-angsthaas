namespace Onitama.LuCHEF.Angsthaas.Server
{
    public class Move
    {
        public CardType Card { get; }
        public Position? From { get; }
        public Position? To { get; }
        public bool IsPass => From == null;

        private Move(CardType card)
        {
            this.Card = card;
        }

        public Move(CardType card, Position from, Position to)
            : this(card)
        {
            From = from;
            To = to;
        }

        public static Move Pass(CardType card)
        {
            return new Move(card);
        }
    }
}
