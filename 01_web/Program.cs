using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _01_web
{
    class Program
    {
        static Dictionary<string, string> dataSource = null;

        static void Main(string[] args)
        {
            dataSource = new Dictionary<string, string>();
            dataSource.Add("d1", "I like to watch the sun set with my friend.");
            dataSource.Add("d2", "The Best Places To Watch The Sunset.");
            dataSource.Add("d3", "My friend watch the sun come up.");

            while (true)
            {
                Console.WriteLine("输入搜索关键字:");
                string searchStr = Console.ReadLine();
                if ("exit".Equals(searchStr))
                {
                    break;
                }

                PrintResult(GetResult(searchStr, dataSource));
                Console.WriteLine();
            }

            Console.ReadKey(true);
        }

        static Dictionary<string, string> GetResult(string searcheStr, Dictionary<string, string> dataSource)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var keyValue in dataSource)
            {
                //法一：普通字符串匹配
                //string[] keys = keyValue.Value.Split(new char[] { ' ', '.' }, StringSplitOptions.RemoveEmptyEntries);
                //for (int i = 0; i < keys.Length; i++)
                //{
                //    if (keys[i].Equals(searcheStr, StringComparison.OrdinalIgnoreCase))
                //    {
                //        result.Add(keyValue.Key, keyValue.Value);
                //        break;
                //    }
                //}

                //法二：正则匹配：单词边界匹配且忽略大小写（注意转义的情况：要么都加@，要么双斜杠）
                string patter = @"(?i)\b" + searcheStr + @"\b";
                if (Regex.Match(keyValue.Value, patter).Success)
                {
                    result.Add(keyValue.Key, keyValue.Value);
                }
            }
            return result;
        }

        static void PrintResult(Dictionary<string, string> result)
        {
            if (result.Count == 0)
                Console.WriteLine("未匹配到");
            else
            {
                Console.WriteLine("匹配结果：");
                foreach (var item in result)
                {
                    Console.WriteLine(item.Key + " = " + item.Value);
                }
            }
        }
    }
}
