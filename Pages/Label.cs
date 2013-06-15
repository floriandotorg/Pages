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
    public class Label : View
    {
        public int Pos;
        public Color Color;
        public Color BackgroundColor;
        public bool AutoResize;

        private string _text;
        private SpriteFont _font;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                if (AutoResize)
                {
                    autoResize();
                }
            }
        }

        public SpriteFont Font
        {
            get { return _font; }
            set
            {
                _font = value;
                if (AutoResize)
                {
                    autoResize();
                }
            }
        }

        public override Viewport Viewport
        {
            get { return base.Viewport; }
            set
            {
                base.Viewport = value;
                if (AutoResize)
                {
                    autoResize();
                }
            }
        }

        private void autoResize()
        {
            if (Font != null && Text != null)
            {
                Vector2 fontSize = Font.MeasureString(Text);

                Viewport viewport = Viewport;
                viewport.Height = Convert.ToInt32(fontSize.Y);
                viewport.Width = Convert.ToInt32(fontSize.X);
                base.Viewport = viewport;
            }   
        }

        public override void LoadContent()
        {
            AutoResize = true;
            Text = "";
            Pos = 0;
            Color = Color.White;
            BackgroundColor = Color.Black;
        }

        public override void Draw(GameTime gameTime)
        {
            NavigationController.SpriteBatch.Draw(Load<Texture2D>("Rectangle"), Viewport.Bounds, BackgroundColor);

            Vector2 position = new Vector2((Viewport.Width - Font.MeasureString(Text).X) / 2, (Viewport.Height - Font.MeasureString(Text).Y) / 2);
            SpriteBatch.DrawString(Font, Text, ConvertPoint(position), Color /** fadeProcess*/);
        }
    }
}
