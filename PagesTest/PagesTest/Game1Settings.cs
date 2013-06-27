using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pages;

namespace PagesTest
{
    class Game1Settings : GameSettings<Game1Settings>
    {
        protected override void Initialize()
        {
            base.Initialize();

            AddSetting("progress", 0f);
        }

        public static float Progress
        {
            get
            {
                return (float)Get("progress");
            }
            set
            {
                Set("progress", value);
            }
        }
    }
}
