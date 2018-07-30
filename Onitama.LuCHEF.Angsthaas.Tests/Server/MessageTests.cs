using Newtonsoft.Json;
using Xunit;

namespace Onitama.LuCHEF.Angsthaas.Server
{
    public class MessageTests
    {
        [Fact]
        public void Message_with_empty_GameState_survives_serialization_roundtrip()
        {
            var bot = new FakeBot(new Message
            {
                Type = MessageType.NewGameState,
                JsonPayload = JsonConvert.SerializeObject(new GameState()),
            });

            var result = bot.Read<GameState>();

            Assert.NotNull(result);
        }
    }
}
