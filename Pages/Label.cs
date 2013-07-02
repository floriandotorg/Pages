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
    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right
    }

    public enum VerticalAlignment
    {
        Top,
        Center,
        Bottom
    }

    public class Label : View
    {
        public HorizontalAlignment HorizontalAlignment;
        public VerticalAlignment VerticalAlignment;
        public Color Color;
        public bool AutoResize;

        private string _text;
        private SpriteFont _font;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                LayoutSubviews();
            }
        }

        public SpriteFont Font
        {
            get { return _font; }
            set
            {
                _font = value;
                LayoutSubviews();
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (AutoResize && Font != null && Text != null)
            {
                Vector2 fontSize = Font.MeasureString(Text);
                Height = Convert.ToInt32(fontSize.Y);
                Width = Convert.ToInt32(fontSize.X);
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Center;
            AutoResize = true;
            Text = "";
            Color = Color.White;
        }

        public override void Draw(GameTime gameTime, AnimationInfo animationInfo)
        {
            base.Draw(gameTime, animationInfo);

            if (!String.IsNullOrEmpty(Text) && Font != null)
            {
                Vector2 textSize = Font.MeasureString(Text);

                Vector2 position = Vector2.Zero;

                if (HorizontalAlignment == HorizontalAlignment.Center)
                {
                    position.X = (Viewport.Width - textSize.X) / 2;
                }
                else if (HorizontalAlignment == HorizontalAlignment.Right)
                {
                    position.X = Viewport.Width - textSize.X;
                }

                if (VerticalAlignment == VerticalAlignment.Center)
                {
                    position.Y = (Viewport.Height - textSize.Y) / 2;
                }
                else if (VerticalAlignment == VerticalAlignment.Bottom)
                {
                    position.Y = Viewport.Height - textSize.Y;
                }

                SpriteBatch.DrawString(Font, Text, Vector2ToSystem(position), Color * animationInfo.Value);
            }
        }
    }
}
