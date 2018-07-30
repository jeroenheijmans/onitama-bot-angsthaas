using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RemoteBotClient;

namespace Onitama.LuCHEF.Angsthaas.Server
{
    public static class IBotInterfaceExtensions
    {
        static IBotInterfaceExtensions()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = new[] { new StringEnumConverter() },
            };
        }

        public static Message Read(this IBotInterface bot)
        {
            if (bot == null) throw new ArgumentNullException(nameof(bot));

            var data = bot.ReadLine();

            return JsonConvert.DeserializeObject<Message>(data);
        }

        public static T Read<T>(this IBotInterface bot)
            where T : IServerInfo
        {
            if (bot == null) throw new ArgumentNullException(nameof(bot));

            var message = bot.Read();

            var serverInfo = JsonConvert.DeserializeObject<T>(message.JsonPayload);

            if (serverInfo.Type != message.Type)
            {
                throw new MessageException($"Expected ${typeof(T)}, received ${message.Type}.");
            }

            return serverInfo;
        }

        public static void Write(this IBotInterface bot, Move move)
        {
            if (bot == null) throw new ArgumentNullException(nameof(bot));
            if (move == null) throw new ArgumentNullException(nameof(move));

            var message = new Message
            {
                Type = move is PassMove ? MessageType.Pass : MessageType.MovePiece,
                JsonPayload = JsonConvert.SerializeObject(move),
            };

            var data = JsonConvert.SerializeObject(message);

            bot.WriteLine(data);
        }
    }
}
