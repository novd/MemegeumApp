using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace memegeumApp.Parsers.KwejkParser
{
    public static class KwejkHtmlParser
    {
        internal static List<string> GetMemeParts(string memePage)
        {
            var document = new HtmlDocument();
            document.LoadHtml(memePage);

            return document.DocumentNode.Descendants("div")
                                        .Where(node => node.GetAttributeValue("class", "").Contains("media-element-wrapper"))
                                        .Select(node => node.InnerHtml)
                                        .ToList();
        }

        internal static string GetTitleOfMeme(string memePart)
        {
            var document = new HtmlDocument();
            document.LoadHtml(memePart);

            return document.DocumentNode.Descendants("a")
                                        .Where(node => node.GetAttributeValue("dusk", "").Contains("media-title-selector"))
                                        .Select(node => node.InnerText)
                                        .First();
        }

        internal static string GetMemeImgPath(string memePart)
        {
            var document = new HtmlDocument();
            document.LoadHtml(memePart);
            if (!IsMemeVideo(memePart))
            {

                return document.DocumentNode.Descendants("img")
                                            .Where(node => node.GetAttributeValue("class", "").Contains("full-image"))
                                            .Select(node => node.GetAttributeValue("src", ""))
                                            .First();

            }

            throw new ArgumentException("This meme is a video");

        }

        internal static List<string> GetMemeTags(string memePart)
        {
            var document = new HtmlDocument();
            document.LoadHtml(memePart);

            return document.DocumentNode.Descendants("div")
                                        .First(node => node.GetAttributeValue("class", "").Contains("tag-list"))
                                        .Descendants("a")
                                        .Select(node => node.InnerText.Replace("#", ""))
                                        .ToList();
        }

        internal static string GetLastPageNumber(string mainPage)
        {
            var document = new HtmlDocument();
            document.LoadHtml(mainPage);

            return document.DocumentNode.SelectSingleNode("//li[@class='current']")
                                        .SelectSingleNode("a")
                                        .InnerText;
        }


        private static bool IsMemeVideo(string memePart)
        {
            var document = new HtmlDocument();
            document.LoadHtml(memePart);
            return document.DocumentNode.Descendants("player").ToList().Count > 0;
        }
    }
}
