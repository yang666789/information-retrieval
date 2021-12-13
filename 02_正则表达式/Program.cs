using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Text.RegularExpressions;

namespace _02_正则表达式
{
    class Program
    {
        static void Main(string[] args)
        {
            //WriteLine(Regex.Match("yes,please", @"\p{P}").Value);//匹配任意标点符号
            //WriteLine(Regex.Match("您的血压为100/160", @"\d{2,3}/\d{2,3}").Value);//提取血压数据

            //贪婪量词符号和懒惰量词符号，默认贪婪，加上?为懒惰
            //string html = "<i>你长得</i>真真真真<i>好看</i>";
            //foreach (Match match in Regex.Matches(html, @"<i>.*?</i>"))
            //{
            //    WriteLine(match.Value);
            //}

            //正前向条件校验密码强度(至少含有六个字符，且其中至少包含一个数字)
            //string pwd = "1iuyth";
            //WriteLine(Regex.Match(pwd, @"(?=.*\d).{6,}").Success);

            //解析Xml/Html标签
            string patter = @"<(?'tag'\w+?).*>" + //第一个括号内最后的?为懒加载，'tag'后面\w+表示标签为多个字母构成
                @"(?'text'.*?)" + //懒加载，'text'后面表示标签中的文本由0或多个符号组成
                @"</\k'tag'>"; //捕获上面的tag
            string testHtml = "<msg>杨毅</msg>";
            Match m = Regex.Match(testHtml, patter);
            WriteLine(m.Groups["tag"]);
            WriteLine(m.Groups["text"]);

            ReadKey(true);
        }
    }
}
