using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WampSharp.AspNetCore.WebSockets.Server;
using WampSharp.Binding;
using WampSharp.V2;
using WampSharp.V2.Realm;

namespace WampDemo.Web
{
    /// <summary>
    ///     Configuration logic for the WampDemo web application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     Configure application services.
        /// </summary>
        /// <param name="services">
        ///     The application service collection.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
        }

        /// <summary>
        ///     Configure the application pipeline.
        /// </summary>
        /// <param name="app">
        ///     The application pipeline builder.
        /// </param>
        /// <param name="loggerFactory">
        ///     The logger factory.
        /// </param>
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Trace,
                includeScopes: true
            );

            app.UseDeveloperExceptionPage();

            WampHost wampHost = new WampHost();
            app.Map("/ws", ws =>
            {
                ws.UseWebSockets(new WebSocketOptions
                {
                    ReplaceFeature = true
                });

                wampHost.RegisterTransport(
                    new AspNetCoreWebSocketTransport(ws),
                    new JTokenJsonBinding(),
                    new JTokenMsgpackBinding()
                );
            });
            wampHost.Open();

            ILogger realm1Logger = app.ApplicationServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("WAMP_REALM1");

            IWampHostedRealm realm1 = wampHost.RealmContainer.GetRealmByName("realm1");
            realm1.SessionCreated += (s, args) =>
            {
                realm1Logger.LogInformation("Session {SessionId} created.",
                    args.SessionId
                );
            };
            realm1.SessionClosed += (s, args) =>
            {
                realm1Logger.LogInformation("Session {SessionId} closed.",
                    args.SessionId
                );
            };

            ILogger realm2Logger = app.ApplicationServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("WAMP_REALM2");
            
            IWampHostedRealm realm2 = wampHost.RealmContainer.GetRealmByName("realm2");
            realm2.SessionCreated += (s, args) =>
            {
                realm2Logger.LogInformation("Session {SessionId} created.",
                    args.SessionId
                );
            };
            realm2.SessionClosed += (s, args) =>
            {
                realm2Logger.LogInformation("Session {SessionId} closed.",
                    args.SessionId
                );
            };
        }
    }
}
