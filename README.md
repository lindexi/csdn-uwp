�����������򵥵�Ӧ�ã�ͳ��csdn���Ķ���

![����дͼƬ����](http://img.blog.csdn.net/20160604093904174)

����

![����дͼƬ����](http://img.blog.csdn.net/20160604095814013)

���벩��

![����дͼƬ����](http://img.blog.csdn.net/20160604095825685)

������ܼ򵥣���һ��List

��Ҫ����������

��������һ��`TextBox`�Ų��͵�ַ

```
            <TextBox Margin="10,10,10,10" Header="���͵�ַ" PlaceholderText="http://blog.csdn.net/lindexi_gd" Text="{x:Bind view.PostCsdn.Url,Mode=TwoWay}"></TextBox>

```

���ǻ���Ҫ��Post������ǵĲ���



```
    public class Post : NotifyProperty
    {
        public Post()
        {

        }

        /// <summary>
        ///     �Ķ���
        /// </summary>
        public string LinkView
        {
            set
            {
                _linkView = value;
                OnPropertyChanged();
            }
            get
            {
                return _linkView;
            }
        }

        public string Original
        {
            set
            {
                _original = value;
                OnPropertyChanged();
            }
            get
            {
                return _original;
            }
        }

        /// <summary>
        ///     ����
        /// </summary>
        public string Title
        {
            set;
            get;
        }

        /// <summary>
        ///     ����
        /// </summary>
        public string Url
        {
            set;
            get;
        }

        public string Time
        {
            set;
            get;
        }

        public DateTime LastTime
        {
            set;
            get;
        }

        public int AddView
        {
            set
            {
                _addView = value;
                OnPropertyChanged();
            }
            get
            {
                return _addView;
            }
        }

        public List<Scrutiny> ScrutinieCollection
        {
            set;
            get;
        }=new List<Scrutiny>();

        public void ClonePost(Post post)
        {
            int postLinkView = int.Parse(post.LinkView);
            int linkView = int.Parse(LinkView);
            if (postLinkView >= linkView)
            {
                AddView = postLinkView - linkView;
            }
            else
            {
                throw new FormatException();
            }

            LinkView = post.LinkView;
            Time = post.Time;
            LastTime = post.LastTime;
            ScrutinieCollection.Add(new Scrutiny()
            {
                LinkView = LinkView,
                Time = LastTime
            });
        }

        private int _addView;

        private string _linkView;
        private string _original;
    }
```

���csdn�ķ��������ֵȣ�������Ҫ�õ������������ڵ�ҳ����������ڵ�һҳ�ͻ�ȡ������ȡ���ҳ��������������ڲ��ǵ�һҳ�Ͳ���ȡ

```
        private void CompositionOriginal()
        {
            TotalPost = PostCollection.Count.ToString();
            //Original
            //var original = PostCollection.Where(temp => temp.Original == "ico ico_type_Original");
            var original = PostCollection.Where(temp => temp.Original == "ԭ");
            Original = original.Count().ToString();
            //Reprint
            //var reprint = PostCollection.Where(temp => temp.Original == "ico ico_type_Translated");
            var reprint = PostCollection.Where(temp => temp.Original == "ת");
            Reprint = reprint.Count().ToString();
            //Translation
            //var translation = PostCollection.Where(temp => temp.Original == "ico ico_type_Repost");
            var translation = PostCollection.Where(temp => temp.Original == "��");
            Translation = translation.Count().ToString();
        }

        private void ResponseCallBack(IAsyncResult result)
        {
            HttpWebRequest http = (HttpWebRequest) result.AsyncState;
            WebResponse webResponse = http.EndGetResponse(result);
            using (Stream stream = webResponse.GetResponseStream())
            {
                using (StreamReader read = new StreamReader(stream))
                {
                    string content = read.ReadToEnd();
                    try
                    {
                        UnEncoding(content);
                    }
                    catch (Exception e)
                    {
                        Debug.Write(e.Message);
                    }
                }
            }
        }

        private async void UnEncoding(string content)
        {
            //<div id="article_list" class="contents">

            //</div>\s+<!--��ʾ��ҳ-->

            //<div id="article_list" class="contents">[^[</div>\s+<!--��ʾ��ҳ-->]]+
            Regex regex;

            if (_maxPage == -1)
            {
                regex =
                    new Regex("<div id=\"papelist\" class=\"pagelist\">\\s{0,}<span>\\s{0,}\\d+��\\s{0,}��(\\d+)ҳ</span>");
                //<div id="papelist" class="pagelist">
                //<span> 109��  ��3ҳ</span>

                //<div id="papelist" class="pagelist">\s{0,}<span>\s{0,}\d+��\s{0,}��(\d+)ҳ</span>
                foreach (Match temp in regex.Matches(content))
                {
                    _maxPage = int.Parse(temp.Groups[1].Value);
                    break;
                }

                //<ul id="blog_rank">
                //<li>���ʣ�<span>81372��</span></li>
                //<li>���֣�<span>2098</span> </li> 

                //<ul id="blog_rank">\s{0,}<li>\s{0,}���ʣ�\s{0,}<span>(\d+)��</span>\s{0,}</li>
                //\s{0,}<li>\s{0,}���֣�\s{0,}<span>(\d+)</span>\s{0,}</li>
                regex = new Regex("<ul id=\"blog_rank\">\\s{0,}<li>\\s{0,}���ʣ�\\s{0,}<span>(\\d+)��</span>\\s{0,}</li>" +
                                  "\\s{0,}<li>\\s{0,}���֣�\\s{0,}<span>(\\d+)</span>\\s{0,}</li>");
                foreach (Match temp in regex.Matches(content))
                {
                    //_maxPage = int.Parse(temp.Groups[1].Value);
                    int total = int.Parse(temp.Groups[1].Value);
                    int integral = int.Parse(temp.Groups[2].Value);
                    Integral = integral.ToString();
                    if (!string.IsNullOrEmpty(TotalView) && TotalView != "0")
                    {
                        AddView = (total - int.Parse(TotalView)).ToString();
                    }
                    TotalView = total.ToString();
                }
            }

            regex = new Regex(@"<div id=""article_list"" class=""contents"">([\w|\W]+)</div>\s+<!--��ʾ��ҳ-->");
            //<div id="article_list" class="contents">([\w|\W]+)</div>\s+<!--��ʾ��ҳ-->
            var match = regex.Matches(content);
            foreach (Match temp in match)
            {
                content = temp.Groups[1].Value;
                break;
            }
            //<div class="list_item list_view">
            //<div class="article_title"> 
            //<span class="ico ico_type_Original"></span>
            //<h1>
            // <span class="link_title"><a href="/lindexi_gd/article/details/51344676">
            //C#��dll�����������
            //</a></span>
            //</h1>
            //</div>
            //<div class="article_manage">
            //<span class="link_postdate">2016-05-29 08:56</span>
            //<span class="link_view" title="�Ķ�����"><a href="/lindexi_gd/article/details/51344676" title="�Ķ�����">�Ķ�</a>(25)</span>

            //<div class="list_item list_view">\s{0,}<div class="article_title">\s{0,}
            //<span class="ico\s{0,}([\w|_]+)"></span>\s{0,}
            //<h1>\s{0,}<span class="link_title"><a href="/([\w|_]+)/article/details/(\d+)">\s{0,}
            //(.+)\s{0,}</a></span>
            //\s{0,}</h1>\s{0,}
            //</div>\s{0,}<div class="article_manage">\s{0,}<span class="link_postdate">(\d+-\d+-\d+ \d+:\d+)</span>\s{0,}
            //<span class="link_view" title="�Ķ�����"><a href="/[\w|_]+/article/details/\d+" title="�Ķ�����">�Ķ�</a>\((\d+)\)</span>
            regex =
                new Regex(
                    "<div class=\"list_item list_view\">\\s{0,}<div class=\"article_title\">\\s{0,}<span class=\"ico\\s{0,}([\\w|_]+)\"></span>\\s{0,}" +
                    "<h1>\\s{0,}<span class=\"link_title\"><a href=\"/([\\w|_]+)/article/details/(\\d+)\">\\s{0,}" +
                    "(.+)\\s{0,}</a></span>" +
                    "\\s{0,}</h1>\\s{0,}" +
                    "</div>\\s{0,}<div class=\"article_manage\">\\s{0,}<span class=\"link_postdate\">(\\d+-\\d+-\\d+ \\d+:\\d+)</span>\\s{0,}" +
                    "<span class=\"link_view\" title=\"�Ķ�����\"><a href=\"/[\\w|_]+/article/details/\\d+\" title=\"�Ķ�����\">�Ķ�</a>\\((\\d+)\\)</span>");
            match = regex.Matches(content);

            DateTime time = DateTime.Now;

            //List<Post> post = match.Cast<Match>().Select(temp => new Post()
            //{
            //    Original = temp.Groups[1].Value,
            //    Url = "/" + temp.Groups[2].Value + "/article/details/" + temp.Groups[3].Value,
            //    Title = temp.Groups[4].Value.Replace("\r","").Replace("\n","").Trim(),
            //    Time = temp.Groups[5].Value,
            //    LinkView = temp.Groups[6].Value,
            //    AddView = 0,
            //    LastTime = time
            //}).ToList();

            List<Post> post = new List<Post>();

            foreach (Match temp in match)
            {
                var original = temp.Groups[1].Value;
                var url = "/" + temp.Groups[2].Value + "/article/details/" + temp.Groups[3].Value;
                var title = temp.Groups[4].Value.Replace("\r", "").Replace("\n", "").Trim();
                var timePost = temp.Groups[5].Value;
                var linkView = temp.Groups[6].Value;

                if (original == "ico_type_Original")
                {
                    original = "ԭ";
                }
                else if (original == "ico_type_Translated")
                {
                    original = "��";
                }
                else
                {
                    original = "ת";
                }

                post.Add(new Post()
                {
                    Original = original,
                    Url = url,
                    Title = title,
                    Time = timePost,
                    LinkView = linkView,
                    AddView = 0,
                    LastTime = time
                });
            }

            //\s{0,}
            await DispatcherPost(post);

            if (_page == -1)
            {
                _page = 2;
            }
            else
            {
                _page++;
            }

            HttpGet();

            //HttpClient client = new HttpClient()
            //{
            //    Timeout = new TimeSpan(1000)
            //};
        }

        private async Task DispatcherPost(IEnumerable<Post> post)
        {
            //var postTitle=PostCollection.Where(temp=>temp.Title==post.Title)
            foreach (var temp in post)
            {
                await DispatcherPost(temp);
            }
            //await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            // {
            //     foreach (var temp in post)
            //     {
            //         PostCollection.Add(temp);
            //     }
            // });
        }

        private async Task DispatcherPost(Post post)
        {
            var postTitle = PostCollection.Where(temp => temp.Title == post.Title);
            if (postTitle.Any())
            {
                var temp = postTitle.First();
                temp.ClonePost(post);
            }
            else
            {
                //await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                //{
                //    PostCollection.Add(post);
                //});
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        PostCollection.Add(post);
                    });
            }
        }
```