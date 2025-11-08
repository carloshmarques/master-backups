using LifeCicles.Modules;
using LifeCicles.Modules.Themes;
using LifeCicles.Modules.UI;
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeCicles.Modules
{
    public static class HydraThemeManager
    {
        
        public static void Apply(ThemeManifest theme)
        {
            Console.WriteLine($"🎨 Aplicando tema: {theme.Name}");
            Console.WriteLine($"🧘 Mood: {theme.Mood}");
            Console.WriteLine($"🔤 Fonte: {theme.FontFamily}");
            Console.WriteLine($"🎧 Música: {theme.Sound}");
            Console.WriteLine($"📺 Vídeo: {theme.Video}");
            Console.WriteLine($"🌈 Cor: {theme.PrimaryColor}");

            // Aplicar cor de fundo global (se houver um formulário principal)
            Color backgroundColor = ColorTranslator.FromHtml(theme.PrimaryColor ?? "#000000");

            // Aplicar fonte padrão
            Font defaultFont = new Font(theme.FontFamily ?? "Segoe UI", 10);

            // Exemplo: aplicar a todos os formulários abertos
            foreach (Form form in Application.OpenForms)
            {
                form.BackColor = backgroundColor;
                form.Font = defaultFont;
            }

            // Se houver imagem de fundo
            if (!string.IsNullOrWhiteSpace(theme.BackgroundImage) && File.Exists(theme.BackgroundImage))
            {
                foreach (Form form in Application.OpenForms)
                {
                    form.BackgroundImage = Image.FromFile(theme.BackgroundImage);
                    form.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }

            // Aqui podes adicionar lógica para tocar música, iniciar vídeo, etc.
        }

        public static void AdaptVisual(Form targetForm, string mood)
        {
            switch (mood)
            {
                case "Sereno":
                    targetForm.BackColor = Color.LightBlue;
                    break;
                case "Eufórico":
                    targetForm.BackColor = Color.OrangeRed;
                    break;
                case "Melancólico":
                    targetForm.BackColor = Color.Gray;
                    break;
                case "Ritualístico":
                    targetForm.BackColor = Color.Purple;
                    break;
                default:
                    targetForm.BackColor = Color.Black;
                    break;
            }

            Console.WriteLine($"🎨 Visual adaptado para o mood: {mood}");
        }

    }
}
