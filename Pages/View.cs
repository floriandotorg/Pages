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
        public View Superview;

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
        private View _overlay;

        private void InitializeView(View view)
        {
            view.Viewport = Viewport;
            view.NavigationController = NavigationController;
            view.Superview = this;
            view.Initialize();
        }

        public void AddSubview(View view)
        {
            InitializeView(view);
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

            if (_overlay != null)
            {
                if (!_overlay.Update(gameTime, fadeInfo))
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

            if (_overlay != null)
            {
                _overlay.Draw(gameTime, fadeInfo);
            }
        }

        public virtual bool TouchInside(TouchLocation location)
        {
            //System.Diagnostics.Debug.WriteLine("TouchInside " + location.Position + " " + PointToLocale(location.Position) + " " + Viewport.Bounds + " " + Viewport.Bounds.Contains(Utility.Vector2ToPoint(PointToLocale(location.Position))));

            Vector2 locale = location.Position;

            if (Superview != null)
            {
                locale = Superview.PointToLocale(locale);
            }

            return Viewport.Bounds.Contains(Utility.Vector2ToPoint(locale)); 
        }

        public virtual void TouchDown(TouchLocation location)
        {
            if (_overlay != null && _overlay.TouchInside(location))
            {
                _overlay.TouchDown(location);
            }
            else
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
        }

        virtual public Color ClearColor
        {
            get
            {
                return Color.Black;
            }
        }

        public Vector2 PointToSystem(Vector2 vector)
        {
            Vector2 converted = new Vector2(Viewport.X + vector.X, Viewport.Y + vector.Y);

            if (Superview != null)
            {
                return Superview.PointToSystem(converted);
            }
            else
            {
                return converted;
            }
        }

        public Vector2 PointToLocale(Vector2 vector)
        {
            Vector2 converted = new Vector2(vector.X - Viewport.X, vector.Y - Viewport.Y);

            if (Superview != null)
            {
                return Superview.PointToLocale(converted);
            }
            else
            {
                return converted;
            }
        }

        public void CenterSubview(View view, int yOffset)
        {
            view.X = (Viewport.Width - view.Viewport.Width) / 2;
            view.Y = (Viewport.Height - view.Viewport.Height) / 2 + yOffset;
        }

        public virtual void PrepareForOverlay(View overlay)
        { }

        public void Overlay(View overlay)
        {
            InitializeView(overlay);
            overlay.LoadContent();
            PrepareForOverlay(_overlay);
            _overlay = overlay;
        }

        private void DismissOverlay()
        {
            _overlay = null;
        }

        public void Dismiss()
        {
            if (Superview != null)
            {
                Superview.DismissOverlay();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
