using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSRestForm
{
    public class HttpRequest
    {
        private string url;
        public string Url { get => url; set => url = value; }

        public event EventHandler WebRequestCompleted;

        protected virtual void OnWebRequestCompleted(WebRequestCompletedEventArgs e) => WebRequestCompleted?.Invoke(this, e);

        public HttpRequest(string url) => this.Url = url;

        public async Task InvokeAsync()
        {
            var request = WebRequest.Create(Url);
            request.UseDefaultCredentials = true;

            var response = (HttpWebResponse)await Task.Factory
                .FromAsync<WebResponse>(request.BeginGetResponse,
                            request.EndGetResponse,
                            null);

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                OnWebRequestCompleted(new WebRequestCompletedEventArgs()
                {
                    Content = reader.ReadToEnd()
                });
            }

            //Task.Factory
            //    .FromAsync(request.BeginGetResponse, request.EndGetResponse, null)
            //    .ContinueWith(task =>
            //    {
            //        using (var reader = new StreamReader(((HttpWebResponse)task.Result).GetResponseStream()))
            //        {
            //            OnWebRequestCompleted(new WebRequestCompletedEventArgs()
            //            {
            //                Content = reader.ReadToEnd()
            //            });
            //        }

            //        //var response = (HttpWebResponse)task.Result;
            //        //Debug.Assert(response.StatusCode == HttpStatusCode.OK);
            //    });
        }
    }

    public class WebRequestCompletedEventArgs : EventArgs
    {
        public string Content { get; set; }
    }
}
