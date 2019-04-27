using HtmlAgilityPack;
using System.Net.Http;

namespace consoleApp
{
    //public class DictionaryComRetriever : IRetriever
    //{
    //    private readonly HttpClient client = new HttpClient();

    //    public Word Retrieve(string word)
    //    {

    //        string url = $"https://www.dictionary.com/browse/{word}";

    //        var web = new HtmlWeb();
    //        var htmlDoc = web.Load(url);
    //        //var node = htmlDoc.DocumentNode.SelectSingleNode("//main/section/section//div[@class='default-content']/..");
    //        //var node = htmlDoc.DocumentNode.SelectSingleNode("//main/section/section//div[@class='default-content']/div[@value='1']/span");


    //        var nodes = htmlDoc.DocumentNode.SelectNodes("//main//div[@value='1']/span");

    //        HtmlNode theNode = null;
    //        foreach(var node in nodes)
    //        {
    //            if (!string.IsNullOrWhiteSpace(node.InnerHtml))
    //            {
    //                theNode = node;
    //                break;
    //            }
    //        }

    //        var result = new Word()
    //        {
    //            WordItself = word,
    //            Definition = theNode?.InnerHtml
    //        };

    //        return result;
    //    }

    //    public string RetrieveDefinition(string word)
    //    {
    //        string url = $"https://www.dictionary.com/browse/{word}";

    //        var web = new HtmlWeb();
    //        var htmlDoc = web.Load(url);
    //        //var node = htmlDoc.DocumentNode.SelectSingleNode("//main/section/section//div[@class='default-content']/..");
    //        //var node = htmlDoc.DocumentNode.SelectSingleNode("//main/section/section//div[@class='default-content']/div[@value='1']/span");


    //        var nodes = htmlDoc.DocumentNode.SelectNodes("//main//div[@value='1']/span");

    //        HtmlNode theNode = null;
    //        foreach (var node in nodes)
    //        {
    //            if (!string.IsNullOrWhiteSpace(node.InnerHtml))
    //            {
    //                theNode = node;
    //                break;
    //            }
    //        }

    //        return theNode?.InnerHtml;
    //    }
    //}
}
