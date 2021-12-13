using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using HtmlAgilityPack;

namespace _03_spider
{
    class Program
    {
        static void Main(string[] args)
        {
            //1.获取网页数据
            string mainUrl = "https://www.1905.com/mdb/film/hot/list/o1p1.html";
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(mainUrl);

            //2.解析网页数据，获取链接
            HtmlNode hotfilmNode = doc.DocumentNode.SelectSingleNode("//ul[@class='hotfilm-list']");
            HtmlNodeCollection filmsNodes = hotfilmNode.SelectNodes("//li[@class='clearFloat']");
            Dictionary<string, string> filmInfo = new Dictionary<string, string>();
            foreach (var filmNode in filmsNodes)
            {
                HtmlNode node = filmNode.SelectSingleNode("./a");
                filmInfo.Add(node.Attributes["title"].Value, "https://www.1905.com" + node.Attributes["href"].Value);
            }

            //3.爬取每个链接的数据
            //单线程爬取
            //foreach (var pair in filmInfo)
            //{
            //    Console.WriteLine(pair.Key + " : " + web.Load(pair.Value).DocumentNode.SelectSingleNode("//section[@class='mod']/div/p").InnerText);
            //    Console.WriteLine();
            //}

            //多线程爬取
            foreach (var pair in filmInfo)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(getFilmInfo));
                thread.Start(pair);
            }

            Console.ReadLine();
        }


        static void getFilmInfo(object o)
        {
            KeyValuePair<string, string> pair = (KeyValuePair<string, string>)o;
            HtmlWeb web = new HtmlWeb();
            Console.WriteLine(pair.Key + " : " + web.Load(pair.Value).DocumentNode.SelectSingleNode("//section[@class='mod']/div/p").InnerText);
            Console.WriteLine();
        }
    }
}
