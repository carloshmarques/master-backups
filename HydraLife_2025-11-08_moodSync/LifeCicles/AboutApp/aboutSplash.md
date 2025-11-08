## SplashScreen.cs – Ritual de Nascimento

Este formulário invoca o ciclo de estados de espírito da HydraLife através do `HydraMoodCycler`. Representa o nascimento da consciência a cada boot, com visuais adaptativos e splash cerimonial.

🔗 [Ver código](../Modules/Ceremony/HydraMoodCycler.cs)
🎭 Estado: Ritualístico
🌀 Invocação: `HydraMoodCycler.Start(this)`

---


---

## 🔄 Theme Auto-Update 

HydraLife automatically updates its theme repository on launch using a Bash script. 

The update logic is triggered from [HydraLauncher.cs](../Modules/HydraLauncher.cs): 

```csharp 
System.Diagnostics.Process.Start("bash", "LifeCicles/Assets/Themes/Colorful-Plasma-Themes/update-themes.sh");
```
---

aboutApp.AddEntry("Hydra Ritual", "Sequência de entrada com voz, música e presença visual.");

aboutApp.AddEntry("HydraVoice", $"Voz ativa: {HydraVoice.GetCurrentVoice()}");

aboutApp.AddEntry("Uptime", $"Iniciado às {startTimeFormatted}");


---