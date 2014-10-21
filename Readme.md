# Tidy Orchard Development Toolkit Readme



The Tidy Orchard Development Toolkit allows you to develop [Orchard](http://orchardproject.net/)-based applications in a way that you own code (e.g. extensions, configuration) is completely separated from the core Orchard source.

This makes Orchard development not only tidier but it also allows you to:

- Manage your extensions better: e.g. now you can keep all your modules under a single repository (with subrepositories for other modules) instead of having all your modules in separate repositories.
- Updating or upgrading the Orchard source is a matter of pulling in the latest changes from the Orchard repository.
- You can even keep a single (or just a few) folders on your computer with the Orchard source that you link to from each of your solutions, thus minimizing storage space usage and build time.

**Keep in mind that this toolkit is purely experimental!**


## Creating a Tidy Orchard solution

There is a sample solution with all of the below tasks already done: see the [Tidy Orchard Development Quick Start](https://bitbucket.org/Lombiq/tidy-orchard-development-quick-start).

1. Create a folder in the root for your web project (e.g. “Orchard.Web” but the name is not mandatory) and copy the contents of Orchard.Web there (the Web csproj can also have an arbitrary name). Modify the Web.config as in the sample.
2. Add the Toolkit to the Lombiq.TidyOrchardDevelopmentToolkit under your web project's folder.
3. Add the full Orchard source to the Web project's folder under a folder called "Orchard". This should be the full Orchard source (e.g. with the lib and src folders in the root). Please note that you have to remove the Web.config from Orchard.Web.
4. Copy the Orchard solution file to the root (and optionally rename it).
5. Change all project references of the solution to point to the new web project's content (assuming your web project's folder is called Orchard.Web):
	- Replace "Orchard\ with "Orchard.Web\Orchard\src\Orchard\ (including the quotes).
	- Replace Orchard.Tests\ with Orchard.Web\Orchard\src\Orchard.Tests\.
	- Replace Orchard.Web.Tests\ with Orchard.Web\Orchard\src\Orchard.Web.Tests\.
	- Replace "Orchard.Web\ with "Orchard.Web\Orchard\src\Orchard.Web\ (including the quotes).
	- Replace Orchard.Tests.Modules\ with Orchard.Web\Orchard\src\Orchard.Tests.Modules\.
	- Replace Orchard.Core.Tests\ with Orchard.Web\Orchard\src\Orchard.Core.Tests\.
	- Replace Orchard.WarmupStarter\ with Orchard.Web\Orchard\src\Orchard.WarmupStarter\.
	- Replace "Tools\ with "Orchard.Web\Orchard\src\Tools\ (including the quotes).
	- Replace Orchard.Specs\ with Orchard.Web\Orchard\src\Orchard.Specs\.
	- Replace Orchard.Profile\ with Orchard.Web\Orchard\src\Orchard.Profile\.
6. Copy over the contents of the original Orchard.Web folder to your own web folder except the Core, Modules, Media and Themes folders.
7. Adjust Orchard.Web.csproj:
	- Replace ..\..\lib\ with Orchard\lib\.
	- Replace ProjectReference Include="..\ with ProjectReference Include="Orchard\src\.
	- Replace ProjectReference Include="Core\ with ProjectReference Include="Orchard\src\Orchard.Web\Core\.
8. Add the Toolkit's project to the solution and reference it from the web project.
9. Register the Toolkit's Autofac module in the HostComponents.config file.
10. Register the TidyDevelopmentHttpModule in the Web.config.
11. Add your own themes and modules under the Web project's folder under "Modules" and "Themes" folders, respectively.
12. Modify module project files according to the [Orchard App Host](http://orchardapphost.codeplex.com/) documentation so they support the new solution structure.

Instead of copying you can always create symlinks with mklink instead.