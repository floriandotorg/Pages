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
    public class View
    {
        public NavigationController NavigationController;
        public Color BackgroundColor;

        public virtual Viewport Viewport { get; set; }

        public int Height
        {
            get
            {
                return Viewport.Height;
            }
            set
            {
                Viewport viewport = Viewport;
                viewport.Height = value;
                Viewport = viewport;
            }
        }

        public int Width
        {
            get
            {
                return Viewport.Width;
            }
            set
            {
                Viewport viewport = Viewport;
                viewport.Width = value;
                Viewport = viewport;
            }
        }

        public int X
        {
            get
            {
                return Viewport.X;
            }
            set
            {
                Viewport viewport = Viewport;
                viewport.X = value;
                Viewport = viewport;
            }
        }

        public int Y
        {
            get
            {
                return Viewport.Y;
            }
            set
            {
                Viewport viewport = Viewport;
                viewport.Y = value;
                Viewport = viewport;
            }
        }

        public SpriteBatch SpriteBatch
        {
            get
            {
                return NavigationController.SpriteBatch;
            }
        }

        public T Load<T>(String assetName)
        {
            return NavigationController.Load<T>(assetName);
        }

        private List<View> _subviews;

        public void AddSubview(View view)
        {
            view.Viewport = Viewport;
            view.NavigationController = NavigationController;
            view.Initialize();
            _subviews.Add(view);
        }

        public virtual void Initialize()
        {
            _subviews = new List<View>();
            BackgroundColor = Color.Transparent;
        }

        public virtual void LoadContent()
        {
            foreach (View subview in _subviews)
            {
                subview.LoadContent();
            }
        }

        public virtual bool Update(GameTime gameTime, FadeInfo fadeInfo)
        {
            foreach (View subview in _subviews)
            {
                if (!subview.Update(gameTime, fadeInfo))
                {
                    return false;
                }
            }

            return true;
        }

        virtual public void PrepareForNavigation(View destination)
        { }

        public virtual void Draw(GameTime gameTime, FadeInfo fadeInfo)
        {
            SpriteBatch.Draw(Load<Texture2D>("Rectangle"), Viewport.Bounds, BackgroundColor * fadeInfo.Value);

            foreach (View subview in _subviews)
            {
                subview.Draw(gameTime, fadeInfo);
            }
        }

        public virtual bool TouchInside(TouchLocation location)
        {
            return Viewport.Bounds.Contains(Utility.Vector2ToPoint(location.Position));
        }

        public virtual void TouchDown(TouchLocation location)
        {
            foreach (View subview in _subviews)
            {
                if (subview.TouchInside(location))
                {
                    subview.TouchDown(location);
                    break;
                }
            }
        }

        virtual public Color ClearColor
        {
            get
            {
                return Color.Black;
            }
        }

        public Vector2 ConvertPoint(Vector2 vector)
        {
            return new Vector2(Viewport.X + vector.X, Viewport.Y + vector.Y);
        }

        public void CenterView(View view, int yOffset)
        {
            view.X = (Viewport.Width - view.Viewport.Width) / 2;
            view.Y = (Viewport.Height - view.Viewport.Height) / 2 + yOffset;
        }
    }
}
