# HydraThemeSync.md

Este documento descreve o ritual de sincronização de temas visuais e emocionais da HydraLife.

## 🎯 Propósito

Garantir que os temas visuais (Colorful-Plasma-Themes) estão atualizados com o repositório original (`L4ki`) e que o sistema Hydra aplica o estado emocional correto.

## 🔄 Processo de sincronização

1. O script `update-themes.sh` é invocado
2. O repositório embutido `Colorful-Plasma-Themes` puxa alterações do `upstream`
3. O `manifest.json` é lido para aplicar:
   - Nome do tema
   - Estado emocional (`mood`)
   - Cor dominante
   - Fonte
   - Som ambiente
   - Vídeo cerimonial

## 🧙 Arquétipo

Cada tema carrega uma presença. `Hydra-Calma`, por exemplo, invoca paciência, pausa e regeneração. Outros temas podem invocar foco, vibração, introspeção.

## 🧠 Observações técnicas

- O repositório de temas é um Git embutido (não submódulo)
- O `upstream` deve apontar para `https://github.com/L4ki/Colorful-Plasma-Themes`
- O script deve ser executado com permissões (`chmod +x`)
- O `manifest.json` deve estar presente e válido

## 🌀 Estado atual

- Última sincronização: 2025-11-05
- Tema ativo: `Hydra-Calma`
- Mood: `calm`
- Fonte: `Segoe UI`
- Som: `Patience_HD_003.mp4`
- Vídeo: `6-take_breaks_001.webm`

- Última sincronização: 2025-11-07


---

Este documento é parte da pasta `aboutApp`, onde a Hydra explica quem é, como respira, e como muda de pele com intenção.

---
