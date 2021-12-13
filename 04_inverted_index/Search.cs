using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_inverted_index
{
    public class Search
    {
        private string[] _InvertedIndex { get; set; }

        public Search()
        {
            GetInvertedIndex();
        }

        /// <summary>
        /// 将倒排索引文件读入到内存中
        /// </summary>
        /// <returns></returns>
        private void GetInvertedIndex()
        {
            string content = null;
            using (StreamReader sr = new StreamReader(@"D:\my_program\cs\Whut.MyWeb\04_inverted_index\dataSource\invertedIndex.txt"))
            {
                content = sr.ReadToEnd();
            }
            _InvertedIndex = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        #region 实验二单个关键词在线搜索
        public void OnlineSearch(string searchStr)
        {
            bool isExisted = false;
            using (StreamReader sr = new StreamReader(@"D:\my_program\cs\Whut.MyWeb\04_inverted_index\dataSource\invertedIndex.txt"))
            {
                string line;
                // 从文件读取并显示行，直到文件的末尾 
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split(new char[] { ' ', '>' }, StringSplitOptions.RemoveEmptyEntries);
                    if (searchStr.Equals(words[0].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[0]))
                    {
                        isExisted = true;
                        for (int i = 1; i < words.Length; i++)
                        {
                            Console.Write(words[i] + " ");
                        }
                        break;
                    }
                }
            }
            if (!isExisted)
                Console.WriteLine("不存在含有该词的文档");
        }
        #endregion

        #region 交集求多个查询词的公共文档
        public void PrintResult(string[] words)
        {
            List<int> result;

            if (words.Length == 1)
            {
                result = GetDocId(words[0]);
            }
            else
            {
                List<List<int>> list = new List<List<int>>();
                foreach (var word in words)
                {
                    list.Add(GetDocId(word));
                }
                result = GetAllIntersection(list);
            }
            foreach (var item in result)
            {
                Console.Write(item + " ");
            }
        }

        public List<int> GetBothIntersection(List<int> l1, List<int> l2)
        {
            //求两个集合的交集  
            //hashset的就是专门求两个交集而生的
            //如果你存在了这个记录，他就会返回false 这里就是利用了这个思路
            HashSet<int> s = new HashSet<int>();
            HashSet<int> answer = new HashSet<int>();
            foreach (var item in l1)
            {
                s.Add(item);
            }
            foreach (var item in l2)
            {
                if (s.Add(item) == false)
                {
                    answer.Add(item);
                }
            }
            return new List<int>(answer);
        }

        public List<int> GetAllIntersection(List<List<int>> docsId)
        {
            List<int> result = GetBothIntersection(docsId[0], docsId[1]);
            for (int i = 2; i < docsId.Count; i++)
            {
                result = GetBothIntersection(result, docsId[i]);
            }
            return result;
        }

        public List<int> GetDocId(string word)
        {
            List<int> docId = new List<int>();
            foreach (var line in _InvertedIndex)
            {
                string[] words = line.Split(new char[] { ' ', '>' }, StringSplitOptions.RemoveEmptyEntries);
                if (word.Equals(words[0]))
                {
                    for (int i = 1; i < words.Length; i++)
                    {
                        docId.Add(Convert.ToInt32(words[i]));
                    }
                    break;
                }
            }
            return docId;
        }
        #endregion

        #region TFIDF按相关性打分排序得到相关文档
        /// <summary>
        /// 获取词对应出现文档的信息（文档id:该词在该文档中出现的次数tf）
        /// </summary>
        /// <param name="word">待查词</param>
        /// <param name="appearNum">该词出现的文档次数df</param>
        /// <returns>文档id:该词在该文档中出现的次数tf</returns>
        private Dictionary<int, int> GetDocId_TF(string word, out double appearNum)
        {
            appearNum = 0;
            Dictionary<int, int> docId_tfs = new Dictionary<int, int>();

            for (int i = 1; i < _InvertedIndex.Length; i++)
            {
                string[] items = _InvertedIndex[i].Split(new char[] { ' ', '>' }, StringSplitOptions.RemoveEmptyEntries);
                var key = items[0].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                appearNum = Convert.ToInt32(key[1]);
                if (word.Equals(key[0]))
                {
                    for (int j = 1; j < items.Length; j++)
                    {
                        var docId_tf = items[j].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                        docId_tfs.Add(Convert.ToInt32(docId_tf[0]), Convert.ToInt32(docId_tf[1]));
                    }
                    break;
                }
            }
            return docId_tfs;
        }

        /// <summary>
        /// 得到对应word在出现的每个文档中的tf.idf列表
        /// </summary>
        /// <param name="word">待查词</param>
        /// <returns>文档id:待查词在该文档中的weight（即tf.idf）</returns>
        private Dictionary<int, double> GetTF_IDF(string word)
        {
            Dictionary<int, double> tf_idfs = new Dictionary<int, double>();
            double weight = 0;
            double df;
            Dictionary<int, int> docId_tfs = GetDocId_TF(word, out df);//文档id:tf
            foreach (var keyValue in docId_tfs)
            {
                double tmp_tf = Math.Log(1 + keyValue.Value);
                //df也要是浮点型，要不然整数除以整数还是整数
                //这里的分子要取大点，否则因为停用词比如a,an,of,the这些词df太大，以至于括号内无线接近于0，最后结果为负无穷
                double tmp_df = Math.Log10(100 / df);
                weight = tmp_tf * tmp_df;
                tf_idfs.Add(keyValue.Key, weight);
            }
            return tf_idfs;
        }

        /// <summary>
        /// 按照对应出现文档各查询词累加总评分
        /// </summary>
        /// <param name="words">查询词</param>
        /// <returns>文档id：查询词在对应文档累加后的总权值</returns>
        private Dictionary<int, double> GetScore(string[] words)
        {
            //生成一个初始化字典是为了计算总分数的时候对齐，以方便计算
            Dictionary<int, double> initial = new Dictionary<int, double>();
            for (int i = 1; i <= Convert.ToInt32(_InvertedIndex[0]); i++)
            {
                initial.Add(i, 0);
            }

            Dictionary<int, double> scoreResult = new Dictionary<int, double>();//文档id:总分数
            foreach (var pair in initial)
            {
                double score = 0;
                for (int i = 0; i < words.Length; i++)
                {
                    if (GetTF_IDF(words[i]).ContainsKey(pair.Key))
                    {
                        //注意加法是+=，而不是=
                        //别犯低级错误
                        score += GetTF_IDF(words[i])[pair.Key];
                    }
                }
                scoreResult.Add(pair.Key, score);
            }
            return scoreResult;
        }

        /// <summary>
        /// 字典内部方法根据值进行排序
        /// </summary>
        /// <param name="rawDic">未排序但已计算好总权值的字典</param>
        /// <returns></returns>
        private Dictionary<int, double> GetSortedResult(Dictionary<int, double> rawDic)
        {
            return rawDic.OrderByDescending(item => item.Value)
                .ToDictionary(item => item.Key, item => item.Value);
        }

        public void Print(string[] words)
        {
            Dictionary<int, double> result = GetSortedResult(GetScore(words));

            foreach (var item in result)
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }
        }
        #endregion
    }
}
