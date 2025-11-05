using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using System.Text.Json;
using System.Threading.Tasks;
namespace LifeCicles.Modules.Theme
{
    internal class HydraThemeLoader
    {
        public static ThemeManifest LoadTheme(string manifestPath)
        {
            if (!File.Exists(manifestPath))
                throw new FileNotFoundException("Theme manifest not found.", manifestPath);

            string json = File.ReadAllText(manifestPath);
            ThemeManifest theme = JsonConvert.DeserializeObject<ThemeManifest>(json);
            return theme;
        }
        public static ThemeManifest CurrentTheme { get; private set; }

        public static bool IsValid(ThemeManifest theme)
        {
            if (theme == null)
                return false;

            if (string.IsNullOrWhiteSpace(theme.Name))
                return false;

            if (string.IsNullOrWhiteSpace(theme.PrimaryColor))
                return false;

            if (string.IsNullOrWhiteSpace(theme.FontFamily))
                return false;

            // Podes adicionar mais validações conforme o uso
            return true;
        }

        public static void Apply(ThemeManifest theme)
        {
            // Aplica cores, fontes, imagens, etc.
            Console.WriteLine($"🎨 Aplicando tema: {theme.Name}");
            Console.WriteLine($"Cor primária: {theme.PrimaryColor}");
            Console.WriteLine($"Fonte: {theme.FontFamily}");
            // Aqui podes aplicar visual ao SplashForm ou guardar em estado global
        }
        public static ConsoleColor ConvertToConsoleColor(string hexColor)
        {
            if (string.IsNullOrWhiteSpace(hexColor))
                return ConsoleColor.Black;

            // Remove o "#" se existir
            if (hexColor.StartsWith("#"))
                hexColor = hexColor.Substring(1);

            if (hexColor.Length != 6)
                return ConsoleColor.DarkGray;

            try
            {
                int r = Convert.ToInt32(hexColor.Substring(0, 2), 16);
                int g = Convert.ToInt32(hexColor.Substring(2, 2), 16);
                int b = Convert.ToInt32(hexColor.Substring(4, 2), 16);

                return ClosestConsoleColor(r, g, b);
            }
            catch
            {
                return ConsoleColor.DarkGray;
            }
        }

        private static ConsoleColor ClosestConsoleColor(int r, int g, int b)
        {
            var consoleColors = new Dictionary<ConsoleColor, (int R, int G, int B)>
    {
        { ConsoleColor.Black, (0, 0, 0) },
        { ConsoleColor.DarkBlue, (0, 0, 139) },
        { ConsoleColor.DarkGreen, (0, 100, 0) },
        { ConsoleColor.DarkCyan, (0, 139, 139) },
        { ConsoleColor.DarkRed, (139, 0, 0) },
        { ConsoleColor.DarkMagenta, (139, 0, 139) },
        { ConsoleColor.DarkYellow, (184, 134, 11) },
        { ConsoleColor.Gray, (190, 190, 190) },
        { ConsoleColor.DarkGray, (105, 105, 105) },
        { ConsoleColor.Blue, (0, 0, 255) },
        { ConsoleColor.Green, (0, 255, 0) },
        { ConsoleColor.Cyan, (0, 255, 255) },
        { ConsoleColor.Red, (255, 0, 0) },
        { ConsoleColor.Magenta, (255, 0, 255) },
        { ConsoleColor.Yellow, (255, 255, 0) },
        { ConsoleColor.White, (255, 255, 255) }
    };

            ConsoleColor closest = ConsoleColor.Black;
            double minDistance = double.MaxValue;

            foreach (var kvp in consoleColors)
            {
                double distance = Math.Sqrt(
                    Math.Pow(kvp.Value.R - r, 2) +
                    Math.Pow(kvp.Value.G - g, 2) +
                    Math.Pow(kvp.Value.B - b, 2)
                );

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = kvp.Key;
                }
            }

            return closest;
        }


    }
    public class ThemeManifest
    {
        public string Name { get; set; }
        public string Mood { get; set; }
        public string PrimaryColor { get; set; }
        public string AccentColor { get; set; }
        public string BackgroundImagePath { get; set; }
        public string FontFamily { get; set; }
        public string Color { get; set; } // campo extra do JSON
        public string Sound { get; set; }
        public string Video { get; set; }
    }

}
