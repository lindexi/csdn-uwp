// lindexi
// 10:56

using System;
using System.Collections.Generic;
using csdn.ViewModel;

namespace csdn
{
    /// <summary>
    ///     博文
    /// </summary>
    public class Post : NotifyProperty
    {
        public Post()
        {
        }

        public int N
        {
            set
            {
                _n = value;
                OnPropertyChanged();
            }
            get
            {
                return _n;
            }
        }

        /// <summary>
        ///     阅读数
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
        ///     文章
        /// </summary>
        public string Title
        {
            set;
            get;
        }

        /// <summary>
        ///     链接
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
        } = new List<Scrutiny>();

        public void ClonePost(Post post)
        {
            int postLinkView = int.Parse(post.LinkView);
            foreach (var temp in ScrutinieCollection)
            {
                if (temp.Time.Year == post.LastTime.Year &&
                    temp.Time.Month == post.LastTime.Month &&
                    temp.Time.Day == post.LastTime.Day - 1)
                {
                    postLinkView = int.Parse(temp.LinkView);
                    break;
                }
            }

            int linkView = int.Parse(LinkView);
            if (postLinkView >= linkView)
            {
                AddView = postLinkView - linkView;
            }
            else
            {
                //throw new FormatException();
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

        private int _n;
        private string _original;
    }
}