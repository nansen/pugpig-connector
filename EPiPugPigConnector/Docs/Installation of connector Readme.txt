Modifications to apply to the Web project
==========================================
These modifications will be needed on the web project to install this as a "plugin"
on a EPiServer site.
The goal is to be able to add this project with some form of installer / powershell script, 
so that we can install this as a “plugin” to new EPiServer websites.
==========================================

- The episerver nuget feed must be added to the package manager in Visual Studio http://nuget.episerver.com/feed/packages.svc/
VS will download references for episerver from this feed upon first build.

- "Url rewrite module" must be installed in IIS server. http://www.iis.net/downloads/microsoft/url-rewrite
The following rules for redirecting .html and .manifest urls to dynamically
generated html and manifest files respectively:
<system.webServer>
	..................................
    <rewrite>
      <rules>
        <rule name="HtmlRewrite">
          <match url="(.*)(\.html)$" />
          <conditions logicalGrouping="MatchAny">
            <add pattern="((.*)(\.xml)$)" ignoreCase="true" negate="true" matchType="Pattern" input="{URL}" />
            <add pattern="((.*)(\.manifest)$)" ignoreCase="true" negate="true" matchType="Pattern" input="{URL}" />
          </conditions>
          <action type="Rewrite" url="{R:1}" />
        </rule>

        <rule name="ManifestRewrite">
          <match url="(.*)(\.manifest)$" />
          <action type="Rewrite" url="{R:1}" />
        </rule>
      </rules>
    </rewrite>
</system.webServer>

* A couple of dynamically added modules will attach upon app startup.

* Module for handling missing assets (404 or 500) to avoid offline manifest problems with pugpig (just returns 200 OK even if asset is missing).
The missing assets module is dependent on this web.config setting on the <modules> section:
<modules runAllManagedModulesForAllRequests="true"> to be able to "catch" static file request such as images and so on.
For now the module is added dynamically, if added manually through config make sure:
DONT set the attribute preCondition="managedHandler" directly on yhe missingfile module (this ensures that 
only managed requests (ASP.NET type of files) are fired on this module).
More info: http://weblog.west-wind.com/posts/2012/Oct/25/Caveats-with-the-runAllManagedModulesForAllRequests-in-IIS-78

* Theres a module handling all all urls with .html and .manifest extension.
* A module for creating manifest files on the file. (scans the outgoing html for assets)

* A logfile will be added to ~/App_Data/pugpig_connector.log as default location.

* Create a Editions Page template directly under the CMS root page, and add values to props.
- you should only have one Editions Page per website. (it is used to render the editions.xml feed)

* Create a Edition Page template and add values to props.
* There must be some sort of content pages published below the Edition Page root.
* Publish the editions and editions page to generate the OPDS feed. / offline manifest file.