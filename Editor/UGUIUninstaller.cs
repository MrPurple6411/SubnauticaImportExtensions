namespace Subnautica.Importer
{
    using ThunderKit.Core.Config.Common;

    public class UGUIUninstaller : UnityPackageUninstaller
    {
        public override string Name => "Unity GUI Uninstaller";
        public override string Description => $"Removes Unity GUI due to compatibility issues with the games modified TextMeshPro library and ensures that Unity.ui.dll is copied from the games directory";
        public override int Priority => Constants.Priority.UGUIUninstaller;
        public override string PackageIdentifier => "com.unity.ugui";
    }
}