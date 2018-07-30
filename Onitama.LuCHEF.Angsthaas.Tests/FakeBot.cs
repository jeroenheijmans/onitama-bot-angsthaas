using System.Collections.Generic;
using Newtonsoft.Json;
using Onitama.LuCHEF.Angsthaas.Server;
using RemoteBotClient;

namespace Onitama.LuCHEF.Angsthaas
{
    public class FakeBot : IBotInterface
    {
        public Message NextMessage { get; set; }
        public ICollection<string> Logs { get; } = new List<string>();
        public ICollection<string> WrittenLines { get; } = new List<string>();

        public FakeBot(Message nextMessage)
        {
            NextMessage = nextMessage;
        }

        public void Log(string message)
        {
            Logs.Add(message);
        }

        public string ReadLine()
        {
            return JsonConvert.SerializeObject(NextMessage);
        }

        public void WriteLine(string line)
        {
            WrittenLines.Add(line);
        }
    }
}
