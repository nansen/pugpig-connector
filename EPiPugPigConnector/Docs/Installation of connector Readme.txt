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

* A couple of dynamically added modules will attach handling to missing images, html files (404 or 500) 
to avoid offline manifest problems with pugpig (just returns 200 OK even if image is missing).
handling for all urls with .html and .manifest extension.

* A logfile will be added to ~/App_Data/pugpig_connector.log as default location.

* Create a Editions Page template directly under the CMS root page, and add values to props.
- you should only have one Editions Page per website. (it is used to render the editions.xml feed)

* Create a Edition Page template and add values to props.
* There must be some sort of content pages published below the Edition Page root.
* Publish the editions and editions page to generate the OPDS feed. / offline manifest file.