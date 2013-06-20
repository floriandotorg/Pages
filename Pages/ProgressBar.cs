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
    public class ProgressBar : View
    {
        public Color BarColor;

        public float Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = Math.Min(Math.Max(0f, value), 1f);
            }
        }

        private float _progress;

        public override void Initialize()
        {
            base.Initialize();

            Progress = .5f;
            BackgroundColor = Color.Gray;
            BarColor = Color.Red;
        }

        public override void Draw(GameTime gameTime, AnimationInfo animationInfo)
        {
            base.Draw(gameTime, animationInfo);
            int width = Convert.ToInt32(Convert.ToSingle(Viewport.Width) * Progress);
            SpriteBatch.Draw(Load<Texture2D>("Rectangle"), RectangleToSystem(new Rectangle(0, 0, width, Viewport.Height)), BarColor * animationInfo.Value);
        }
    }
}
