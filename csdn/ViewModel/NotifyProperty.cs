// lindexi
// 22:16

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace csdn.ViewModel
{
    /// <summary>
    ///     提供继承通知UI改变值
    /// </summary>
    public class NotifyProperty : INotifyPropertyChanged
    {
        /// <summary>
        ///     一直添加value
        /// </summary>
        public string Reminder
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _reminder.Clear();
                }
                else
                {
                    _reminder.Append(value + "\r\n");
                }
                OnPropertyChanged("reminder");
            }
            get
            {
                return _reminder.ToString();
            }
        }

        public void UpdateProper<T>(ref T properValue, T newValue, [CallerMemberName] string properName = "")
        {
            if (Equals(properValue, newValue))
            {
                return;
            }

            properValue = newValue;

            if (properName != null)
            {
                OnPropertyChanged(properName);
            }
        }

        public async void OnPropertyChanged([CallerMemberName] string name = "")
        {
            var handler = PropertyChanged;

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                   handler?.Invoke(this, new PropertyChangedEventArgs(name));
               });

            //await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    handler?.Invoke(this, new PropertyChangedEventArgs(name));
            //});
        }


        private StringBuilder _reminder = new StringBuilder();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}