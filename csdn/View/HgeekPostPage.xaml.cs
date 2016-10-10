// lindexi
// 15:46

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
using csdn.ViewModel;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace csdn.View
{
    /// <summary>
    ///     汉堡菜单导航
    /// </summary>
    public sealed partial class HgeekPostPage : Page
    {
        public HgeekPostPage()
        {
            View = new HgeekPost();
            this.InitializeComponent();
            View.Frame = frame;
            View.NagivatePost();
        }

        private HgeekPost View
        {
            set;
            get;
        }
    }
}