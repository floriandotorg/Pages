using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Pages
{
    public class Button : Label
    {
        public delegate void TapHandler(object sender);
        public event TapHandler Tap;

        public override bool TouchDown(TouchLocation location)
        {
            if (!base.TouchDown(location))
            {
                Tap(this);
            }

            return true;
        }
    }
}
