using HydraLife;

using LifeCicles.Modules.Media;
using LifeCicles.Modules.Theme;
using LifeCicles.Modules.UI;
//using System.Text.Json; // ← Este é o que faltava
using Newtonsoft.Json;
using System;
using System.IO;
namespace LifeCicles.Modules
{
    internal class HydraLauncher
    {
        public static void Launch()
        {
            Console.WriteLine("🚪 Iniciando ritual de entrada da Hydra...");

            string mood = HydraMediaLexicon.GetCurrentMood();
            string message = HydraMediaLexicon.GetSplashMessage(mood);
            string music = HydraMediaLexicon.GetSuggestedTrack(mood);
            SplashVisual style = SplashScreenManager.AdaptVisual(mood);

            var theme = HydraThemeLoader.LoadTheme("LifeCicles/Assets/Themes/Colorful-Plasma-Themes/ThemeManifest.json");
            // Aplica cor ao terminal
            ConsoleColor consoleColor = HydraThemeLoader.ConvertToConsoleColor(theme.PrimaryColor);
            Console.BackgroundColor = consoleColor;
            Console.Clear(); // aplica a cor ao fundo
            

            if (!HydraThemeLoader.IsValid(theme))
            {
                Console.WriteLine("[ERROR] Tema inválido. Ritual abortado.");
                return;
            }


            HydraThemeLoader.Apply(theme);

            Directory.CreateDirectory("HydraLogs");
            File.AppendAllText("HydraLogs/ThemeHistory.log", $"{DateTime.Now}: {theme.Name}\n");

            var splash = new SplashScreen
            {
                Message = HydraMediaLexicon.GetSplashMessage(HydraMediaLexicon.GetCurrentMood()),
                MusicPath = HydraMediaLexicon.GetSuggestedTrack(HydraMediaLexicon.GetCurrentMood()),
                VisualStyle = SplashScreenManager.AdaptVisual(HydraMediaLexicon.GetCurrentMood()),
                Theme = theme
            };

            splash.Show();

            Console.WriteLine("🌀 Ritual iniciado. A Hydra está desperta.");
            try
            {
                System.Diagnostics.Process.Start("bash", "LifeCicles/Assets/Themes/Colorful-Plasma-Themes/update-themes.sh");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Theme auto-update failed: " + ex.Message);
            }
        }
    }
}
