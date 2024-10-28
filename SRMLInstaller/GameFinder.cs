using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SRMLInstaller
{
    static class GameFinder
    {
        private const string gameDLL = "Assembly-CSharp.dll";

        private const string gameName = "SlimeRancher";

        private const string gameNameWithSpace = "Slime Rancher";

        private static readonly string dataFolder = gameName + "_Data";

        private const string epicPath = "C:/Program Files/Epic Games/";

        private const string steamPath32 = "C:/Program Files (x86)/Steam/steamapps/common/";

        private const string steamPath64 = "C:/Program Files/Steam/steamapps/common/";

        private const string drmfree64 = "C:/Program Files/";

        private const string drmfree32 = "C:/Program Files (x86)/";

        private static readonly string exeToDLL = Path.Combine(dataFolder, Path.Combine("Managed", gameDLL));

        private const string GameExe = "SlimeRancher.exe";

        static bool CheckForValidDllPath(string path)
        {
            var parent = Directory.GetParent(path);
            return parent.Name == "Managed" && parent.Parent.Parent.GetFiles().Any((x) => x.Name == GameExe);
        }

        public static string FindGame()
        {
            if (File.Exists(gameDLL) && CheckForValidDllPath(Path.GetFullPath(gameDLL))) return Path.GetFullPath(gameDLL);
            var managedDLL = Path.Combine("Managed", gameDLL);
            if (File.Exists(managedDLL))
                return Path.GetFullPath(managedDLL);
            if (File.Exists(Path.Combine(dataFolder, managedDLL)))
                return Path.GetFullPath(Path.Combine(dataFolder, managedDLL));

            Console.WriteLine($"Not in a game folder! Searching for {gameName}...");

            Console.WriteLine();

            List<string> candidates = new List<string>();

            void AddIfCandidate(string path)
            {
                if (CheckPathForGame(path, gameName))
                    candidates.Add(Path.Combine(path, gameName));
                if (CheckPathForGame(path, gameNameWithSpace))
                    candidates.Add(Path.Combine(path, gameNameWithSpace));
            }
            AddIfCandidate(epicPath);
            AddIfCandidate(steamPath32);
            AddIfCandidate(steamPath64);
            AddIfCandidate(drmfree32);
            AddIfCandidate(drmfree64);

            if (candidates.Count == 0) throw new Exception($"Could not auto-locate game folder! Please move {Path.GetFileName(Assembly.GetExecutingAssembly().Location)} to a valid game folder and try again");
            var candidatetoDLL = exeToDLL;
            if (candidates.Count > 1)
            {
                Console.WriteLine("Found multiple candidates for the game path!");

                return Path.Combine(SelectFromList(candidates), candidatetoDLL);
            }

            var p = Path.Combine(candidates[0], candidatetoDLL);
            Console.WriteLine($"Found {p}!");
            return p;
        }

        static bool CheckPathForGame(string path, string gameName)
        {
            return (File.Exists(Path.Combine(path, gameName, exeToDLL)));
        }

        static T SelectFromList<T>(List<T> elements)
        {

            for (int i = 0; i < elements.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {elements[i]}");
            }
        restart:
            Console.Write($"Please select an option from 1 to {elements.Count}: ");
            if (int.TryParse(Console.ReadLine(), out int val) && val <= elements.Count && val >= 1)
                return elements[val - 1];
            goto restart;
        }
    }
}
