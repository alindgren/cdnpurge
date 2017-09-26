using System;
using System.Collections.Generic;
using System.Web.Http;
using Umbraco.Web;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using PurgeCDN.Web.Models;

namespace PurgeCDN.Web.Controllers.API
{
    [PluginController("PurgeCdn")]
    public class PurgeCdnAPIController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        public DialogViewModel InitDialog(int nodeId)
        {
            if (nodeId == -1)
            {
                if (CdnPurger.PurgeMethod == "tag")
                {
                    return new DialogViewModel()
                    {
                        NodeId = nodeId,
                        NodeName = "All content",
                        Status = "ok"
                    };
                }
                else
                {
                    return new DialogViewModel() { Status = "error", ErrorMessage = "Purging by tag not enabled." };
                }
            }

            if (nodeId < -1)
            {
                return new DialogViewModel() { Status = "error", ErrorMessage = "Purging of this node is not allowed." };
            }

            var node = Umbraco.TypedContent(nodeId);
            if (node == null)
            {
                return new DialogViewModel() { Status = "error", ErrorMessage = "Invalid node id: " + nodeId };
            }

            if (node.TemplateId == 0)
            {
                return new DialogViewModel() { Status = "error", ErrorMessage = "There is no template for this node." };
            }

            // if KeyCDN Client ID and/or Secret are missing, return error message
            if (!CdnPurger.IsActive())
                return new DialogViewModel() { Status = "error", ErrorMessage = "KeyCDN Client ID and/or Secret not set." };

            return new DialogViewModel()
            {
                NodeId = nodeId,
                NodeName = node.Name,
                Status = "ok"
            };
        }

        [HttpGet]
        public DialogViewModel Purge(int nodeId)
        {
            if (nodeId == -1 && CdnPurger.PurgeMethod == "tag")
            {
                // purge all ("umbhtml")
                CdnPurger.PurgeByTag(new string[] { "umbhtml" });
            }
            try
            {
                var node = Umbraco.TypedContent(nodeId);
                if (node != null)
                {
                    if (CdnPurger.PurgeMethod == "tag")
                    {
                        CdnPurger.PurgeByTag(new string[] { "node-" + node.Id });
                    }
                    else
                    {
                        var urlProvider = UmbracoContext.Current.RoutingContext.UrlProvider;
                        List<string> urls = new List<string>(urlProvider.GetOtherUrls(nodeId));
                        urls.Add(urlProvider.GetUrl(nodeId));
                        CdnPurger.PurgeByUrls(urls.ToArray());
                    }
                    return new DialogViewModel()
                    {
                        NodeId = nodeId,
                        NodeName = node.Name,
                        Status = "purged"
                    };
                }
            }
            catch (Exception ex)
            {
                return new DialogViewModel()
                {
                    ErrorMessage = "Error: " + ex.Message,
                    Status = "error"
                };
            }


            return new DialogViewModel()
            {
                ErrorMessage = "Error purging CDN.",
                Status = "error"
            };
        }
    }
}
