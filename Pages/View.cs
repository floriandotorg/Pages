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
        public Texture2D BackgroundTexture;
        public View Superview;
        public bool Visible;

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

        public Rectangle Bounds
        {
            get
            {
                return Viewport.Bounds;
            }
            set
            {
                Viewport viewport = Viewport;
                viewport.Bounds = value;
                Viewport = viewport;
            }
        }

        public Point Position
        {
            get
            {
                return new Point(Viewport.X, Viewport.Y);
            }
            set
            {
                Viewport viewport = Viewport;
                viewport.X = value.X;
                viewport.Y = value.Y;
                Viewport = viewport;
            }
        }

        public Point Size
        {
            get
            {
                return new Point(Viewport.Width, Viewport.Height);
            }
            set
            {
                Viewport viewport = Viewport;
                viewport.Width = value.X;
                viewport.Height = value.Y;
                Viewport = viewport;
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

        public View Overlay
        {
            get
            {
                return _overlay;
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

        public void PerformActionAfterDelay(Action action, TimeSpan delay)
        {
            NavigationController.PerformActionAfterDelay(action, delay);
        }

        public void Exit()
        {
            NavigationController.Exit();
        }

        #endregion

        private List<View> _subviews;
        private View _overlay;
        public AnimationInfo OverlayAnimationInfo;

        #region Game Interface

        public virtual void Initialize()
        {
            _subviews = new List<View>();
            BackgroundColor = Color.Transparent;
            NeedsRelayout = true;
            Visible = true;
        }

        public virtual void LoadContent()
        {
            if (BackgroundTexture == null)
            {
                BackgroundTexture = Load<Texture2D>("Rectangle");
            }

            foreach (View subview in _subviews)
            {
                subview.LoadContent();
            }
        }

        public virtual void UnloadContent()
        {
            if (_overlay != null)
            {
                _overlay.UnloadContent();
            }

            foreach (View subview in _subviews)
            {
                subview.UnloadContent();
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

        public virtual void Update(GameTime gameTime, AnimationInfo animationInfo)
        {
            foreach (View subview in _subviews)
            {
                subview.Update(gameTime, animationInfo);
            }

            if (_overlay != null)
            {
                if (OverlayAnimationInfo.State == AnimationState.FadeIn && OverlayAnimationInfo.Value.Inc())
                {
                    OverlayAnimationInfo.State = AnimationState.Visible;
                }
                else if (OverlayAnimationInfo.State == AnimationState.FadeOut && OverlayAnimationInfo.Value.Dec())
                {
                    doDismissOverlay();
                }

                if (_overlay != null)
                {
                    AnimationInfo overlayAnimationInfo = OverlayAnimationInfo;

                    if (animationInfo.State != AnimationState.Visible)
                    {
                        overlayAnimationInfo = animationInfo;
                    }

                    _overlay.Update(gameTime, overlayAnimationInfo);
                }
            }
        }

        protected void DrawLine(SpriteBatch batch, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);
            batch.Draw(Load<Texture2D>("Rectangle"), point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }

        public void Redraw(GameTime gameTime, AnimationInfo animationInfo)
        {
            if (Visible)
            {
                Draw(gameTime, animationInfo);

                foreach (View subview in _subviews)
                {
                    subview.Redraw(gameTime, animationInfo);
                }

                if (_overlay != null)
                {
                    AnimationInfo overlayAnimationInfo = OverlayAnimationInfo;

                    if (animationInfo.State != AnimationState.Visible)
                    {
                        overlayAnimationInfo = animationInfo;
                    }

                    _overlay.Redraw(gameTime, overlayAnimationInfo);
                }
            }
        }

        public virtual void Draw(GameTime gameTime, AnimationInfo animationInfo)
        {
            Rectangle viewportBounds = Viewport.Bounds;

            if (Superview != null)
            {
                viewportBounds = Superview.RectangleToSystem(viewportBounds);
            }

            SpriteBatch.Draw(BackgroundTexture, viewportBounds, BackgroundColor * animationInfo.Value);
        }

        public virtual void OverlayWillDimiss(View overlay)
        { }

        public virtual void OverlayDimissed(View overlay)
        { }

        public virtual Color ClearColor
        {
            get
            {
                return Color.Black;
            }
        }

        public virtual bool BackButtonPressed()
        {
            if (_overlay != null && _overlay.BackButtonPressed())
            {
                return true;
            }

            foreach (View subview in _subviews)
            {
                if (subview.BackButtonPressed())
                {
                    return true;
                }
            }

            return false;
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

        public virtual bool TouchDown(TouchLocation location)
        {
            if (_overlay != null && _overlay.Visible && _overlay.TouchInside(location) && _overlay.TouchDown(location))
            {
                return true;
            }
            
            foreach (View subview in _subviews)
            {
                if (subview.Visible && subview.TouchInside(location) && subview.TouchDown(location))
                {
                    return true;
                }
            }

            return false;
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

        public void ShowOverlay(View overlay, bool animated)
        {
            initializeView(overlay);
            overlay.LoadContent();
            _overlay = overlay;

            if (animated)
            {
                OverlayAnimationInfo = new AnimationInfo();
            }
            else
            {
                OverlayAnimationInfo = new AnimationInfo();
                OverlayAnimationInfo.Visible();
            }
        }

        private void doDismissOverlay()
        {
            View overlay = _overlay;

            _overlay = null;
            OverlayAnimationInfo = null;

            OverlayDimissed(overlay);
        }

        private void dismissOverlay(bool animated)
        {
            OverlayWillDimiss(_overlay);

            if (animated)
            {
                if (OverlayAnimationInfo.State == AnimationState.Visible)
                {
                    OverlayAnimationInfo.FadeOut();
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                doDismissOverlay();
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
