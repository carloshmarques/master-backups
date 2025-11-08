using Newtonsoft.Json;
using LifeCicles.Modules.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeCicles.Modules.Themes
{
    public class ThemeManifest
    {
        public string Name { get; set; }
        public string Mood { get; set; }
        [JsonProperty("primaryColor")]
        public string PrimaryColor { get; set; }

    
       


        public string Font { get; set; }
        public string BackgroundImage { get; set; }
        public bool IsCeremonial { get; set; }
        public string FontFamily { get; set; }
       

        public string Sound { get; set; }
        public string Video { get; set; }
    }


}
