using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _04_inverted_index
{
    public class InvertedIndex
    {
        //倒排索引
        private Dictionary<string, Dictionary<int, int>> _invertedIndex;
        //数据源
        private Dictionary<int, string> dataSource;

        public void OffLine()
        {
            dataSource = new Dictionary<int, string>();
            using (StreamReader sr = new StreamReader(@"D:\my_program\cs\Whut.MyWeb\04_inverted_index\dataSource\data.txt"))
            {
                string line;
                int num = 0;
                // 从文件读取并显示行，直到文件的末尾 
                while ((line = sr.ReadLine()) != null)
                {
                    dataSource.Add(++num, line);
                }
            }

            ConstructInvertedIndex(dataSource);
            GenerateInvertedIndexFile();
        }


        /// <summary>
        /// 生成倒排索引文件，保存在外存中
        /// </summary>
        private void GenerateInvertedIndexFile()
        {
            using (StreamWriter sw = new StreamWriter(@"D:\my_program\cs\Whut.MyWeb\04_inverted_index\dataSource\invertedIndex.txt"))
            {
                sw.WriteLine(dataSource.Count);//记录文档数据的条数，便于后期对齐处理
                foreach (var pair in _invertedIndex)
                {
                    StringBuilder line = new StringBuilder(pair.Key + "-" + pair.Value.Count + " ");//记录df
                    foreach (var item in pair.Value)
                    {
                        line.Append(item.Key + "-" + item.Value + ">");
                    }
                    sw.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// 构建倒排索引
        /// </summary>
        /// <param name="dataSource"></param>
        private void ConstructInvertedIndex(Dictionary<int, string> dataSource)
        {
            _invertedIndex = new Dictionary<string, Dictionary<int, int>>();
            foreach (var keyValue in dataSource)
            {
                //普通字符串匹配
                string[] words = keyValue.Value.ToLower().Split(new char[] { ' ', '.' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    int wordTF = words.Where(w => w.Equals(word)).Count();//统计TF
                    if (!_invertedIndex.ContainsKey(word))//倒排索引中不存在word
                    {
                        //记录出现在哪些句子中
                        Dictionary<int, int> posting = new Dictionary<int, int>();//文档id:单词在该文档出现的次数
                        posting.Add(keyValue.Key, wordTF);
                        _invertedIndex.Add(word, posting);
                    }
                    else//字典中有word
                    {
                        //word对应的文档id不存在的情况
                        if (!_invertedIndex[word].ContainsKey(keyValue.Key))
                        {
                            _invertedIndex[word].Add(keyValue.Key, wordTF);
                        }
                    }
                }
            }
        }
    }
}
