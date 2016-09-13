// lindexi
// 10:57

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.Web.Http.Filters;
using Newtonsoft.Json;

namespace csdn
{
    /// <summary>
    ///     个人博客
    /// </summary>
    public class Neti : NotifyProperty
    {
        public Neti()
        {
            string i = 0.ToString();
            Original = i;
            Reprint = i;
            Translation = i;
            AddView = i;
            TotalPost = i;
            TotalView = i;
            Integral = i;

            _object = new object();
            _intervalTime = 36;



            if (NetworkInformation.GetInternetConnectionProfile()?.IsWlanConnectionProfile == true)
            {
                new Task(IntervalSwoop).Start();
            }
        }

        /// <summary>
        ///     原创
        /// </summary>
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
        ///     转载
        /// </summary>
        public string Reprint
        {
            set
            {
                _reprint = value;
                OnPropertyChanged();
            }
            get
            {
                return _reprint;
            }
        }

        /// <summary>
        ///     翻译
        /// </summary>
        public string Translation
        {
            set
            {
                _translation = value;
                OnPropertyChanged();
            }
            get
            {
                return _translation;
            }
        }

        /// <summary>
        ///     文章访问增加
        /// </summary>
        public string AddView
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

        /// <summary>
        ///     文章
        /// </summary>
        public string TotalPost
        {
            set
            {
                _totalPost = value;
                OnPropertyChanged();
            }
            get
            {
                return _totalPost;
            }
        }


        /// <summary>
        ///     链接
        /// </summary>
        public string Url
        {
            set;
            get;
        }

        public ObservableCollection<Post> PostCollection
        {
            set;
            get;
        } = new ObservableCollection<Post>();

        public string TotalView
        {
            set
            {
                _totalView = value;
                OnPropertyChanged();
            }
            get
            {
                return _totalView;
            }
        }

        public string Integral
        {
            set
            {
                _integral = value;
                OnPropertyChanged();
            }
            get
            {
                return _integral;
            }
        }


        /// <summary>
        ///     抓取
        /// </summary>
        public void Swoop()
        {
#if DEBUG
            if (string.IsNullOrEmpty(Url))
            {
                Url = "http://blog.csdn.net/lindexi_gd";
            }
#endif
            _maxPage = -1;
            _page = -1;
            HttpGet();

            //ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
            //if (profile?.IsWlanConnectionProfile==true)
            //{
            //    View();
            //}
            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop")
            {
                View(0);
            }
            else if(NetworkInformation.GetInternetConnectionProfile()?.IsWlanConnectionProfile == true)
            {
                View(0);
            }

        }

        public void View()
        {
            //View(0);
            string url = "http://blog.csdn.net/lindexi_gd/article/details/51683015";
            View(url);
        }

        /// <summary>
        ///     阅读排序
        /// </summary>
        public void SortView()
        {
            SortDispatcher((a, b) =>
            {
                int linkViewa = int.Parse(a.LinkView);
                int linkViewb = int.Parse(b.LinkView);
                return linkViewa.CompareTo(linkViewb)*-1;
            });
        }

        /// <summary>
        ///     增加排序
        /// </summary>
        public void SortAdd()
        {
            SortDispatcher((a, b) => a.AddView.CompareTo(b.AddView)*-1);
        }

        /// <summary>
        ///     发布时间
        /// </summary>
        public void SortTime()
        {
            SortDispatcher((a, b) =>
            {
                DateTime timea = DateTime.Parse(a.Time);
                DateTime timeb = DateTime.Parse(b.Time);
                return timea.CompareTo(timeb)*-1;
            });
        }

        private string _addView;
        private string _integral;


        private long _intervalTime;

        private int _maxPage;

        private object _object;

        private string _original;
        private int _page;

        private Random _ran = new Random();
        private string _reprint;
        private string _totalPost;
        private string _totalView;
        private string _translation;

        private async void View(int i)
        {
            //函数可以刷访问，可以测试我们软件，你可以使用函数来刷博客比排名1多
            string url = "http://blog.csdn.net";
            if (PostCollection.Count == 0)
            {
                await Task.Delay(1000);
                View(i);
                return;
            }
            if (i >= PostCollection.Count)
            {
                return;
            }

            var temp = PostCollection[i];
            url = url + temp.Url;
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
            filter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;
            request.BeginGetResponse(result =>
            {
                HttpWebRequest http = (HttpWebRequest) result.AsyncState;
                WebResponse webResponse = http.EndGetResponse(result);
                webResponse.Dispose();
                i++;
                View(i);
            }, request);
        }

        private void View(string url)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "GET";
                HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
                filter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;
                request.BeginGetResponse(result =>
                {
                    HttpWebRequest http = (HttpWebRequest) result.AsyncState;
                    WebResponse webResponse = http.EndGetResponse(result);
                    webResponse.GetResponseStream().ReadByte();
                    webResponse.Dispose();
                    Task.Delay(_ran.Next(1000, 10000)).Wait();
                    View(url);
                }, request);
            }
            catch
            {
                Task.Delay(_ran.Next(1000, 10000)).Wait();
                View(url);
            }
        }

        /// <summary>
        ///     间隔爬博客
        /// </summary>
        private void IntervalSwoop()
        {
            while (true)
            {
                Task.Delay(100000).Wait();
                if (_object == null)
                {
                    _object = new object();
                }
                lock (_object)
                {
                    _intervalTime--;
                }
                if (_intervalTime == 0)
                {
                    _intervalTime = 36;
                    Swoop();
                }
                if (_intervalTime < 0)
                {
                    return;
                }
            }
        }

        private void SortDispatcher(Comparison<Post> compariso)
        {
            new Task(async () =>
            {
                var postSort = PostCollection.ToList();
                postSort.Sort(compariso);

                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        int n = 0;
                        PostCollection.Clear();
                        foreach (var temp in postSort)
                        {
                            n++;
                            temp.N = n;
                            PostCollection.Add(temp);
                        }
                    });
            }).Start();
        }

        private void HttpGet()
        {
            //获取url
            try
            {
                string url = Url;
                if (_page == -1)
                {
                    url += "?viewmode=contents";
                }
                else if (_page > _maxPage)
                {
                    CompositionOriginal();
                    return;
                }
                else
                {
                    url += $"/article/list/{_page}" + "?viewmode=contents";
                }
                WebRequest request = WebRequest.Create(url);
                request.Method = "GET";
                request.BeginGetResponse(ResponseCallBack, request);
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
        }

        private void CompositionOriginal()
        {
            TotalPost = PostCollection.Count.ToString();
            //Original
            //var original = PostCollection.Where(temp => temp.Original == "ico ico_type_Original");
            var original = PostCollection.Where(temp => temp.Original == "原");
            Original = original.Count().ToString();
            //Reprint
            //var reprint = PostCollection.Where(temp => temp.Original == "ico ico_type_Translated");
            var reprint = PostCollection.Where(temp => temp.Original == "转");
            Reprint = reprint.Count().ToString();
            //Translation
            //var translation = PostCollection.Where(temp => temp.Original == "ico ico_type_Repost");
            var translation = PostCollection.Where(temp => temp.Original == "翻");
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

            //</div>\s+<!--显示分页-->

            //<div id="article_list" class="contents">[^[</div>\s+<!--显示分页-->]]+
            Regex regex;

            if (_maxPage == -1)
            {
                regex =
                    new Regex("<div id=\"papelist\" class=\"pagelist\">\\s{0,}<span>\\s{0,}\\d+条\\s{0,}共(\\d+)页</span>");
                //<div id="papelist" class="pagelist">
                //<span> 109条  共3页</span>

                //<div id="papelist" class="pagelist">\s{0,}<span>\s{0,}\d+条\s{0,}共(\d+)页</span>
                foreach (Match temp in regex.Matches(content))
                {
                    _maxPage = int.Parse(temp.Groups[1].Value);
                    break;
                }

                //<ul id="blog_rank">
                //<li>访问：<span>81372次</span></li>
                //<li>积分：<span>2098</span> </li> 

                //<ul id="blog_rank">\s{0,}<li>\s{0,}访问：\s{0,}<span>(\d+)次</span>\s{0,}</li>
                //\s{0,}<li>\s{0,}积分：\s{0,}<span>(\d+)</span>\s{0,}</li>
                regex = new Regex("<ul id=\"blog_rank\">\\s{0,}<li>\\s{0,}访问：\\s{0,}<span>(\\d+)次</span>\\s{0,}</li>" +
                                  "\\s{0,}<li>\\s{0,}积分：\\s{0,}<span>(\\d+)</span>\\s{0,}</li>");
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

            regex = new Regex(@"<div id=""article_list"" class=""contents"">([\w|\W]+)</div>\s+<!--显示分页-->");
            //<div id="article_list" class="contents">([\w|\W]+)</div>\s+<!--显示分页-->
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
            //C#将dll打包到程序中
            //</a></span>
            //</h1>
            //</div>
            //<div class="article_manage">
            //<span class="link_postdate">2016-05-29 08:56</span>
            //<span class="link_view" title="阅读次数"><a href="/lindexi_gd/article/details/51344676" title="阅读次数">阅读</a>(25)</span>

            //<div class="list_item list_view">\s{0,}<div class="article_title">\s{0,}
            //<span class="ico\s{0,}([\w|_]+)"></span>\s{0,}
            //<h1>\s{0,}<span class="link_title"><a href="/([\w|_]+)/article/details/(\d+)">\s{0,}
            //(.+)\s{0,}</a></span>
            //\s{0,}</h1>\s{0,}
            //</div>\s{0,}<div class="article_manage">\s{0,}<span class="link_postdate">(\d+-\d+-\d+ \d+:\d+)</span>\s{0,}
            //<span class="link_view" title="阅读次数"><a href="/[\w|_]+/article/details/\d+" title="阅读次数">阅读</a>\((\d+)\)</span>
            regex =
                new Regex(
                    "<div class=\"list_item list_view\">\\s{0,}<div class=\"article_title\">\\s{0,}<span class=\"ico\\s{0,}([\\w|_]+)\"></span>\\s{0,}" +
                    "<h1>\\s{0,}<span class=\"link_title\"><a href=\"/([\\w|_]+)/article/details/(\\d+)\">\\s{0,}" +
                    "(.+)\\s{0,}</a></span>" +
                    "\\s{0,}</h1>\\s{0,}" +
                    "</div>\\s{0,}<div class=\"article_manage\">\\s{0,}<span class=\"link_postdate\">(\\d+-\\d+-\\d+ \\d+:\\d+)</span>\\s{0,}" +
                    "<span class=\"link_view\" title=\"阅读次数\"><a href=\"/[\\w|_]+/article/details/\\d+\" title=\"阅读次数\">阅读</a>\\((\\d+)\\)</span>");
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
                    original = "原";
                }
                else if (original == "ico_type_Translated")
                {
                    original = "翻";
                }
                else
                {
                    original = "转";
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
    }
}