using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace Luban.Editor
{
    public static class GenEntry
    {
        [MonKey.Command("Gen Config", "Luban Config Gen", Category = "Config")]
        public static void GenAuto()
        {
            AssetDatabase.LoadAssetAtPath<LubanExportConfig>("Assets/Editor/Luban/Editor/Luban.asset").Gen();
        }
    }
    
    internal static class GenUtils
    {
        public static void Gen(string dotnet_path,
                               string arguments,
                               string before,
                               string after)
        {
            Debug.Log(arguments);

            TypeConvert.BEFORE_TYPES.TryGetValue(before, out var before_type);
            TypeConvert.AFTER_TYPES.TryGetValue(after, out var after_type);

            IBeforeGen before_gen = null;
            IAfterGen  after_gen  = null;

            if(before_type != null)
            {
                before_gen = Activator.CreateInstance(before_type) as IBeforeGen;
            }

            if(after_type != null)
            {
                after_gen = Activator.CreateInstance(after_type) as IAfterGen;
            }

            before_gen?.Process();

            var process = _Run(
                dotnet_path,
                arguments,
                ".",
                true
            );

            Debug.Log(process.StandardOutput.ReadToEnd());

            after_gen?.Process();

            AssetDatabase.Refresh();
        }

        private static Process _Run(string exe,
                                    string arguments,
                                    string working_dir = ".",
                                    bool   wait_exit   = false)
        {
            bool redirect_standard_output = true;
            bool redirect_standard_error  = true;
            bool use_shell_execute        = false;

            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                redirect_standard_output = false;
                redirect_standard_error  = false;
                use_shell_execute        = true;
            }

            if(wait_exit)
            {
                redirect_standard_output = true;
                redirect_standard_error  = true;
                use_shell_execute        = false;
            }

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName               = exe,
                Arguments              = arguments,
                CreateNoWindow         = true,
                UseShellExecute        = use_shell_execute,
                WorkingDirectory       = working_dir,
                RedirectStandardOutput = redirect_standard_output,
                RedirectStandardError  = redirect_standard_error,
                StandardOutputEncoding = Encoding.UTF8
            };

            Process process = Process.Start(info);

            if(wait_exit)
            {
                process.WaitForExit();
                if(process.ExitCode != 0)
                {
                    throw new Exception($"{process.StandardOutput.ReadToEnd()} {process.StandardError.ReadToEnd()}");
                }
            }
            
            return process;
        }
    }
}