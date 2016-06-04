// lindexi
// 9:56

namespace csdn
{
    public class ViewModel : NotifyProperty
    {
        public ViewModel()
        {
            PostCsdn = new Neti();
        }

        public Neti PostCsdn
        {
            set;
            get;
        }

        /// <summary>
        ///     用户博客
        /// </summary>
        public string Url
        {
            set
            {
                _url = value;
                OnPropertyChanged();
            }
            get
            {
                return _url;
            }
        }

        private string _url;
    }
}