namespace Onitama.LuCHEF.Angsthaas.Server
{
    public enum MessageType
    {
        // Messages from bot to game:
        MovePiece,
        Pass,

        // Messages from game to bot:
        GameInfo,
        NewGameState,
        InvalidMove,
    }
}
