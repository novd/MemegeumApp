using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using memegeumApp.Models;
using memegeumApp.Parsers;
using memegeumApp.Parsers.JbzdParser;
using memegeumApp.Parsers.KwejkParser;
using Microsoft.Extensions.Logging;

namespace memegeumApp.Parsers
{
    public class GeneralMemeParser : IMemeParser
    {
        private readonly List<IMemeParser> _parsers;
        private readonly ILogger<GeneralMemeParser> _logger;
        public GeneralMemeParser(ILogger<GeneralMemeParser> logger)
        {
            _logger = logger;

            _parsers =new List<IMemeParser> {
                new JbzdMemeParser(_logger),
                new KwejkMemeParser(_logger)
                };
        }

        public async Task<List<Meme>> GetMemesByNewest(int amount)
        {
            var memes = new List<Meme>();

            foreach(var parser in _parsers)
            {
                memes.AddRange(await parser.GetMemesByNewest(amount));
            }

            return memes;
        }
    }
}
