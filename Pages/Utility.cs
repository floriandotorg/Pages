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
    public class Utility
    {
        public static Point Vector2ToPoint(Vector2 vector)
        {
            return new Point(Convert.ToInt32(vector.X), Convert.ToInt32(vector.Y));
        }

        public static Vector2 PointToVector2(Point point)
        {
            return new Vector2(Convert.ToSingle(point.X), Convert.ToSingle(point.Y));
        }
    }
}
