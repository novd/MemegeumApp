using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using memegeumApp.Models;
using Microsoft.Extensions.Logging;

namespace memegeumApp.Parsers.KwejkParser
{
    public class KwejkMemeParser : IMemeParser
    {
        private readonly Uri _baseAddress = new Uri("https://kwejk.pl");
        private readonly HttpClient _client;
        private const int _MEMES_PER_PAGE = 8;

        private readonly ILogger _logger;

        public KwejkMemeParser(ILogger logger)
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
            var newestPageNumber = await GetNewestPageNumber();
            var lastPageNumber = newestPageNumber - pageCount;

            for (int i = newestPageNumber; i > lastPageNumber; i--)
            {
                memes.AddRange(await GetMemesFromPage(i));
            }

            return memes.Take(amount).ToList();
        }

        private async Task<List<Meme>> GetMemesFromPage(int pageNumber)
        {
            var memesFromSpecificPage = new List<Meme>();
            var url = $"{_baseAddress}/strona/{pageNumber}";

            SendMessage($"Getting memes from URL: {url}");

            var memePage = await _client.GetAsync(url).Result.Content.ReadAsStringAsync();

            var htmlPartsContainingMeme = KwejkHtmlParser.GetMemeParts(memePage);

            foreach (var memePart in htmlPartsContainingMeme)
            {
                try
                {
                    memesFromSpecificPage.Add(new Meme()
                    {
                        Page = Meme.SourcePage.KWEJK,
                        Title = KwejkHtmlParser.GetTitleOfMeme(memePart),
                        ImagePath = KwejkHtmlParser.GetMemeImgPath(memePart),
                        Tags = KwejkHtmlParser.GetMemeTags(memePart)
                    });
                }
                catch(Exception e)
                {
                    SendMessage($"Exception catched: {e.Message} on page {url}");
                }
            }

            return memesFromSpecificPage;
        }

        private async Task<int> GetNewestPageNumber()
        {
            var mainPage = await _client.GetAsync(_baseAddress).Result.Content.ReadAsStringAsync();
            return int.Parse(KwejkHtmlParser.GetLastPageNumber(mainPage));
        }

        #region Debug 
        private void SendMessage(string message)
        {
            var messageSource = "KwejkMemeParser";
            _logger.LogDebug($"{messageSource}: {message}");
        }
        #endregion
    }
}
