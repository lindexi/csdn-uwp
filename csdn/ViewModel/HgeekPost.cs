using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace csdn.ViewModel
{
    class HgeekPost : NotifyProperty
    {
        /// <summary>
        /// 跳转到博客
        /// </summary>
        public void NagivatePost()
        {
            Frame.Navigate(typeof(View.CsdnProgressPage));
        }

        public void NagivateGeek()
        {
            Frame.Navigate(typeof(View.ConeHtCsdnPage));
        }

        public Frame Frame
        {
            set;
            get;
        }
    }
}
