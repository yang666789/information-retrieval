using System;
using System.IO;

namespace _04_inverted_index
{
    class Program
    {
        static void Main(string[] args)
        {
            InvertedIndex inverted = new InvertedIndex();
            inverted.OffLine();

            while (true)
            {
                Console.WriteLine("输入搜索关键字:");
                string line = Console.ReadLine().ToLower();
                string[] words = line.Split(new char[] { ' ', '.' }, StringSplitOptions.RemoveEmptyEntries);
                if ("exit".Equals(words[0]))
                {
                    break;
                }
                //if ("exit".Equals(line))
                //{
                //    break;
                //}
                //OnlineSearch(line);

                Search s = new Search();
                //s.PrintResult(words);
                s.Print(words);

                Console.WriteLine();
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
