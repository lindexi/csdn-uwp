// lindexi
// 15:34

using System;
using System.IO;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI;
using Newtonsoft.Json;

namespace csdn
{
    public class ViewModel : NotifyProperty
    {
        public ViewModel()
        {
            App.Current.Suspending += OnSuspending;
            Read();
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

        private async void Read()
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync("data");
                var json = JsonSerializer.Create();
                using (TextReader stream = new StreamReader(await file.OpenStreamForReadAsync()))
                {
                    PostCsdn = json.Deserialize<Neti>(new JsonTextReader(stream));
                }
            }
            catch (Exception)
            {
                PostCsdn = new Neti();
            }
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            var json = JsonSerializer.Create();
            //json.Serialize();
            var file =
                await
                    ApplicationData.Current.LocalFolder.CreateFileAsync("data", CreationCollisionOption.ReplaceExisting);
            using (TextWriter stream = new StreamWriter(await file.OpenStreamForWriteAsync()))
            {
                json.Serialize(stream, PostCsdn);
            }
            deferral.Complete();
        }
    }
}