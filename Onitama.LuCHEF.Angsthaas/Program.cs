using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using RemoteBotClient;

namespace Onitama.LuCHEF.Angsthaas
{
    public class Program
    {
        private const string ApiKey = "TODO";

        static void Main(string[] args)
        {
            using (var cancellationSource = new CancellationTokenSource())
            {
                var botInterface = RemoteBotClientInitializer.Init(ApiKey, forceLocal: false);
                var testbot = new AngsthaasBot(botInterface);

                try
                {
                    Task.Factory.StartNew(
                        () => testbot.RunGameLoop(cancellationSource.Token), 
                        cancellationSource.Token
                    );

                    Console.WriteLine("Press 'Q' to quit");

                    while (Console.ReadKey().KeyChar != 'q')
                    {
                        Task.Delay(100, cancellationSource.Token);
                    }

                    cancellationSource.Cancel();
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Operation was cancelled.");
                }
                catch (AggregateException exc)
                {
                    if (!(exc.InnerException is OperationCanceledException))
                    {
                        throw;
                    }

                    Console.WriteLine("Operation was cancelled.");
                }

                forciblyCloseRemoteBot(botInterface);
                Console.WriteLine("Exiting. Press any key to quit.");
                Console.ReadKey();
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
