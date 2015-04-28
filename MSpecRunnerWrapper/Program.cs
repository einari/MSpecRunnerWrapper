using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MSpecRunnerWrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var location = entryAssembly.Location;
            var runnerDir = Path.GetDirectoryName(location);

            var assemblies = args.Where(s => Path.GetExtension(s).ToLowerInvariant() == ".dll");
            var arguments = args.Where(s => Path.GetExtension(s).ToLowerInvariant() != ".dll");

            var x86Assemblies = assemblies.Where(a => AssemblyName.GetAssemblyName(a).ProcessorArchitecture == ProcessorArchitecture.X86);
            var remainingAssemblies = assemblies.Where(a => !x86Assemblies.Contains(a));

            var argumentsAsString = string.Empty;
            foreach (var argument in arguments) argumentsAsString += argument + " ";

            RunAssemblies(runnerDir, x86Assemblies, argumentsAsString, "mspec-x86-clr4.exe");
            RunAssemblies(runnerDir, remainingAssemblies, argumentsAsString, "mspec-clr4.exe");
        }

        private static void RunAssemblies(string runnerDir, IEnumerable<string> assemblies, string argumentsAsString, string runner)
        {
            if (assemblies.Count() == 0) return;

            var runnerProcess = Path.Combine(runnerDir, runner);

            var processStartInfo = new ProcessStartInfo(runnerProcess, argumentsAsString);
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            
            
            foreach (var assembly in assemblies)
            {
                processStartInfo.Arguments = processStartInfo.Arguments + " " + assembly;
            }

            using (var process = new Process())
            {
                process.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
                process.StartInfo = processStartInfo;
                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();

                process.WaitForExit();
            }
        }
    }
}
