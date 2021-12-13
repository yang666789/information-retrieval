# information-retrieval
基于倒排索引和 TF-IDF 算法实现的简单信息检索控制台程序

### 目的:
输入多个查询词，按照相关性排序输出数据源中各个句子的编号。

### 数据源:
d1：I like to watch the sun set with my friend.<br/>
d2：The Best Places To Watch The Sunset.<br/>
d3：My friend watch the sun come up.

### 运行示例：
![image](https://user-images.githubusercontent.com/54838327/145751527-8720c3c6-2769-4fc5-b559-a1f8b5d2ee44.png)
<br/>
说明：<br/>
输入"like to watch"，根据相关性进行排序输出句子的id，以及根据算法生成的总权值，输出结果表明句子1与搜索词最相关；<br/>
输入"a"，因为数据源中含有这个单词，所以总权值都为0；<br/>
输入一个词"the"，按照相关性输出，观察数据集符合结果。
