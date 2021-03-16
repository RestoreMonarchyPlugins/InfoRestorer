using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adam.InfoRestorer
{
    public class InfoRestorerConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }
        public int MaxSavesPerPlayer { get; set; }
        public bool ShouldClearInventory { get; set; }
        public bool ShouldRemoveSavesOnLeave { get; set; }

        public void LoadDefaults()
        {
            MessageColor = "green";
            MaxSavesPerPlayer = 20;
            ShouldClearInventory = true;
            ShouldRemoveSavesOnLeave = true;
        }
    }
}
