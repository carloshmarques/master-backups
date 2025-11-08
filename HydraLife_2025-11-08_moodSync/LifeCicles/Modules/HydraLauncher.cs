using LifeCicles.Modules.Media;
using LifeCicles.Modules.UI;
using LifeCicles.Modules.Themes;

using System;
using System.IO;
using Newtonsoft.Json;
// ← Este é o que faltava

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

            var json = File.ReadAllText("LifeCicles/Assets/Themes/Colorful-Plasma-Themes/ThemeManifest.json");
            var theme = JsonConvert.DeserializeObject<ThemeManifest>(json);

            HydraThemeManager.Apply(theme);

            var splash = new SplashForm
            {
                Message = message,
                MusicPath = music,
                VisualStyle = style
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
