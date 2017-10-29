using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using csdn.Model;
using csdn.ViewModel;
using lindexi.uwp.Framework.ViewModel;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace csdn.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    [ViewModel(typeof(LicyvgAstgzModel))]
    public sealed partial class ConeHtCsdnPage : Page
    {
        public ConeHtCsdnPage()
        {
            this.InitializeComponent();

            var csdn = new LogCsdn(GeekWebView, AppId.Account);
            csdn.Signed += SnkmvxjWfmf;
            csdn.Sign();
        }

        private void SnkmvxjWfmf(object sender, EventArgs e)
        {
            //登陆成功，进入 feed 
            string url = "http://feed.csdn.net/";
            GeekWebView.Navigate(new Uri(url));
        }

        public LicyvgAstgzModel ViewModel { get; set; } = new LicyvgAstgzModel();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (LicyvgAstgzModel) e.Parameter;
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }
    }
}
