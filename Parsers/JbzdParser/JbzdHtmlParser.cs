using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HtmlAgilityPack;

namespace memegeumApp.Parsers.JbzdParser
{
    public static class JbzdHtmlParser 
    {

        internal static List<string> GetMemeParts(string memePage)
        {
            var document = new HtmlDocument();
            document.LoadHtml(memePage);

            return document.DocumentNode.Descendants("article")
                                        .Where(node => node.Attributes.Contains("data-id"))
                                        .Select(node => node.InnerHtml)
                                        .ToList();
        }

        internal static string GetTitleOfMeme(string memePart)
        {
            var document = new HtmlDocument();
            document.LoadHtml(memePart);

            return document.DocumentNode.Descendants("div")
                                        .First(node => node.GetAttributeValue("class", "").Equals("title"))
                                        .SelectSingleNode("a")
                                        .InnerText.Trim();
        }

        internal static string GetMemeImgPath(string memePart)
        {
            var document = new HtmlDocument();
            document.LoadHtml(memePart);

            try
            {
                return document.DocumentNode.Descendants("img")
                                            .First(node => node.GetAttributeValue("class", "").Equals("resource-image"))
                                            .GetAttributeValue("src", "");
            }
            catch(Exception e)
            {
                Debug.WriteLine($"Błąd: {e.Message} podczas pobierania url obrazka w: \n{memePart}");
                return null;
            }
        }

        internal static List<string> GetMemeTags(string memePart)
        {
            var document = new HtmlDocument();
            document.LoadHtml(memePart);

            return document.DocumentNode.Descendants("a")
                                        .Where(node => node.Attributes.Contains("data-tag"))
                                        .Select(node => node.GetAttributeValue("data-tag", ""))
                                        .ToList();
        }
    }
}
