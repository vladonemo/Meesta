using System;
using System.Net;
using System.Net.Http;

using Meesta.Properties;


namespace Meesta
{
    internal class Notification
    {
        public static void Send(Status statusEvent)
        {
            if (string.IsNullOrEmpty(Settings.Default.IftttUri))
            {
                throw new InvalidOperationException("Please define the IFTTT Uri to send this notification to");
            }
            var httpClientHandler = new HttpClientHandler
            {
                UseProxy = Settings.Default.UseProxy,
                UseDefaultCredentials = true
            };

            if (Settings.Default.UseProxy)
            {
                httpClientHandler.Proxy = new WebProxy(Settings.Default.ProxyUri, false, new string[] {}, CredentialCache.DefaultNetworkCredentials);
            }

            using (var client = new HttpClient(httpClientHandler))
            {
                var task = client.PostAsync(string.Format(Settings.Default.IftttUri, statusEvent), new StringContent(string.Empty));
                task.Wait(5000);
            }
        }
    }
}
