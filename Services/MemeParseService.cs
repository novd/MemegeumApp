using System;
using System.Threading;
using System.Threading.Tasks;
using memegeumApp.Models;
using memegeumApp.Parsers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace memegeumApp.Services
{
    public class MemeParseService : BackgroundService
    {
        private readonly ILogger<MemeParseService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private IMemeParser _memeParser ;

        public MemeParseService(ILogger<MemeParseService> logger,IServiceScopeFactory scopeFactory, IMemeParser memeParser)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            _memeParser = memeParser;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Meme parsing is started...");

            stoppingToken.Register(() => _logger.LogDebug("Meme parsing is stopped"));

            while(!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("Parsing meme...");

                using (var scope = _scopeFactory.CreateScope())
                {
                    var memeResp = scope.ServiceProvider.GetRequiredService<IMemeRespository>();
                    memeResp.AddMemes(await _memeParser.GetMemesByNewest(100));
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }

            _logger.LogDebug("Meme parsing is stopped");
        }
    }
}
