using System;
using System.Text;
using System.Net;
using Umbraco.Core.Logging;

namespace PurgeCDN
{
    public class CdnPurger
    {
        private static string ApiKey = System.Configuration.ConfigurationManager.AppSettings["PurgeCdnApiKey"];
        private static string PurgeCdnZoneId = System.Configuration.ConfigurationManager.AppSettings["PurgeCdnZoneId"];
        private static string purgeCdnZoneUrl = System.Configuration.ConfigurationManager.AppSettings["PurgeCdnZoneUrl"];
        private static string purgeMethod = System.Configuration.ConfigurationManager.AppSettings["PurgeCdnMethod"];// "tag" or "url" -- default to "url"

        public static string PurgeCdnZoneUrl
        {
            get
            {
                return purgeCdnZoneUrl;
            }
        }

        public static string PurgeMethod
        {
            get
            {
                return purgeMethod;
            }
        }

        public static void PurgeByUrls(string[] urls)
        {
            LogHelper.Info<CdnPurger>("PurgeByUrls() called. URLs: " + String.Join(", ", urls));
            string url = String.Format("https://api.keycdn.com/zones/purgeurl/{0}.json", PurgeCdnZoneId);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
            request.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", ApiKey);
            request.ContentType = "application/json";
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "{\"urls\":[\"" + PurgeCdnZoneUrl + string.Join("\",\"" + PurgeCdnZoneUrl, urls) + "\"]}";
            byte[] bytes = encoding.GetBytes(postData);
            request.ContentLength = bytes.Length;
            var stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            LogHelper.Debug<CdnPurger>("data: " + postData);
            try
            {
                request.Timeout = 250;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var responseCode = response.StatusCode;
                    if (!responseCode.Equals(HttpStatusCode.OK))
                    {
                        LogHelper.Info<CdnPurger>("Purge failed. Response: " + response.StatusCode + " - " + response.StatusDescription + " for " + String.Join(", ", urls));
                    }
                    else
                    {
                        LogHelper.Info<CdnPurger>("Purge successful. Response: " + response.StatusCode + " - " + response.StatusDescription + " for " + String.Join(", ", urls));
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<CdnPurger>("Purge failed for " + String.Join(", ", urls), ex);
            }
        }

        public static void PurgeByTag(string[] tags)
        {
            LogHelper.Info<CdnPurger>("PurgeByTag() called. tags: " + String.Join(", ", tags));
            string url = String.Format("https://api.keycdn.com/zones/purgetag/{0}.json", PurgeCdnZoneId);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
            request.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", ApiKey);
            request.ContentType = "application/json";
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "{\"urls\":[\"" + PurgeCdnZoneUrl + string.Join("\",\""+ PurgeCdnZoneUrl, tags) + "\"]}";
            byte[] bytes = encoding.GetBytes(postData);
            request.ContentLength = bytes.Length;
            var stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            LogHelper.Debug<CdnPurger>("data: " + postData);
            try
            {
                request.Timeout = 250;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var responseCode = response.StatusCode;
                    if (!responseCode.Equals(HttpStatusCode.OK))
                    {
                        LogHelper.Info<CdnPurger>("Purge failed. Response: " + response.StatusCode + " - " + response.StatusDescription + " for " + String.Join(", ", tags));
                    }
                    else
                    {
                        LogHelper.Info<CdnPurger>("Purge successful. Response: " + response.StatusCode + " - " + response.StatusDescription + " for " + String.Join(", ", tags));
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<CdnPurger>("Purge failed for " + String.Join(", ", tags), ex);
            }
        }

        public static bool IsActive()
        {
            return !String.IsNullOrWhiteSpace(ApiKey) && !String.IsNullOrWhiteSpace(PurgeCdnZoneId) && !String.IsNullOrWhiteSpace(PurgeCdnZoneUrl);
        }
    }
}
