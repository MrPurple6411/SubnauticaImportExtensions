namespace Subnautica.Importer
{
    using ThunderKit.Integrations.Thunderstore;

    public class InstallBepInEx : ThunderstorePackageInstaller
    {
        public override int Priority => Constants.Priority.InstallBepInEx;
        public override string ThunderstoreAddress => "https://subnautica.thunderstore.io";
        public override string DependencyId => "Subnautica_Modding-BepInExPack";
        public override string Description => $"Installs the latest version of BepInEx.\r\nThe Unified BepInEx all-in-one modding pack - plugin framework, detour library";
        public override string Name => $"Install BepInEx";
    }
}

