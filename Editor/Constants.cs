namespace Subnautica.Importer
{
    using System.IO;
    using UnityEngine;
    using TkPriority = ThunderKit.Common.Constants.Priority;
    public static class Constants
    {
        public static class Priority
        {
            public const int PostProcessingInstaller = TkPriority.AssemblyImport + 250_000;
            public const int TextMeshProUninstaller = TkPriority.AssemblyImport + 240_000;
            public const int UGUIUninstaller = TkPriority.AssemblyImport + 230_000;
            public const int AssemblyPublicizerConfiguration = TkPriority.AssemblyImport + 125_000;
            public const int InstallBepInEx = TkPriority.AddressableCatalog - 135_000;
            public const int ThunderstorePackageInstaller = TkPriority.AddressableCatalog - 250_000;
        }

        public static class Paths
        {
            public const string NStripExePath = "Packages/com.mrpurple6411.subnauticaimportextensions/Binary/NStrip/NStrip.exe";
            public static string PublicizedAssembliesFolder
            {
                get
                {
                    string tempFolder = Application.dataPath.Replace("Assets", "Temp");
                    return Path.Combine(tempFolder, "ThunderKit", "PublicizedAssemblies");
                }
            }
        }
    }
}