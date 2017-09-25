using Umbraco.Core;
using Umbraco.Web.Trees;

namespace PurgeCDN.Web.App_Start
{
    public class PurgeCdnApplicationEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication,
    ApplicationContext application)
        {
            TreeControllerBase.MenuRendering += PurgeCdn_MenuRendering;
        }

        private static void PurgeCdn_MenuRendering(TreeControllerBase sender, MenuRenderingEventArgs e)
        {
            if (sender.TreeAlias != "content") return;

            if (CdnPurger.IsActive())
            {
                var menuItem = new Umbraco.Web.Models.Trees.MenuItem("purgeCdn", "Purge CDN");
                menuItem.AdditionalData.Add("actionView", "/App_Plugins/PurgeCdn/Views/purgecdn.html");
                menuItem.AdditionalData.Add("contentId", e.NodeId);
                menuItem.Icon = "axis-rotation";
                menuItem.SeperatorBefore = true;

                e.Menu.Items.Insert(e.Menu.Items.Count, menuItem);
            }
        }
    }
}
