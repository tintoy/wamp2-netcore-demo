using System;
using System.Reactive.Subjects;
using System.Threading;
using WampSharp.V2;

namespace WampDemo.Client
{
    /// <summary>
    ///     A quick-and-dirty client for the WAMP server.
    /// </summary>
    static class Program
    {
        /// <summary>
        ///     The WAMP server address.
        /// </summary>
        const string ServerAddress = "ws://127.0.0.1:5050/ws";

        /// <summary>
        ///     The main program entry-point.
        /// </summary>
        static void Main()
        {
            SynchronizationContext.SetSynchronizationContext(
                new SynchronizationContext()
            );

            Console.WriteLine($"Connecting to '{ServerAddress}'...");

            DefaultWampChannelFactory channelFactory = new DefaultWampChannelFactory();
            IWampChannel channel = channelFactory.CreateJsonChannel(ServerAddress, "realm1");
            channel.Open().Wait();

            Console.WriteLine("Connected. Type stuff and press enter (or a blank line to quit).");

            ISubject<string> lineSubject = channel.RealmProxy.Services.GetSubject<string>("io.tintoy.topic1");
            lineSubject.Subscribe(
                message => Console.WriteLine("[MESSAGE] '{0}'", message),
                error => Console.WriteLine("[ERROR] '{0}'", error),
                () => Console.WriteLine("[COMPLETED]")
            );

            string currentLine;
            while ((currentLine = Console.ReadLine()) != "")
            {
                lineSubject.OnNext(currentLine);
            }
        }
    }
}
