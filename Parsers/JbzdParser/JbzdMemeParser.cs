using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using memegeumApp.Models;
using Microsoft.Extensions.Logging;

namespace memegeumApp.Parsers.JbzdParser
{
    public class JbzdMemeParser : IMemeParser
    {
        private readonly Uri _baseAddress = new Uri("https://jbzdy.pl");
        private readonly HttpClient _client;
        private const int _MEMES_PER_PAGE = 8;

        private readonly ILogger _logger;

        public JbzdMemeParser(ILogger logger)
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            _client = new HttpClient(handler) { BaseAddress = _baseAddress };

            _logger = logger;
        }

        public async Task<List<Meme>> GetMemesByNewest(int amount)
        {
            var memes = new List<Meme>();

            var pageCount = (amount + _MEMES_PER_PAGE - 1) / _MEMES_PER_PAGE;

            for (int i = 1; i <= pageCount; i++)
            {
                memes.AddRange(await GetMemesFromPage(i));
            }

            return memes.Take(amount).ToList();
        }

        private async Task<List<Meme>> GetMemesFromPage(int pageNumber)
        {
            var memesFromSpecificPage = new List<Meme>();
            var url = $"{_baseAddress}/str/{ pageNumber}";

            SendMessage($"Getting memes from URL: {url}");

            var memePage = await _client.GetAsync(url).Result.Content.ReadAsStringAsync();

            var htmlPartsContainingMeme = JbzdHtmlParser.GetMemeParts(memePage);

            foreach (var memePart in htmlPartsContainingMeme)
            {
                try
                {
                    memesFromSpecificPage.Add(new Meme()
                    {
                        Page = Meme.SourcePage.JBZD,
                        Title = JbzdHtmlParser.GetTitleOfMeme(memePart),
                        ImagePath = JbzdHtmlParser.GetMemeImgPath(memePart),
                        Tags = JbzdHtmlParser.GetMemeTags(memePart)
                    });
                }
                catch (Exception e)
                {
                    SendMessage($"Exception catched: {e.Message} on page {url}");
                }
            }

            return memesFromSpecificPage;
        }

        void SendMessage(string message)
        {
            var messageSource = "JbzdMemeParser";
            _logger.LogDebug($"{messageSource}: {message}");
        }

    }
}
