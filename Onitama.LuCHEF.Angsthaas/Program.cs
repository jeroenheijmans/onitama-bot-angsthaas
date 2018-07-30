using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using RemoteBotClient;

namespace Onitama.LuCHEF.Angsthaas
{
    public class Program
    {
        static void Main(string[] args)
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(dir, "apikey.txt");
            var apiKey = File.ReadAllText(path).Trim();

            using (var cancellationSource = new CancellationTokenSource())
            {
                var botInterface = RemoteBotClientInitializer.Init(apiKey, forceLocal: false);
                var testbot = new AngsthaasBot(botInterface);

                try
                {
                    Task.Factory.StartNew(
                        () => testbot.RunGameLoop(cancellationSource.Token), 
                        cancellationSource.Token
                    );

                    botInterface.Log("Press 'Q' to quit");

                    while (Console.IsInputRedirected || Console.ReadKey().KeyChar != 'q')
                    {
                        Task.Delay(100, cancellationSource.Token);
                    }

                    cancellationSource.Cancel();
                }
                catch (OperationCanceledException)
                {
                    botInterface.Log("Operation was cancelled.");
                }
                catch (AggregateException exc)
                {
                    if (!(exc.InnerException is OperationCanceledException))
                    {
                        throw;
                    }

                    botInterface.Log("Operation was cancelled.");
                }

                if (!Console.IsInputRedirected)
                {
                    forciblyCloseRemoteBot(botInterface);
                    botInterface.Log("Exiting. Press any key to quit.");
                    Console.ReadKey();
                }
            }
        }

        private static void forciblyCloseRemoteBot(IBotInterface botInterface)
        {
            // This is not very nice, but there seems to be no other way
            // to disconnect. And without disconnecting, you can't even
            // kill the console app without force-quitting it. So it'll
            // have to do.

            try
            {
                var remoteBotClient = botInterface as RemoteBotClient.RemoteBotClient;
                var field = remoteBotClient.GetType().GetField("_client", BindingFlags.Instance | BindingFlags.NonPublic);
                var client = field.GetValue(remoteBotClient);
                var lidclient = client as Lidgren.Network.NetClient;
                lidclient.Disconnect("Adios!");
                Task.Delay(500).Wait();
            }
            catch (Exception)
            {
                // Yes, an anti-pattern. But the above dirty hack might die
                // in weird and unexpected ways, and it shouldn't affect 
                // running a game ever. So hey, let's fly with this...
            }
        }
    }
}
