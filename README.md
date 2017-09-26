# Umbraco CDN Purge #

[![Build status](https://ci.appveyor.com/api/projects/status/ppa2kr5mfpi67g07/branch/master?svg=true
)](https://ci.appveyor.com/project/alindgren/cdnpurge/branch/master)
[![NuGet release](https://img.shields.io/nuget/v/PurgeCDN.svg)](https://www.nuget.org/packages/PurgeCDN)


A package which allows editors to purge CDN content in Umbraco 7.  Currently works with KeyCDN but if you are interested in support for other CDNs, please create an issue.

## Installation ##

Install via nuget.

    PM> Install-Package PurgeCDN

## Configuration ##

There are four app settings that need to be added:

    <add key="PurgeCdn" value="keycdn" />
    <add key="PurgeCdnApiKey" value="API KEY" />
    <add key="PurgeCdnZoneId" value="1234" />
    <add key="PurgeCdnZoneUrl" value="ZONE URL" />
    <add key="PurgeCdnMethod" value="tag" />
    
Currently this just supports KeyCDN, so the first app setting is not used, but when support for other CDNs are added, it will be used to determine which CDN API to call.

`PurgeCdnMethod` values can be `tag` or `url`.  When set to `tag`, it will use the tag `node-x` where x is the id of the node.  When purging call content, it will use the tag `umbhtml`.  For this to work, you need to add an HTTP header to your pages. For KeyCDN, you can do this as follows:

    Response.AddHeader("Cache-Tag", "umbhtml node-" + Model.Id);

When `PurgeCdnMethod` is set to `url`, it will purge by all the URLs (as returned by the current UrlProvider). 

## Usage ##

When configured, a "Purge CDN" option will appear in the contextual menu (under "Actions" menu for each node and when clicking "Do something else" button from the contextual menu.
