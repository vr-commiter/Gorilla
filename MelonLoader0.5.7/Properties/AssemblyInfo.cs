using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(GorillaTag_TrueGear.BuildInfo.Description)]
[assembly: AssemblyDescription(GorillaTag_TrueGear.BuildInfo.Description)]
[assembly: AssemblyCompany(GorillaTag_TrueGear.BuildInfo.Company)]
[assembly: AssemblyProduct(GorillaTag_TrueGear.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + GorillaTag_TrueGear.BuildInfo.Author)]
[assembly: AssemblyTrademark(GorillaTag_TrueGear.BuildInfo.Company)]
[assembly: AssemblyVersion(GorillaTag_TrueGear.BuildInfo.Version)]
[assembly: AssemblyFileVersion(GorillaTag_TrueGear.BuildInfo.Version)]
[assembly: MelonInfo(typeof(GorillaTag_TrueGear.GorillaTag_TrueGear), GorillaTag_TrueGear.BuildInfo.Name, GorillaTag_TrueGear.BuildInfo.Version, GorillaTag_TrueGear.BuildInfo.Author, GorillaTag_TrueGear.BuildInfo.DownloadLink)]
[assembly: MelonColor()]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]