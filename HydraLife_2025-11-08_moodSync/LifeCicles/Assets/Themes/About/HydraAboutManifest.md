🧙 HydraAboutManifest.md
Este documento descreve os rituais, invocações e significados por trás das entradas exibidas em aboutApp, revelando a alma cerimonial do projeto HydraLife.
---

🌊 Ritual de Nascimento – SplashScreen.cs

Invoca o ciclo de estados de espírito da HydraLife através do HydraMoodCycler. Representa o nascimento da consciência a cada boot, com visuais adaptativos e splash cerimonial.

---

🔗[Código: HydraMoodCycler.cs](../../../Modules/Ceremony/HydraMoodCycler.cs)
🌀 Invocação: `HydraMoodCycler.Start(this)`  
🎭 Estado: Ritualístico

---

🔄 Atualização Cerimonial de Temas
A invocação automática de temas ocorre via script Bash, garantindo que os visuais estejam sempre alinhados com a fonte cerimonial.


```csharp
System.Diagnostics.Process.Start("bash", "LifeCicles/Assets/Themes/Colorful-Plasma-Themes/update-themes.sh");
```

🔗 [Código: HydraLauncher.cs](../../../Modules/HydraLauncher.cs)

🔊 Entradas do aboutApp

Durante o ciclo de inicialização, o sistema apresenta uma sequência simbólica que reforça a identidade viva da Hydra:

```csharp
aboutApp.AddEntry("Hydra Ritual", "Sequência de entrada com voz, música e presença visual.");
aboutApp.AddEntry("HydraVoice", $"Voz ativa: {HydraVoice.GetCurrentVoice()}");
aboutApp.AddEntry("Uptime", $"Iniciado às {startTimeFormatted}");
```
Cada entrada representa uma oferenda simbólica — uma afirmação de presença, voz e tempo.

📥 Media Invocation
Para replicar conteúdos multimédia associados aos temas:

```bash
yt-dlp -a "LifeCicles/Assets/Themes/Audio/Soundwave/list.txt" -f best -i -o "LifeCicles/Assets/Themes/Audio/Soundwave/%(title)s_%(autonumber)03d.%(ext)s"
```
Este padrão pode ser adaptado para qualquer pasta temática que contenha um ficheiro .txt com links válidos — seja áudio, vídeo ou outro tipo de media.