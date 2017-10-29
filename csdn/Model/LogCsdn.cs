using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace csdn.Model
{
    /// <summary>
    /// µÇÂ½csdn
    /// </summary>
    public class LogCsdn
    {
        public LogCsdn(WebView webView, Account account)
        {
            WebView = webView;
            Account = account;
        }

        /// <summary>
        /// µÇÂ½
        /// </summary>
        public void Sign()
        {
            //Ìí¼Ó cookie
            var filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            var cookie = new Windows.Web.Http.HttpCookie("cookieName", "csdn.net", "/")
            {
                Value = "cookieValue"
            };
            filter.CookieManager.SetCookie(cookie, false);

            WebView.Navigate(new Uri(Url));
            WebView.NavigationCompleted += Post_NavigationCompleted;
        }

        public event EventHandler Signed;

        private async void Post_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            WebView.NavigationCompleted -= Post_NavigationCompleted;

            string str = await WebView.InvokeScriptAsync("eval", new string[] { "document.getElementsByName('lt')[0].value;" });
            string execution = await WebView.InvokeScriptAsync("eval", new string[] { "document.getElementsByName('execution')[0].value;" });
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(Url));

            var httpContent = new HttpFormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("username",Account.UserName),
                new KeyValuePair<string, string>("password",Account.Password),
                new KeyValuePair<string, string>("lt",str),
                new KeyValuePair<string, string>("execution",execution),
                new KeyValuePair<string, string>("_eventId","submit"),
            });

            httpRequestMessage.Content = httpContent;
            WebView.NavigateWithHttpRequestMessage(httpRequestMessage);

            WebView.NavigationCompleted += PimzlLxhtr_NavigationCompleted;
        }

        private void PimzlLxhtr_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            WebView.NavigationCompleted -= PimzlLxhtr_NavigationCompleted;
            Signed?.Invoke(sender, null);
        }

        /// <summary>
        /// ÕËºÅ
        /// </summary>
        private Account Account { get; set; }

        /// <summary>
        /// µÇÂ½csdnÁ¬½Ó
        /// </summary>
        private string Url { get; } = "http://passport.csdn.net/";

        private WebView WebView { get; }
    }
}