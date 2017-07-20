# Sitecore Assurance

## Summary

Sitecore Assurance is a test tool for Sitecore CMS sites (versions 7.5 and above). It uses the Sitecore Services Client to traverse the content tree and report on the pages in the web site.

Future developments of this tool will enhance the reporting capabilities for integration with an automated site testing process (for example)

## Installation

After building from source, the `Revida.Sitecore.Services.Client.Extensions.dll` should be deployed to the web site `bin` folder for the target Sitecore website.

The `Revida.Sitecore.Assurance.Services.Client.Extensions.config` configuration file should also be deployed to the web site `App_Config\Include` folder.


This DLL modifies the output of the Sitecore Services Client web service to include additional information required by the Sitecore Assurance tool.

## Usage

The Sitecore Assurance command line tool accepts the following command line arguments:

`sitecore-assurance -r {root node guid} -b {base url} [-u {user name}] [-p {password}] [-d {domain}] [-l] [-h] [-s]`

or

`sitecore-assurance --root {root node guid} --baseurl {base url} [--username {user name}] [--password {password}] [--domain {domain}] [--list] [--http] [--selenium]`


Where `base url` is the URL of your Sitecore site, i.e. `http://jobs.tac.local/`

Where `root node guid` is the node in the Sitecore content tree that you want to start the traversal from, i.e. `{394EE7DC-036C-4FDD-9F79-0699296130E0}`

Typically the root node guid will be the Item ID of the `/sitecore/content/Home` item, but this can be any collection of pages within the Sitecore web site that require monitoring.

Where `user name`, `password` and `domain` contain Sitecore credentials for login to the Sitecore Services Client, if anonymous access is not enabled. If these credentials are not supplied, the tool will attempt to access without providing authentication.

If the `-l` or `--list` option is used, then the tool will list the URLs found during the crawl of the content tree, but will not test the validity of the pages.

The `-h` or `--http` option specifies that the tool should execute HTTP status code tests for each URL in the Sitecore content tree.

The `-s` or `--selenium` option specifies that the tool should execute Selenium WebDriver tests for each URL in the Sitecore content tree.

## Sitecore Services Client Configuration

To configure anonymous access to Sitecore Services Client, change the following setting in the `Sitecore.Services.Client.config` file in
`{web site root}\App_Config\Include`:

`<setting name="Sitecore.Services.AllowAnonymousUser" value="true" />`

## Limitations

At present, the Selenium WebDriver tests are very limited, checking for the presence of the HEAD and BODY tags only. Future developments of this tool will enable the configuration of more extensive WebDriver tests.
