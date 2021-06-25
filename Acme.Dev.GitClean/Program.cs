using System;

namespace Acme.Dev.GitClean
{
    using System.Diagnostics;
    using System.Text;

    using CommandLine;

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<StartupOptions>(args)
                .WithParsed(ExecuteGitClean);
        }

        private static void ExecuteGitClean(StartupOptions options)
        {
            ExecuteCommand("git fetch");

            if (!string.IsNullOrWhiteSpace(options.MainBranchName))
            {
                ExecuteCommand($"git checkout {options.MainBranchName}");
                ExecuteCommand("git pull");
            }

            if (!string.IsNullOrWhiteSpace(options.DevelopBranchName))
            {
                ExecuteCommand($"git checkout {options.DevelopBranchName}");
                ExecuteCommand("git pull");
            }

            if (string.IsNullOrWhiteSpace(options.DevelopBranchName) && string.IsNullOrWhiteSpace(options.MainBranchName))
            {
                ExecuteCommand("git pull");
            }

            ExecuteCommand("git remote update origin --prune");

            var branches = ExecuteCommand("git branch -vv");

            foreach (var branch in branches.Split("\n"))
            {
                if (branch.IndexOf(": gone]", StringComparison.Ordinal) > 0)
                {
                    var branchName = branch.Trim().Split(' ')[0];
                    ExecuteCommand($"git branch -D {branchName}");
                }
            }
        }

        private static string ExecuteCommand(string command)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            Console.WriteLine(command);
            var process = Process.Start(processInfo);

            if (process == null)
            {
                return string.Empty;
            }

            var output = new StringBuilder();

            process.OutputDataReceived += (_, e) =>
            {
                Console.WriteLine(e.Data);
                output.AppendLine(e.Data);
            };
            process.BeginOutputReadLine();

            process.ErrorDataReceived += (_, e) =>
            {
                Console.WriteLine(e.Data);
                output.AppendLine(e.Data);
            };
            process.BeginErrorReadLine();

            process.WaitForExit();

            process.Close();

            Console.WriteLine(string.Empty.PadRight(75, '='));

            return output.ToString();
        }
    }
}
