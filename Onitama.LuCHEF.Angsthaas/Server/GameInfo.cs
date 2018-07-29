namespace Onitama.LuCHEF.Angsthaas.Server
{
    public class GameInfo : IServerInfo
    {
        public MessageType Type => MessageType.GameInfo;

        public PlayerIdentity Identity { get; set; }
    }
}
