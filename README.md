# sitecore-assurance

## Summary

Sitecore Assurance is a test tool for Sitecore CMS sites (versions 7.5 and above). It uses the Sitecore Services Client to traverse the content tree and report on the status of pages in the web site.

For each page found during the crawl of the Sitecore content tree, a web request is performed and the page status and HTTP status code is reported back to the console.

Future developments of this tool will enhance the reporting capabilities for integration with an automated site testing process (for example)

## Usage

The Sitecore Assurance command line tool accepts the following command line arguments:

`sitecore-assurance -r {root node guid} -u {base url}`

or

`sitecore-assurance --root {root node guid} --baseurl {base url}`


Where `base url` is the URL of your Sitecore site, i.e. `http://jobs.tac.local/`

Where `root node guid` is the node in the Sitecore content tree that you want to start the traversal from, i.e. `{394EE7DC-036C-4FDD-9F79-0699296130E0}`

Typically the root node guid will be the Item ID of the `/sitecore/content/Home` item, but this can be any collection of pages within the Sitecore web site that require monitoring.

## Limitations

At present, the tool will only function if the Sitecore Services Client has anonymouse access enabled. Future developments of this tool will enable use with authentication.

To configure anonymous access, change the following setting in the `Sitecore.Services.Client.config` file in `{web site root}\App_Config\Include`:

`<setting name="Sitecore.Services.AllowAnonymousUser" value="true" />`
