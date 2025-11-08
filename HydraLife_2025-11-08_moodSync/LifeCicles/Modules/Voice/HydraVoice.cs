using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.IO;


namespace LifeCicles.Modules.Voice
{
   
    public static class HydraVoice
    {
        private static SpeechSynthesizer synth;
        public static string GetCurrentVoice()
        {
            return synth?.Voice?.Name ?? "Voz não definida";
        }

        static HydraVoice()
        {
            synth = new SpeechSynthesizer();
            try
            {
                synth.SelectVoice("Microsoft Zira Desktop");
            }
            catch (ArgumentException)
            {
                synth.SelectVoice("Microsoft Zira Desktop"); // fallback
                synth.SpeakAsync("A voz Helena não está disponível. A Hydra usará Zira.");
            }
            synth.Volume = 100;
            synth.Rate = -10; // mais lento que o padrão (0)

        }


        public static List<string> GetInstalledVoiceNames()
        {
            return synth.GetInstalledVoices().Select(v => v.VoiceInfo.Name).ToList();
        }

        public static void Speak(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                synth = new SpeechSynthesizer();
                synth.SelectVoice("Microsoft Zira Desktop"); // ou outra disponível
                synth.Volume = 100;
                synth.Rate = 0;
                
                synth.SpeakAsync(text);
            }
        }

        public static void WelcomeUser(string username)
        {
            Speak($"Bem-vindo, {username}. A Hydra está consciente.");
        }

        public static void AnnounceDirectoryCreation(string path)
        {
            Speak($"Diretório criado com sucesso: {Path.GetFileName(path)}.");
        }

        public static void RitualClosing()
        {
            Speak("Encerramento cerimonial em progresso. Até breve.");
        }

        public static void NarrateMood(string mood)
        {
            Speak($"Estado emocional atual: {mood}. Ajustando atmosfera.");
        }

        public static void ReadText(string content)
        {
            Speak(content);
        }
    }
}
