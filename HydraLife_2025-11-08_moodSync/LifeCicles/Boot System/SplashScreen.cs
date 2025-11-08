
using HydraLife;
using LifeCicles.Boot_System;
using LifeCicles.Modules;
using LifeCicles.Modules.Ceremony;
using LifeCicles.Modules.Themes;
using LifeCicles.Modules.Voice;

using LifeCicles.Modules.Helpers;
using LifeCicles.Modules.Lexicon;
using LifeCicles.Modules.UI;
using WMPLib;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;



namespace HydraLife
{
    public partial class SplashScreen : Form
    {
        public ThemeManifest Theme { get; set; }
        public string Message { get; set; }
        public string MusicPath { get; set; }
        public object VisualStyle { get; set; } // ou um tipo mais específico se souberes

        private string startTimeFormatted; // ✅ Only here
        private Timer logoSlide;
        public RichTextBox BootConsole => bootMessagesRtb;
        //private readonly HydraTerminal _terminal; //Error cs0723
        private void AppendCeremonialMessage(string message)
        {
            message = "[OK!], proceding!";
            bootMessagesRtb.AppendText(message + "\r\n");
            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
            bootMessagesRtb.SelectionLength = 0;
            bootMessagesRtb.ScrollToCaret(); // 
        }

        public static string HydraDataPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "HydraLife"
                );
        public int BlendSteps { get; set; } // property

        private void InjectSystemDirectories()
        {
            string basePath = HydraDataPath;
            string[] directories = new string[]
            {
            "App",
            "App/Admin",
            "App/Admin/Credentials",
            "App/Admin/Credentials/Password",
            "App/Admin/Credentials/Username",
            "App/Admin/Credentials/Picture",

            };

            foreach (string dir in directories)
            {
                string fullPath = Path.Combine(basePath, dir);
                Directory.CreateDirectory(fullPath);
                if (!bootMessagesRtb.IsDisposed && bootMessagesRtb.IsHandleCreated)
                {
                    bootMessagesRtb.AppendText($"[ OK ] Created: {fullPath}\n");
                    bootMessagesRtb.SelectionStart = bootMessagesRtb.TextLength;
                    bootMessagesRtb.ScrollToCaret();
                }

                if (bootMessagesRtb.IsHandleCreated)
                {
                    bootMessagesRtb.AppendText($"[ OK ] Created: {fullPath}\n");
                    bootMessagesRtb.SelectionStart = bootMessagesRtb.TextLength;
                    bootMessagesRtb.ScrollToCaret();
                }

                if (bootMessagesRtb != null && !bootMessagesRtb.IsDisposed && bootMessagesRtb.IsHandleCreated)
                {
                    bootMessagesRtb.SelectionStart = bootMessagesRtb.TextLength;
                    bootMessagesRtb.ScrollToCaret();
                }

                bootMessagesRtb.ScrollToCaret();
            }

            // Simula cópia de imagem de perfil
            /*
            string sourceImage = @"C:\SomePath\face.jpg"; // Substituir pelo caminho real
            string targetImage = Path.Combine(basePath, "usersettings/users/username/face/face.jpg");
            if (File.Exists(sourceImage))
            {
                File.Copy(sourceImage, targetImage, true);
                bootMessagesRtb.AppendText("[ OK ] User face image copied.\n");
                bootMessagesRtb.AppendText("[ OK ] Created: ...\n");
                bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                bootMessagesRtb.ScrollToCaret();
            }
            */
        }


        // Inject Directories Method 
        private string[] appDirectories = {

            // redo project dir treee
            // base dir
        "App\\Data",
        "App\\Data\\Virtual",
        "App\\Data\\Virtual\\RAM",
        "App\\Data\\Virtual\\ROM",
        "App\\Database",
        "App\\Initial",
        // flag if application is used for the first time, Yes/No,add json file do db
        "App\\Initial\\FirstUse\\",
        "App\\Initial\\FirstUse\\Settings",
        "App\\Temp",
        "App\\Shared",
        // set app settings with json file and store it here, first create directory,
        // then add json file here:
        "App\\Settings",
        "App\\Settings\\Theme",
        /*
        "App\\Settings\\Backups",
        "App\\Settings\\Backups\\Recovery",
        "App\\Settings\\Backups\\Recovery\\Events",
        "App\\Settings\\Backups\\Recovery\\Events\\Logs",
        */
        "App\\Shared\\Documents",
        "App\\Shared\\Music",
        "App\\Shared\\Pictures",
        "App\\Shared\\Downloads",
        "App\\Shared\\Media",
        "App\\Shared\\Temp",

        // Database
        // First save here upon usage,
        // then upon save o c:drive self destruct and schred on close if no longer needed
        
        "App\\Database\\Temp\\Backups",
        "App\\Database\\Temp\\Backups\\Recovery",
        "App\\Database\\Temp\\Backups\\Recovery\\Logs",
        "App\\Database\\Temp\\Backups\\Recovery\\SnapShots",

        // Account Manager
        // User Admin
        "App\\Users",
        "App\\Users\\Accounts",
        "App\\Users\\Accounts\\Admin",
        // to be created upon first login
        /*
        "App\\Users\\Accounts\\Admin\\Profile\\Files",
        "App\\Users\\Accounts\\Admin\\Profile\\Settings",
        "App\\Users\\Accounts\\Admin\\Documents",
        "App\\Users\\Accounts\\Admin\\Music",
        "App\\Users\\Accounts\\Admin\\Pictures",
        "App\\Users\\Accounts\\Admin\\Downloads",
        "App\\Users\\Accounts\\Admin\\Media",
        */

        // default

        "App\\Users\\Accounts\\Default",
        // to be created upon first login
        /*
        "App\\Users\\Accounts\\Default\\Profile\\Files",
        "App\\Users\\Accounts\\Default\\Profile\\Settings",
        "App\\Users\\Accounts\\Default\\Documents",
        "App\\Users\\Accounts\\Default\\Music",
        "App\\Users\\Accounts\\Default\\Pictures",
        "App\\Users\\Accounts\\Default\\Downloads",
        "App\\Users\\Accounts\\Default\\Media",
        */

        // Prep more  directories in the future here; // add your own logic here:

        };



       // private int dirIndex = 0;
       // private Timer directoryTimer;

       

        // Timer for periodic actions
        System.Timers.Timer timer;
        //int m, S;

        // Uptime  message settings
        private bool uptimeStopped = false;
        private int uptimeSeconds = 0;


        // Spinner animation variables
        private string[] spinnerFrames = { "|", "/", "-", " / " };
        //private int spinnerIndex = 0;

        private Timer spinnerTimer;
        //private int fileCheckIndex = 0;
        private string[] bootFiles = { "boot.sys", "kernel.img", "drivers.dll", "config.ini" };

        private string[] spinnerShutDownFrames = { "|", "/", "-", "/" };
        private int spinnerShutDownFramesIndex = 0;
        private Timer spinnerShutDownFramesTimer;

        private bool waitingForOk = false;

        // smooth background color transitiom
        private Timer colorBlendTimer;
        //private int blendStep;
        //private int blendSteps;
        private Color startColor;
        private Color endColor;
        //private bool directoriesFinalized = false;

        // 🌈 Campos para transição multicolorida
        private Color[] transitionColors;
        private int currentColorIndex;

        private Color destColor = Color.FromArgb(0, 120, 215); // Windows blue 



        // Custom RichTextBox with double buffering to reduce flicker
        public class SmoothRichTextBox : RichTextBox
        {
            public SmoothRichTextBox()
            {
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                //this.SetStyle(ControlStyles.UserPaint, true);
            }
        }
        Timer ritualTimer = new Timer();
        private void RitualTimer_Tick(object sender, EventArgs e)
        {
            ritualTimer.Stop();
            if (Theme != null)
            {
                HydraVoice.Speak($"Bem-vindo ao ritual Hydra. Tema atual: {Theme.Name}.");
                HydraVoice.NarrateMood(Theme.Mood);
            }
            else
            {
                HydraVoice.Speak("Bem-vindo ao ritual Hydra. Tema não definido.");
            }

        }

        public SplashScreen()
        {
            InitializeComponent();
            Panel.Hide();
            this.Load += SplashScreen_Load;

            // _terminal = new HydraTerminal(BootConsole); //cs0712
            // Iniatialize progress bar
            progressBar1.Visible = false;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = appDirectories.Length;
            progressBar1.Value = 0;
            // 🌈 Inicializar sequência de cores para transição
            transitionColors = new Color[]
            {
            Color.Black,
            Color.DarkSlateBlue,
            Color.MediumPurple,
            Color.DeepSkyBlue,
            Color.DodgerBlue,
            Color.FromArgb(0, 120, 215) // ← fim desejado? // Windows blue
            };

            currentColorIndex = 0;
            //blendStep = 0;
            //blendSteps = 100; // ← Aqui defines o número de passos

            startColor = transitionColors[currentColorIndex];
            endColor = transitionColors[currentColorIndex + 1];

            // ⏱️ Inicializar e configurar o timer
            /*
            colorBlendTimer = new Timer();
            colorBlendTimer.Interval = 100; // ← Aqui defines a velocidade
            colorBlendTimer.Tick += ColorBlendTimer_Tick;
            colorBlendTimer.Start();
            */
                    
        }
        

        private void btnCloseApp_Click(object sender, EventArgs e)
        {
            Close();
        }

        /*
        // end Form1 class
        // Background color change variables
        private Color[] bgColors = new Color[]
        {
            Color.FromArgb(0, 120, 215), // Windows blue
            Color.FromArgb(0, 153, 188), // Cyan-ish
            Color.FromArgb(51, 51, 51),  // Dark gray
            Color.Black
        };

        // Index to track current background color
      
        private Timer bgTimer;

        private bool showCursor = true;
        private Timer cursorBlinkTimer;
        private void BgTimer_Tick(object sender, EventArgs e)
        {
            // Example: fade background or animate something
            this.BackColor = Color.FromArgb(
            Math.Min(this.BackColor.R + 1, destColor.R),
            Math.Min(this.BackColor.G + 1, destColor.G),
            Math.Min(this.BackColor.B + 1, destColor.B)
         );

            // Optional: update a label or spinner
             Label1.Text = "HydraLife is waking up...";
        }

        */






        private async void SplashScreen_Load(object sender, EventArgs e)
        {
            // Exemplo de invocação
            AppendCeremonialMessage("Iniciando o sistema...");
            bootMessagesRtb.AppendText($"[VOZ ATIVA] {HydraVoice.GetCurrentVoice()}\r\n");

            this.Size = Screen.PrimaryScreen.Bounds.Size;
            this.BringToFront();
           
            DateTime startTime = DateTime.Now;
            startTimeFormatted = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //Theme = HydraThemeManager.GetCurrentTheme(); // ou qualquer método que inicialize o tema

            if (Theme != null)
            {
                this.BackColor = ColorTranslator.FromHtml(Theme.PrimaryColor ?? "#000000");


                this.Font = new Font(Theme.FontFamily ?? "Segoe UI", 10);

                // 🗣️ A Hydra fala ao despertar


                HydraVoice.Speak("Bem-vindo ao ritual Hydra. Tema atual: " + Theme.Name + ".");

                HydraVoice.NarrateMood(Theme.Mood);
            }
            if (!string.IsNullOrEmpty(Message))
            {


                Label lblMessage = new Label
                {
                    Text = "Bem-vindo ao ritual Hydra",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Location = new Point(20, this.ClientSize.Height - 50) // ajusta para aparecer no fundo
                };

                lblMessage.BringToFront(); // garante que não fica escondido
                this.Controls.Add(lblMessage);
                bootMessagesRtb.AppendText($"[MENSAGEM] {Message}\r\n");

               





            }

            // Música cerimonial
            if (!string.IsNullOrEmpty(MusicPath))
            {
                string ext = Path.GetExtension(MusicPath).ToLowerInvariant();

                if (ext == ".wav")
                {
                    try
                    {
                        var player = new System.Media.SoundPlayer(MusicPath);
                        player.Play();
                    }
                    catch
                    {
                        HydraVoice.Speak("Falha ao tocar música WAV.");
                    }
                }
                else if (ext == ".mp3")
                {
                    try
                    {
                        var wmp = new WMPLib.WindowsMediaPlayer();
                        wmp.URL = MusicPath;
                        wmp.controls.play();
                    }
                    catch
                    {
                        HydraVoice.Speak("Falha ao tocar música MP3.");
                        // how the humanity
                    }
                }
                else
                {
                    HydraVoice.Speak("Formato de música não suportado.");
                }   // doh, simpsons
            }

            // VisualStyle pode ser usado para aplicar cores, imagens, etc.


            
            Panel.Visible = false;


            #region Modular injection
            HydraMoodCycler.Start(this);
            //HydraThemeLoader.Apply(Theme);

            #endregion Modular injection

            #region Normalize visuals
            // fix control position and looks
            //this.BackColor = Color.FromArgb(0, 0, 64); // Azul escuro elegante

            // progressbar1 start switch case (divid) percentage directories lenght
            // make progressbar start and terminal panel shows upon picture box first appears
            // label1 certa com canto inferior esquerdo de terminal panel

            // start appended messages(directory creation, divid by swicth case to progressbar maximum = directories lenght)
            // upon full progress bar,buttons appear
            // make splash bread and give time for user stop app before login with await task
            // login shows,  this hide
            PictureBox2.Location = new Point(
                (this.ClientSize.Width - PictureBox2.Width) / 2,
                (this.ClientSize.Height - PictureBox2.Height) / 2
            );
            PictureBox2.Anchor = AnchorStyles.None;
            PictureBox2.BackColor = Color.Transparent;
            PictureBox2.BorderStyle = BorderStyle.None;
            PictureBox2.Image = LifeCicles.Properties.Resources.hydra;
            PictureBox2.Visible = true;
            PictureBox2.BringToFront();
            PictureBox2.Size = new Size(200, 200); // ajusta conforme estética desejada
            PictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

            #endregion Normalize visuals
            #region Logo slide animation
            Timer fadeTimer = new Timer { Interval = 30 };
            logoSlide = new Timer { Interval = 30 };

            int startX = -PictureBox2.Width;
            int targetX = (this.ClientSize.Width - PictureBox2.Width) / 2;
            PictureBox2.Location = new Point(startX, 100);

            logoSlide.Tick += (object s, EventArgs e2) =>
            {
                PictureBox2.Left += 5;
                if (PictureBox2.Left >= targetX)
                    ((Timer)s).Stop();
            };

            await Task.Delay(1000);
            logoSlide.Start();
            #endregion Logo slide animation

            #region Terminal visuals
            //CreateTerminalPanel();

            Panel terminalPanel = new Panel();
            terminalPanel.Size = new Size(1250, 337);
            int terminalTop = Math.Min(PictureBox2.Bottom + 60, this.ClientSize.Height - terminalPanel.Height - 20);
            terminalPanel.Location = new Point(
                (this.ClientSize.Width - terminalPanel.Width) / 2,
                terminalTop
            );
            // an elegance effect here maybe
            await Task.Delay(1500);

            this.Controls.Add(terminalPanel);
            //terminalPanel.flat style here
            terminalPanel.BringToFront();
            terminalPanel.BackColor = this.BackColor;

            terminalPanel.BorderStyle = BorderStyle.None;
            terminalPanel.AutoScroll = false;


            bootMessagesRtb = new SmoothRichTextBox();

            terminalPanel.Controls.Add(bootMessagesRtb);
            bootMessagesRtb.Dock = DockStyle.Fill;
            bootMessagesRtb.BackColor = this.BackColor;
            bootMessagesRtb.ForeColor = Color.LimeGreen;
            bootMessagesRtb.Font = new Font("Consolas", 10, FontStyle.Bold);
            bootMessagesRtb.Enabled = true;
            bootMessagesRtb.ReadOnly = true;
            bootMessagesRtb.ScrollBars = RichTextBoxScrollBars.None;
            bootMessagesRtb.HideSelection = true;
            bootMessagesRtb.WordWrap = true;

            bootMessagesRtb.AppendText($"[VOZ ATIVA] {HydraVoice.GetCurrentVoice()}\r\n");
            //void.splashscreen,AppendCeremonialMessage(message);
            // Música cerimonial
            if (!string.IsNullOrEmpty(MusicPath))
            {
                string ext = Path.GetExtension(MusicPath).ToLowerInvariant();

                if (ext == ".wav")
                {
                    try
                    {
                        var player = new System.Media.SoundPlayer(MusicPath);
                        player.Play();
                    }
                    catch (Exception ex)
                    {
                        HydraVoice.Speak("Falha ao tocar música WAV.");
                    }
                }
                else if (ext == ".mp3")
                {
                    try
                    {
                        var wmp = new WMPLib.WindowsMediaPlayer();
                        wmp.URL = MusicPath;
                        wmp.controls.play();
                    }
                    catch (Exception ex)
                    {
                        HydraVoice.Speak("Falha ao tocar música MP3.");
                    }
                }
                else
                {
                    HydraVoice.Speak("Formato de música não suportado.");
                }
            }




            #endregion Terminal visuals





            /* 1. Efeitos visuais iniciais
            TriggerBackgroundFade(ColorTranslator.FromHtml("#2196F3"));
            colorBlendTimer.Start();
            timer1.Start();
            StartCursorBlink();
            StartDirectoryCheck();
            InjectSystemDirectories();

            #region Appended messages to virtual console

            // 2. Logs e mensagens
            bootMessagesRtb.AppendText("[SYSTEM] Load sequence complete. Launching HydraLife System...\r\n");
            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
            bootMessagesRtb.SelectionLength = 0;
            bootMessagesRtb.ScrollToCaret();

            // Code lbl cursur position here
            lblCursor.BringToFront();
            lblCursor.Text = "Load Completed... ";
            bootMessagesRtb.AppendText("[ OK ] Bye...\n");
            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
            bootMessagesRtb.SelectionLength = 0;
            bootMessagesRtb.ScrollToCaret();

           


            if (cursorBlinkTimer != null)
            {
                cursorBlinkTimer.Stop();
                cursorBlinkTimer.Dispose();
            }

           
            

            // 4. Espera para splash respirar
            await Task.Delay(6000); // ⏳ tempo calibrado

          
            // 6. Login

            try
            {
                // await    task here


                LoginForm login = new LoginForm();
                login.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening LoginForm: " + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            


            
            
           
            
            
          


            bootMessagesRtb.AppendText("[ Checking system logs... ]\n");
           
            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
            bootMessagesRtb.SelectionLength = 0;
            bootMessagesRtb.ScrollToCaret();

            bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
            bootMessagesRtb.SelectionLength = 0;
            bootMessagesRtb.ScrollToCaret();

            bootMessagesRtb.AppendText("[ System logs verified successfully. ]\n");
            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
            bootMessagesRtb.SelectionLength = 0;
            bootMessagesRtb.ScrollToCaret();

            bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
            bootMessagesRtb.SelectionLength = 0;
            bootMessagesRtb.ScrollToCaret();

            bootMessagesRtb.AppendText("[ Checking file and directory integrity... ]\n");
            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
            bootMessagesRtb.SelectionLength = 0;
            bootMessagesRtb.ScrollToCaret();

            bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
            bootMessagesRtb.SelectionLength = 0;
            bootMessagesRtb.ScrollToCaret();

            bootMessagesRtb.AppendText("[ Integrity check complete. No missing files or directories. ]\n");
            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
            bootMessagesRtb.SelectionLength = 0;
            bootMessagesRtb.ScrollToCaret();

            bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
            bootMessagesRtb.SelectionLength = 0;
            bootMessagesRtb.ScrollToCaret();


            bgTimer = new Timer();
            bgTimer.Interval = 1000;
            bgTimer.Tick += BgTimer_Tick; // Make sure this method exists

            bgTimer.Start();
            cursorBlinkTimer = new Timer();
            cursorBlinkTimer.Interval = 500; // blink every half second
            cursorBlinkTimer.Tick += CursorBlinkTimer_Tick;
            cursorBlinkTimer.Start();


            // Iniatialize progress bar
            progressBar1.Visible = false;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = appDirectories.Length;
            progressBar1.Value = 0;

            // Hide the control buttons initially  on load
            btnCloseApp.Visible = false;
            btnEndSession.Visible = false;
            btnrestart.Visible = false;

            // set the app date time
            appStartTime = DateTime.Now;

            // Set up the spinner timer
            spinnerTimer = new Timer();
            spinnerTimer.Interval = 750; // Speed of animation (500ms)
            spinnerTimer.Tick += SpinnerTimer_Tick;
            spinnerTimer.Start();

            // Initialize and start the timer
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            // Make the form full screen
            this.Size = Screen.PrimaryScreen.Bounds.Size;
            this.BringToFront();

            
            LifeCicles.Properties.Settings.Default.Save();

            // Show the button after 10 seconds
            await Task.Delay(10000).ContinueWith(_ =>
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    btnCloseApp.Visible = true;
                    btnEndSession.Visible = true;
                    btnrestart.Visible = true;

                    // Optional: stop background animation and lock final state
                    bgTimer.Stop();
                    this.BackColor = Color.Black;
                    

                    // Recalculate spinner frame here
                    string spinner = spinnerFrames[spinnerIndex];
                    spinnerIndex = (spinnerIndex + 1) % spinnerFrames.Length;

                    lblTimer.Text += $"\r\nLoad completed. Showing login screen... ";
                    // ✅ Start directory creation sequence here
                    directoryTimer = new Timer();
                    directoryTimer.Interval = 3000; // 1 second per directory
                    directoryTimer.Tick += DirectoryTimer_Tick;
                    directoryTimer.Start();
                }));
            });

        }
        // 2️ Timer for ProgressBar + Cinematic Logs + Percentage

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Increment(10);
            int percent = progressBar1.Value;
            lblTimer.Text = $"Load progress: {percent}%";

            switch (percent)
            { // await task here maybe between schitch's
                case 30:
                    
                    bootMessagesRtb.AppendText("[ Verifying system integrity... ]\n");
                    bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                    bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                    bootMessagesRtb.ScrollToCaret();
                    TriggerBackgroundFade(Color.FromArgb(15, 15, 30)); // Deep blue for login
                    break;
                    
                case 60:
                    bootMessagesRtb.AppendText("[ Launching core services... ]\n");
                    bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                    bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                    bootMessagesRtb.ScrollToCaret();
                    break;
                case 90:
                    bootMessagesRtb.AppendText("[ Finalizing load sequence... ]\n");
                    bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                    bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                    bootMessagesRtb.ScrollToCaret();
                    break;
            }

            if (percent >= 100)
            {
                timer1.Stop();
                bootMessagesRtb.AppendText("[ Load complete. Redirecting to login... ]\n");
                bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                bootMessagesRtb.ScrollToCaret();

                /*
                LoginForm login = new LoginForm();
                login.Show();
                this.Hide();
                */



            ritualTimer.Tick += RitualTimer_Tick;
            ritualTimer.Start();




        }


    }


    /*
    private void StartCursorBlink()
    {
        cursorBlinkTimer = new Timer();
        cursorBlinkTimer.Interval = 500;
        cursorBlinkTimer.Tick += CursorBlinkTimer_Ticking;
        cursorBlinkTimer.Start();
    }

    private void CursorBlinkTimer_Ticking(object sender, EventArgs e)
    {
        showCursor = !showCursor;
        lblCursor.Text = showCursor ? "_" : " ";

    }

    // End Blinking Cursor in Virtual Terminal


    private void CursorBlinkTimer_Tick(object sender, EventArgs e)
    {
        showCursor = !showCursor;
        string cursor = showCursor ? "_" : " ";
        lblCursor.Text = cursor; // Use a Label at bottom of RichTextBox
    }
    private void StartDirectoryCheck()
    {
        directoryTimer = new Timer();
        directoryTimer.Interval = 750; // adjust speed if needed
        directoryTimer.Tick += DirectoryTimer_Tick;
        directoryTimer.Start();
    }
    private bool directoriesInitialized = false;

    private async void DirectoryTimer_Tick(object sender, EventArgs e)
    {
        if (dirIndex < appDirectories.Length)
        {
            progressBar1.Value = dirIndex + 1;

            string fullPath = Path.Combine(HydraDataPath, appDirectories[dirIndex]);

            try
            {
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                    bootMessagesRtb.AppendText($"[{DateTime.Now:HH:mm:ss}] [OK] Created directory: {fullPath}\r\n");
                    bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                    bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                    bootMessagesRtb.ScrollToCaret();
                    bootMessagesRtb.AppendText($"[OK] Created directory with success: Proceeding... {fullPath}\r\n");
                    bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                    bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                    bootMessagesRtb.ScrollToCaret();

                    TriggerBackgroundFade(Color.FromArgb(0, 30, 0)); // 🟢 green for success
                }
                else
                {
                    bool hasFiles = Directory.GetFiles(fullPath).Length > 0;
                    bool hasSubDirs = Directory.GetDirectories(fullPath).Length > 0;

                    if (hasFiles || hasSubDirs)
                    {
                        bootMessagesRtb.AppendText($"[SKIP] Directory already exists and is not empty. Skipping... {fullPath}\r\n");
                        bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                        bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                        bootMessagesRtb.ScrollToCaret();
                        TriggerBackgroundFade(Color.FromArgb(30, 30, 0)); // 🟠 amber for skip


                    }
                    else
                    {
                        bootMessagesRtb.AppendText($"[OK] Directory exists but was empty. Skiping... {fullPath}\r\n");
                        bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                        bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                        bootMessagesRtb.ScrollToCaret();
                        TriggerBackgroundFade(Color.FromArgb(0, 30, 30)); // 🔵 teal for empty but valid
                    }
                }

                progressBar1.Value = dirIndex + 1;
                dirIndex++;
            }
            catch (Exception ex)
            {
                bootMessagesRtb.AppendText($"[ERROR] Failed to process {fullPath}, aborting: {ex.Message}\r\n");
                bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                bootMessagesRtb.ScrollToCaret();
                TriggerBackgroundFade(Color.DarkRed); // 🔴 red for error
                dirIndex++;
            }
        }
        else if (!directoriesFinalized)
        {
            directoriesFinalized = true; // ✅ Prevent future triggers
            bootMessagesRtb.AppendText("All directories initialized. System ready.\r\n");
            bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
            bootMessagesRtb.ScrollToCaret();
            TriggerBackgroundFade(Color.FromArgb(20, 20, 20)); // neutral tone

            bootMessagesRtb.AppendText("[SYSTEM] Load sequence complete. Lauching HydraLife System...\r\n");

            lblCursor.Text = $"Load Completed... ";

            await Task.Delay(1000); // ⏳ pausa final

            if (cursorBlinkTimer != null)
            {
                cursorBlinkTimer.Stop();
            }

            lblCursor.Text = "_"; // Mantém o cursor visível


            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
            bootMessagesRtb.ScrollToCaret();


            TriggerBackgroundFade(Color.FromArgb(15, 15, 30)); // Deep blue for login

            directoryTimer.Stop();

            // ✨ Cinematic pause before showing buttons
            await Task.Delay(10000).ContinueWith(_ =>
             {
                 this.Invoke((MethodInvoker)(() =>
                 {
                     btnCloseApp.Visible = true;
                     btnEndSession.Visible = true;
                     btnrestart.Visible = true;

                     // Optional: lock background animation
                     bgTimer?.Stop();
                 }));
             });
        }





    }




    // Spinner animation update

    private void SpinnerTimer_Tick(object sender, EventArgs e)
    {
        if (fileCheckIndex < bootFiles.Length)
        {
            progressBar1.Value = Math.Min(dirIndex + 1, progressBar1.Maximum);

            string file = bootFiles[fileCheckIndex];


            if (!waitingForOk)
            {
                bootMessagesRtb.AppendText($"Checking {file}...\r\n");
                bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                bootMessagesRtb.ScrollToCaret();
                TriggerBackgroundFade(Color.FromArgb(10, 10, 30)); // bluish tone for scanning
                waitingForOk = true;
            }
            else
            {
                // Simulate error condition for specific files
                if (file.Contains("corrupt") || file == "boot.sys")
                {
                    bootMessagesRtb.AppendText($"[ERROR] Failed to load {file}...\r\n");
                    bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                    bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                    bootMessagesRtb.ScrollToCaret();
                    TriggerBackgroundFade(Color.DarkRed); // 🔴 dramatic red for failure
                }
                else
                {
                    bootMessagesRtb.AppendText($"[OK] {file} loaded successfully...\r\n");
                    bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                    bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                    bootMessagesRtb.ScrollToCaret();
                    TriggerBackgroundFade(Color.FromArgb(0, 30, 0)); // 🟢 dark green for success

                }

                fileCheckIndex++;
                waitingForOk = false;
            }

            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
            bootMessagesRtb.ScrollToCaret();
        }
        else
        {
            bootMessagesRtb.AppendText("System ready. Proceding to check app files and directories...\r\n");

            TriggerBackgroundFade(Color.FromArgb(20, 20, 20)); // neutral tone
            dirIndex = 0;
            StartDirectoryCheck(); // ← this must be called

            //spinnerTimer.Stop();
        }
    }



    private void TriggerBackgroundFade(Color targetColor)
    {
        startColor = this.BackColor;
        endColor = targetColor;
        blendStep = 0;
        blendSteps = 60;

        if (colorBlendTimer != null)
        {
            colorBlendTimer.Stop();
            colorBlendTimer.Dispose();
        }

        colorBlendTimer = new Timer();
        colorBlendTimer.Interval = 1000;
        colorBlendTimer.Tick += ColorBlendTimer_Tick;
        colorBlendTimer.Start();
        Console.WriteLine($"Current: {BackColor.R},{BackColor.G},{BackColor.B} → Target: {targetColor.R},{targetColor.G},{targetColor.B}");


    }

    private void ColorBlendTimer_Tick(object sender, EventArgs e)
    {
        if (blendStep <= blendSteps)
        {
            int r = startColor.R + (endColor.R - startColor.R) * blendStep / blendSteps;
            int g = startColor.G + (endColor.G - startColor.G) * blendStep / blendSteps;
            int b = startColor.B + (endColor.B - startColor.B) * blendStep / blendSteps;

            // Clamp values to valid range [0, 255]
            r = Math.Max(0, Math.Min(255, r));
            g = Math.Max(0, Math.Min(255, g));
            b = Math.Max(0, Math.Min(255, b));

            Color blendedColor = Color.FromArgb(r, g, b);
            this.BackColor = blendedColor;
            bootMessagesRtb.BackColor = blendedColor;
            blendStep++;
        }
        else
        {
            colorBlendTimer.Stop();
            colorBlendTimer.Dispose();

            this.BackColor = endColor;
            bootMessagesRtb.BackColor = endColor;

            if (endColor == Color.Black)
            {
                bootMessagesRtb.AppendText("\n[ SYSTEM ] Shutdown complete.\n");
                bootMessagesRtb.Cursor = Cursors.No;
            }
            else
            {
                StartDirectoryCheck();
            }

            Console.WriteLine($"Background fade complete → Final: {endColor.R},{endColor.G},{endColor.B}");
        }
    }


    // Timer elapsed event handler
    private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        Invoke(new Action(() =>
        {
            if (!uptimeStopped)
            {
                uptimeSeconds++;

                TimeSpan elapsed = DateTime.Now - appStartTime;
                string startTimeFormatted = appStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                string uptimeFormatted = $"{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";

                lblTimer.Text = $"Application started at: {startTimeFormatted}\r\n";
                label2.Text = $"Uptime: {uptimeFormatted}\r\n";

                if (uptimeSeconds >= 7)
                {
                    uptimeStopped = true;
                    label2.Text = $"Counting... {spinnerFrames[spinnerIndex]}";
                }
            }
            else
            {
                string spinner = spinnerFrames[spinnerIndex];
                spinnerIndex = (spinnerIndex + 1) % spinnerFrames.Length;
                label2.Text = $"Counting... {spinner}";
            }

            // Show one boot file per tick
            if (fileCheckIndex < bootFiles.Length)
            {
                string spinner = spinnerFrames[spinnerIndex];
                spinnerIndex = (spinnerIndex + 1) % spinnerFrames.Length;

                if (!waitingForOk)
                {
                    lblTimer.Text += $"Checking {bootFiles[fileCheckIndex]}... \r\n";
                    waitingForOk = true; // Wait for next tick to show [OK]
                }
                else
                {

                    lblTimer.Text += $"[OK]\r\n";

                    fileCheckIndex++;
                    waitingForOk = false;
                }
            }

        }));
    }





    // Button click event to close the application
    private void btnCloseApp_Click(object sender, EventArgs e)
    {

        // Stop background color changes
        bgTimer?.Stop();

        // Stop spinner updates
        spinnerTimer?.Stop();

        // Lock final background state
        this.BackColor = Color.Black;
        bootMessagesRtb.BackColor = Color.Black;
        bootMessagesRtb.ForeColor = Color.LimeGreen;

        // Show shutdown message

        Label1.Text = $"BIOS Clock has received signal to shutdown at: {startTimeFormatted}\r\n";




        //Label1.Text = $"BIOS clock as received signal to shutdown,.. system is shuting down now at:...\r\n";
        // replace by this:    // Label1.Text = $"BIOs as received signal to sdown at: {startTimeFormatted}\r\n";  throws error must be fixed

        // Let spinnerTimer keep running for animation
        Task.Delay(10000).ContinueWith(_ =>
        {


            this.Invoke((MethodInvoker)(() =>
            {

                spinnerTimer.Stop(); // Stop right before killing the app
                Process.GetCurrentProcess().Kill();
            }));
        });
    }
    */

}



        /*
        private void StartCursorBlink()
        {
            cursorBlinkTimer = new Timer();
            cursorBlinkTimer.Interval = 500;
            cursorBlinkTimer.Tick += CursorBlinkTimer_Ticking;
            cursorBlinkTimer.Start();
        }

        private void CursorBlinkTimer_Ticking(object sender, EventArgs e)
        {
            showCursor = !showCursor;
            lblCursor.Text = showCursor ? "_" : " ";

        }

        // End Blinking Cursor in Virtual Terminal


        private void CursorBlinkTimer_Tick(object sender, EventArgs e)
        {
            showCursor = !showCursor;
            string cursor = showCursor ? "_" : " ";
            lblCursor.Text = cursor; // Use a Label at bottom of RichTextBox
        }
        private void StartDirectoryCheck()
        {
            directoryTimer = new Timer();
            directoryTimer.Interval = 750; // adjust speed if needed
            directoryTimer.Tick += DirectoryTimer_Tick;
            directoryTimer.Start();
        }
        private bool directoriesInitialized = false;

        private async void DirectoryTimer_Tick(object sender, EventArgs e)
        {
            if (dirIndex < appDirectories.Length)
            {
                progressBar1.Value = dirIndex + 1;

                string fullPath = Path.Combine(HydraDataPath, appDirectories[dirIndex]);

                try
                {
                    if (!Directory.Exists(fullPath))
                    {
                        Directory.CreateDirectory(fullPath);
                        bootMessagesRtb.AppendText($"[{DateTime.Now:HH:mm:ss}] [OK] Created directory: {fullPath}\r\n");
                        bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                        bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                        bootMessagesRtb.ScrollToCaret();
                        bootMessagesRtb.AppendText($"[OK] Created directory with success: Proceeding... {fullPath}\r\n");
                        bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                        bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                        bootMessagesRtb.ScrollToCaret();

                        TriggerBackgroundFade(Color.FromArgb(0, 30, 0)); // 🟢 green for success
                    }
                    else
                    {
                        bool hasFiles = Directory.GetFiles(fullPath).Length > 0;
                        bool hasSubDirs = Directory.GetDirectories(fullPath).Length > 0;

                        if (hasFiles || hasSubDirs)
                        {
                            bootMessagesRtb.AppendText($"[SKIP] Directory already exists and is not empty. Skipping... {fullPath}\r\n");
                            bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                            bootMessagesRtb.ScrollToCaret();
                            TriggerBackgroundFade(Color.FromArgb(30, 30, 0)); // 🟠 amber for skip


                        }
                        else
                        {
                            bootMessagesRtb.AppendText($"[OK] Directory exists but was empty. Skiping... {fullPath}\r\n");
                            bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                            bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                            bootMessagesRtb.ScrollToCaret();
                            TriggerBackgroundFade(Color.FromArgb(0, 30, 30)); // 🔵 teal for empty but valid
                        }
                    }

                    progressBar1.Value = dirIndex + 1;
                    dirIndex++;
                }
                catch (Exception ex)
                {
                    bootMessagesRtb.AppendText($"[ERROR] Failed to process {fullPath}, aborting: {ex.Message}\r\n");
                    bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                    bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                    bootMessagesRtb.ScrollToCaret();
                    TriggerBackgroundFade(Color.DarkRed); // 🔴 red for error
                    dirIndex++;
                }
            }
            else if (!directoriesFinalized)
            {
                directoriesFinalized = true; // ✅ Prevent future triggers
                bootMessagesRtb.AppendText("All directories initialized. System ready.\r\n");
                bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                bootMessagesRtb.ScrollToCaret();
                TriggerBackgroundFade(Color.FromArgb(20, 20, 20)); // neutral tone

                bootMessagesRtb.AppendText("[SYSTEM] Load sequence complete. Lauching HydraLife System...\r\n");

                lblCursor.Text = $"Load Completed... ";

                await Task.Delay(1000); // ⏳ pausa final

                if (cursorBlinkTimer != null)
                {
                    cursorBlinkTimer.Stop();
                }

                lblCursor.Text = "_"; // Mantém o cursor visível


                bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                bootMessagesRtb.ScrollToCaret();


                TriggerBackgroundFade(Color.FromArgb(15, 15, 30)); // Deep blue for login

                directoryTimer.Stop();

                // ✨ Cinematic pause before showing buttons
                await Task.Delay(10000).ContinueWith(_ =>
                 {
                     this.Invoke((MethodInvoker)(() =>
                     {
                         btnCloseApp.Visible = true;
                         btnEndSession.Visible = true;
                         btnrestart.Visible = true;

                         // Optional: lock background animation
                         bgTimer?.Stop();
                     }));
                 });
            }





        }




        // Spinner animation update

        private void SpinnerTimer_Tick(object sender, EventArgs e)
        {
            if (fileCheckIndex < bootFiles.Length)
            {
                progressBar1.Value = Math.Min(dirIndex + 1, progressBar1.Maximum);

                string file = bootFiles[fileCheckIndex];
                

                if (!waitingForOk)
                {
                    bootMessagesRtb.AppendText($"Checking {file}...\r\n");
                    bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                    bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                    bootMessagesRtb.ScrollToCaret();
                    TriggerBackgroundFade(Color.FromArgb(10, 10, 30)); // bluish tone for scanning
                    waitingForOk = true;
                }
                else
                {
                    // Simulate error condition for specific files
                    if (file.Contains("corrupt") || file == "boot.sys")
                    {
                        bootMessagesRtb.AppendText($"[ERROR] Failed to load {file}...\r\n");
                        bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                        bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                        bootMessagesRtb.ScrollToCaret();
                        TriggerBackgroundFade(Color.DarkRed); // 🔴 dramatic red for failure
                    }
                    else
                    {
                        bootMessagesRtb.AppendText($"[OK] {file} loaded successfully...\r\n");
                        bootMessagesRtb.AppendText("[ OK ] Proceding: ...\n");
                        bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                        bootMessagesRtb.ScrollToCaret();
                        TriggerBackgroundFade(Color.FromArgb(0, 30, 0)); // 🟢 dark green for success

                    }

                    fileCheckIndex++;
                    waitingForOk = false;
                }

                bootMessagesRtb.SelectionStart = bootMessagesRtb.Text.Length;
                bootMessagesRtb.ScrollToCaret();
            }
            else
            {
                bootMessagesRtb.AppendText("System ready. Proceding to check app files and directories...\r\n");

                TriggerBackgroundFade(Color.FromArgb(20, 20, 20)); // neutral tone
                dirIndex = 0;
                StartDirectoryCheck(); // ← this must be called

                //spinnerTimer.Stop();
            }
        }


        
        private void TriggerBackgroundFade(Color targetColor)
        {
            startColor = this.BackColor;
            endColor = targetColor;
            blendStep = 0;
            blendSteps = 60;

            if (colorBlendTimer != null)
            {
                colorBlendTimer.Stop();
                colorBlendTimer.Dispose();
            }
            
            colorBlendTimer = new Timer();
            colorBlendTimer.Interval = 1000;
            colorBlendTimer.Tick += ColorBlendTimer_Tick;
            colorBlendTimer.Start();
            Console.WriteLine($"Current: {BackColor.R},{BackColor.G},{BackColor.B} → Target: {targetColor.R},{targetColor.G},{targetColor.B}");


        }
        
        private void ColorBlendTimer_Tick(object sender, EventArgs e)
        {
            if (blendStep <= blendSteps)
            {
                int r = startColor.R + (endColor.R - startColor.R) * blendStep / blendSteps;
                int g = startColor.G + (endColor.G - startColor.G) * blendStep / blendSteps;
                int b = startColor.B + (endColor.B - startColor.B) * blendStep / blendSteps;

                // Clamp values to valid range [0, 255]
                r = Math.Max(0, Math.Min(255, r));
                g = Math.Max(0, Math.Min(255, g));
                b = Math.Max(0, Math.Min(255, b));

                Color blendedColor = Color.FromArgb(r, g, b);
                this.BackColor = blendedColor;
                bootMessagesRtb.BackColor = blendedColor;
                blendStep++;
            }
            else
            {
                colorBlendTimer.Stop();
                colorBlendTimer.Dispose();

                this.BackColor = endColor;
                bootMessagesRtb.BackColor = endColor;

                if (endColor == Color.Black)
                {
                    bootMessagesRtb.AppendText("\n[ SYSTEM ] Shutdown complete.\n");
                    bootMessagesRtb.Cursor = Cursors.No;
                }
                else
                {
                    StartDirectoryCheck();
                }

                Console.WriteLine($"Background fade complete → Final: {endColor.R},{endColor.G},{endColor.B}");
            }
        }


        // Timer elapsed event handler
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                if (!uptimeStopped)
                {
                    uptimeSeconds++;

                    TimeSpan elapsed = DateTime.Now - appStartTime;
                    string startTimeFormatted = appStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    string uptimeFormatted = $"{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";

                    lblTimer.Text = $"Application started at: {startTimeFormatted}\r\n";
                    label2.Text = $"Uptime: {uptimeFormatted}\r\n";

                    if (uptimeSeconds >= 7)
                    {
                        uptimeStopped = true;
                        label2.Text = $"Counting... {spinnerFrames[spinnerIndex]}";
                    }
                }
                else
                {
                    string spinner = spinnerFrames[spinnerIndex];
                    spinnerIndex = (spinnerIndex + 1) % spinnerFrames.Length;
                    label2.Text = $"Counting... {spinner}";
                }

                // Show one boot file per tick
                if (fileCheckIndex < bootFiles.Length)
                {
                    string spinner = spinnerFrames[spinnerIndex];
                    spinnerIndex = (spinnerIndex + 1) % spinnerFrames.Length;

                    if (!waitingForOk)
                    {
                        lblTimer.Text += $"Checking {bootFiles[fileCheckIndex]}... \r\n";
                        waitingForOk = true; // Wait for next tick to show [OK]
                    }
                    else
                    {

                        lblTimer.Text += $"[OK]\r\n";

                        fileCheckIndex++;
                        waitingForOk = false;
                    }
                }

            }));
        }

       



        // Button click event to close the application
        private void btnCloseApp_Click(object sender, EventArgs e)
        {

            // Stop background color changes
            bgTimer?.Stop();

            // Stop spinner updates
            spinnerTimer?.Stop();

            // Lock final background state
            this.BackColor = Color.Black;
            bootMessagesRtb.BackColor = Color.Black;
            bootMessagesRtb.ForeColor = Color.LimeGreen;

            // Show shutdown message

            Label1.Text = $"BIOS Clock has received signal to shutdown at: {startTimeFormatted}\r\n";




            //Label1.Text = $"BIOS clock as received signal to shutdown,.. system is shuting down now at:...\r\n";
            // replace by this:    // Label1.Text = $"BIOs as received signal to sdown at: {startTimeFormatted}\r\n";  throws error must be fixed

            // Let spinnerTimer keep running for animation
            Task.Delay(10000).ContinueWith(_ =>
            {


                this.Invoke((MethodInvoker)(() =>
                {

                    spinnerTimer.Stop(); // Stop right before killing the app
                    Process.GetCurrentProcess().Kill();
                }));
            });
        }
        */




