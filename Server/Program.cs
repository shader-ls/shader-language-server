using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Server;
using Serilog;
using ShaderLS.Handlers;
using ShaderLS.Management;

namespace ShaderLS
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Any(a => a.Equals("launchDebugger", StringComparison.OrdinalIgnoreCase)))
            {
                System.Diagnostics.Debugger.Launch();
            }

            Prepare();

            var server = await LanguageServer.From(Configure);

            await server.WaitForExit;
        }

        private static void Prepare()
        {
            Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .WriteTo.File(".log/log.txt", rollingInterval: RollingInterval.Day)
                        .MinimumLevel.Verbose()
                        .CreateLogger();
        }

        private static void Configure(LanguageServerOptions options)
        {
            options
                .WithInput(Console.OpenStandardInput())
                .WithOutput(Console.OpenStandardOutput())
                .ConfigureLogging(builder => builder.AddSerilog(Log.Logger).AddLanguageProtocolLogging().SetMinimumLevel(LogLevel.Debug))
                .WithHandler<TextDocumentSyncHandler>()
                .WithHandler<CompletionHandler>()
                //.WithHandler<CodeActionHandler>()
                .WithHandler<HoverHandler>()
                .WithServices(ConfigureServices);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new ConfigurationItem { Section = "shaderlab" });
            services.AddSingleton(new ConfigurationItem { Section = "shader-ls.completion.word" });
            services.AddSingleton(new DocumentSelector(
                new DocumentFilter { Pattern = "**/*.shader" },
                new DocumentFilter { Pattern = "**/*.cginc" },
                new DocumentFilter { Pattern = "**/*.glslinc" },
                new DocumentFilter { Pattern = "**/*.compute" },
                new DocumentFilter { Pattern = "**/*.cg" },
                new DocumentFilter { Pattern = "**/*.hlsl" }
                ));
            services.AddSingleton<Workspace>();
        }
    }
}
