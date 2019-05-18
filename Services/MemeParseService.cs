using System;
using System.Threading;
using System.Threading.Tasks;
using memegeumApp.Models;
using memegeumApp.Parsers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace memegeumApp.Services
{
    public class MemeParseService : BackgroundService
    {
        private readonly ILogger<MemeParseService> _logger;
        private readonly IMemeRespository _memeRespository;

        private IMemeParser _memeParser ;

        public MemeParseService(ILogger<MemeParseService> logger, IMemeRespository memeRespository, IMemeParser memeParser)
        {
            _logger = logger;
            _memeRespository = memeRespository;

            _memeParser = memeParser;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Meme parsing is started...");

            stoppingToken.Register(() => _logger.LogDebug("Meme parsing is stopped"));

            while(!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("Parsing meme...");

                _memeRespository.AddMemes(await _memeParser.GetMemesByNewest(100));

                await Task.Delay(1000 * 60, stoppingToken);
            }

            _logger.LogDebug("Meme parsing is stopped");
        }
    }
}
