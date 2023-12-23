namespace Subnautica.Importer
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using ThunderKit.Core.Config;
    using ThunderKit.Core.Data;
    using UnityEditor;
    using Debug = UnityEngine.Debug;
    using UObject = UnityEngine.Object;

    public class AssemblyPublicizerProcessor : AssemblyProcessor
    {
        public override int Priority => 400;
        public override string Name => $"Assembly Publicizer Processor";

        public override string Process(string assemblyPath)
        {
            AssemblyPublicizerConfiguration dataStorer = AssemblyPublicizerConfiguration.GetDataStorer();
            //Publicizer not enabled? dont publicize
            if (!dataStorer.enabled)
                return assemblyPath;

            var assemblyFileName = Path.GetFileName(assemblyPath);
            //assembly file name is not in assemblyNames? dont publicize.
            if (!dataStorer.assemblyNames.Contains(assemblyFileName))
                return assemblyPath;

            UObject nstripExe = dataStorer.NStripExecutable;
            if (nstripExe == null)
            {
                Debug.LogWarning($"Could not strip assembly {assemblyFileName}, as NStrip has not been located.");
                return assemblyPath;
            }

            string managedDir = ThunderKitSetting.GetOrCreateSettings<ThunderKitSettings>().ManagedAssembliesPath;

            if (!Directory.Exists(Constants.Paths.PublicizedAssembliesFolder))
            {
                Directory.CreateDirectory(Constants.Paths.PublicizedAssembliesFolder);
            }
            string outputPath = Path.Combine(Constants.Paths.PublicizedAssembliesFolder, assemblyFileName);
            string nstripPath = Path.GetFullPath(AssetDatabase.GetAssetPath(nstripExe));

            List<string> arguments = new List<string>
            {
                "-p",
                "-n",
                "-d", $"\"{managedDir}\"",
                "-cg",
                "--cg-exclude-events",
                "-t ValueRet",
                "--remove-readonly",
                "--unity-non-serialized",
                $"\"{assemblyPath}\"",
                $"\"{outputPath}\""
            };

            List<string> log = new List<string> {  };
            if (!StripAssembly(arguments, nstripPath, assemblyFileName))
            {
                Debug.LogError($"Failed to publicize {assemblyFileName} with arguments: \n{string.Join(" ", arguments)}");
                return assemblyPath;
            }
            
            return outputPath;
        }

        private bool StripAssembly(List<string> arguments, string nstripPath, string assemblyFileName)
        {
            ProcessStartInfo psi = new ProcessStartInfo(nstripPath)
            {
                WorkingDirectory = Path.GetDirectoryName(nstripPath),
                Arguments = string.Join(" ", arguments),
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Debug.Log($"Publicizing {assemblyFileName} with the following arguments:\n{psi.Arguments}");

            var process = System.Diagnostics.Process.Start(psi);
            process.OutputDataReceived += (sender, args) => Debug.Log(args.Data);
            process.ErrorDataReceived += (sender, args) => Debug.LogError(args.Data);
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            return process.ExitCode == 0;
        }
    }
}