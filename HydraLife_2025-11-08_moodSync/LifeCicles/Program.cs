using HydraLife;
using LifeCicles.Boot_System;
using LifeCicles.Modules;
using LifeCicles.Modules.UI;
using System;

using System.Windows.Forms;

namespace LifeCicles.Boot_System
{ 
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Invocação cerimonial da Hydra
            Application.Run(new SplashScreen());
        }
        public class HydraLauncher : Form
        {

     
            public HydraLauncher()
            {
                // Inicialização cerimonial da janela
                Text = "🌀 HydraLauncher";
                Width = 800;
                Height = 600;

            }
          

            
        }          
        
    }
}

