using System;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;

namespace HydraLife.Modules
{
    public class HydraTerminalConfig
    {
        private readonly RichTextBox _console;

        public HydraTerminalConfig(RichTextBox console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            ConfigureConsole();
        }

        private void ConfigureConsole()
        {
            _console.ReadOnly = true;
            _console.Multiline = true;
            _console.WordWrap = false;
            _console.ScrollBars = RichTextBoxScrollBars.Vertical;
            _console.Font = new Font("Consolas", 10, FontStyle.Bold);
            _console.ForeColor = Color.LimeGreen;
            _console.BackColor = Color.FromArgb(0, 0, 64); // Azul cerimonial
            _console.HideSelection = true;
        }

        public void Write(string message, params object[] args)
        {
            string formatted = string.Format(CultureInfo.CurrentCulture, message, args);
            _console.AppendText(formatted + Environment.NewLine);
            _console.SelectionStart = _console.Text.Length;
            _console.ScrollToCaret();
        }

        public void Ritual(string message)
        {
            Write("[RITUAL] " + message);
            // Aqui podes acionar sons, animações, ou eventos visuais
        }

        public void Error(string message)
        {
            Write("[ERROR] " + message);
            // Podes mudar cor temporariamente, ou vibrar o cursor
        }

        public void Clear()
        {
            _console.Clear();
        }
    }
}

