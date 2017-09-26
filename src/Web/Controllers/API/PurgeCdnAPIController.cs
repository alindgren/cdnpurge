using System.Configuration;
using System.Web.Http;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using PurgeCDN.Web.Models;

namespace PurgeCDN.Web.Controllers.API
{
    [PluginController("PurgeCdn")]
    public class PurgeCdnAPIController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        public DialogViewModel InitDialog()
        {
            // if KeyCDN Client ID and/or Secret are missing, return error message
            if (!CdnPurger.IsActive())
                return new DialogViewModel() { Status = "error", ErrorMessage = "KeyCDN Client ID and/or Secret not set." };

            return new DialogViewModel()
            {
                Status = "ok"
            };
        }

    }
}
