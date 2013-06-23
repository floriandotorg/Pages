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

        private Viewport _viewport;
        private bool _needsRelayout;

        #region Properties

        public virtual Viewport Viewport
        {
            get
            {
                return _viewport;
            }
            set
            {
                _viewport = value;
                NeedsRelayout = true;
            }
        }

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

        public bool NeedsRelayout
        {
            get
            {
                foreach (View subview in _subviews)
                {
                    if (subview.NeedsRelayout)
                    {
                        return true;
                    }
                }

                if (_overlay != null && _overlay.NeedsRelayout)
                {
                    return true;
                }

                return _needsRelayout;
            }
            set
            {
                _needsRelayout = value;
            }
        }

        #endregion

        #region NavigationController

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

        #endregion

        private List<View> _subviews;
        private View _overlay;
        private AnimationInfo _overlayAnimationInfo;

        #region Game Interface

        public virtual void Initialize()
        {
            _subviews = new List<View>();
            BackgroundColor = Color.Transparent;
            NeedsRelayout = true;
        }

        public virtual void LoadContent()
        {
            foreach (View subview in _subviews)
            {
                subview.LoadContent();
            }
        }

        public void Relayout()
        {
            if (_needsRelayout)
            {
                LayoutSubviews();
            }

            foreach (View subview in _subviews)
            {
                if (subview.NeedsRelayout)
                {
                    subview.Relayout();
                }
            }

            if (_overlay != null && _overlay.NeedsRelayout)
            {
                _overlay.Relayout();
            }

            NeedsRelayout = false;
        }

        public virtual void LayoutSubviews()
        { }

        public virtual bool Update(GameTime gameTime, AnimationInfo animationInfo)
        {
            foreach (View subview in _subviews)
            {
                if (!subview.Update(gameTime, animationInfo))
                {
                    return false;
                }
            }

            if (_overlay != null)
            {
                if (_overlayAnimationInfo.State == AnimationState.FadeIn && _overlayAnimationInfo.Value.Inc())
                {
                    _overlayAnimationInfo.State = AnimationState.Visible;
                }
                else if (_overlayAnimationInfo.State == AnimationState.FadeOut && _overlayAnimationInfo.Value.Dec())
                {
                    dismissOverlay(false);
                }

                if (_overlay != null && !_overlay.Update(gameTime, _overlayAnimationInfo))
                {
                    return false;
                }
            }

            return true;
        }

        protected void DrawLine(SpriteBatch batch, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);
            batch.Draw(Load<Texture2D>("Rectangle"), point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }

        public virtual void Draw(GameTime gameTime, AnimationInfo animationInfo)
        {
            Rectangle viewportBounds = Viewport.Bounds;

            if (Superview != null)
            {
                viewportBounds = Superview.RectangleToSystem(viewportBounds);
            }

            SpriteBatch.Draw(Load<Texture2D>("Rectangle"), viewportBounds, BackgroundColor * animationInfo.Value);

            foreach (View subview in _subviews)
            {
                subview.Draw(gameTime, animationInfo);
            }

            if (_overlay != null)
            {
                _overlay.Draw(gameTime, _overlayAnimationInfo);
            }
        }

        public virtual void OverlayDimissed(View overlay)
        { }

        public virtual Color ClearColor
        {
            get
            {
                return Color.Black;
            }
        }

        #endregion

        #region Subviews

        public void AddSubview(View view)
        {
            initializeView(view);
            _subviews.Add(view);
        }

        public void CenterSubview(View view, int yOffset)
        {
            view.X = (Viewport.Width - view.Viewport.Width) / 2;
            view.Y = (Viewport.Height - view.Viewport.Height) / 2 + yOffset;
        }

        #endregion

        #region Touch

        public virtual bool TouchInside(TouchLocation location)
        {
            Vector2 locale = location.Position;

            if (Superview != null)
            {
                locale = Superview.Vector2ToLocale(locale);
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

        #endregion

        #region Coordinate Conversation

        public Vector2 Vector2ToSystem(Vector2 vector)
        {
            Vector2 converted = new Vector2(Viewport.X + vector.X, Viewport.Y + vector.Y);

            if (Superview != null)
            {
                return Superview.Vector2ToSystem(converted);
            }
            else
            {
                return converted;
            }
        }

        public Vector2 Vector2ToLocale(Vector2 vector)
        {
            Vector2 converted = new Vector2(vector.X - Viewport.X, vector.Y - Viewport.Y);

            if (Superview != null)
            {
                return Superview.Vector2ToLocale(converted);
            }
            else
            {
                return converted;
            }
        }

        public Point PointToSystem(Point point)
        {
            return Utility.Vector2ToPoint(Vector2ToSystem(Utility.PointToVector2(point)));
        }

        public Point PointToLocale(Point point)
        {
            return Utility.Vector2ToPoint(Vector2ToLocale(Utility.PointToVector2(point)));
        }

        public Rectangle RectangleToSystem(Rectangle rectangle)
        {
            Point topLeft = PointToSystem(rectangle.Location);
            return new Rectangle(topLeft.X, topLeft.Y, rectangle.Width, rectangle.Height);
        }

        public Rectangle RectangleToLocale(Rectangle rectangle)
        {
            Point topLeft = PointToLocale(rectangle.Location);
            return new Rectangle(topLeft.X, topLeft.Y, rectangle.Width, rectangle.Height);
        }

        #endregion

        #region Overlay

        public void Overlay(View overlay, bool animated)
        {
            initializeView(overlay);
            overlay.LoadContent();
            _overlay = overlay;

            if (animated)
            {
                _overlayAnimationInfo = new AnimationInfo();
            }
            else
            {
                _overlayAnimationInfo = new AnimationInfo();
                _overlayAnimationInfo.Visible();
            }
        }

        private void dismissOverlay(bool animated)
        {
            if (animated)
            {
                if (_overlayAnimationInfo.State == AnimationState.Visible)
                {
                    _overlayAnimationInfo.FadeOut();
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                OverlayDimissed(_overlay);
                _overlay = null;
                _overlayAnimationInfo = null;
            }
        }

        public void Dismiss(bool animated)
        {
            if (Superview != null)
            {
                Superview.dismissOverlay(animated);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        #endregion

        #region Helper

        private void initializeView(View view)
        {
            view.Viewport = Viewport;
            view.NavigationController = NavigationController;
            view.Superview = this;
            view.Initialize();
        }

        #endregion
    }
}
