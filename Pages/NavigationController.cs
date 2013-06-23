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
    public class NavigationController
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ContentManager _contentManager;

        private bool _touching = false;
        private Stack<View> _navigationStack;
        private Dictionary<String, Object> _assetDictionary;
        private AnimationInfo _animationInfo;
        private View _navigateView;

        private struct Timer
        {
            public TimeSpan Time;
            public Action Action;
        }

        private TimeSpan _totalGameTime;
        private List<Timer> _timerList;

        #region Properties
        
        public GraphicsDeviceManager Graphics
        {
            get
            {
                return _graphics;
            }
        }

        public SpriteBatch SpriteBatch
        {
            get
            {
                return _spriteBatch;
            }
        }

        #endregion

        #region Game Interface

        public NavigationController(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
        }

        virtual public void Initialize(View rootView)
        {
            _navigationStack = new Stack<View>();
            _assetDictionary = new Dictionary<String, Object>();
            _animationInfo = new AnimationInfo();
            _timerList = new List<Timer>();

            _navigationStack.Push(rootView);

           initializeView(rootView);
        }

        virtual public void LoadContent(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            _spriteBatch = spriteBatch;
            _contentManager = contentManager;

            Texture2D rectTexture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            rectTexture.SetData(new[] { Color.White });
            _assetDictionary.Add("Rectangle", rectTexture);

            _navigationStack.Peek().LoadContent();
        }

        virtual public void UnloadContent()
        {

        }

        virtual public bool Update(GameTime gameTime)
        {
            if (gameTime.IsRunningSlowly)
            {
                System.Diagnostics.Debug.WriteLine("Warning: gameTime is running slowly!");
            }

            _totalGameTime = gameTime.TotalGameTime;

            handleTimer();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Back(true);
            }

            handleTouches();

            if (_animationInfo.State == AnimationState.FadeIn && _animationInfo.Value.Inc())
            {
                _animationInfo.State = AnimationState.Visible;
            }
            else if (_animationInfo.State == AnimationState.FadeOut && _animationInfo.Value.Dec())
            {
                if (_navigateView == null)
                {
                    _navigationStack.Pop();
                }
                else
                {
                    doNavigate();
                }

                _animationInfo.FadeIn();
            }


            if (_navigationStack.Count != 0)
            {
                bool result = _navigationStack.Peek().Update(gameTime, _animationInfo);

                if (_navigationStack.Peek().NeedsRelayout)
                {
                    _navigationStack.Peek().Relayout();
                }

                return result;
            }
            else
            {
                return false;
            }
        }

        virtual public Color ClearColor
        {
            get
            {
                return _navigationStack.Peek().ClearColor;
            }
        }

        virtual public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            _navigationStack.Peek().Draw(gameTime, _animationInfo);

            _spriteBatch.End();
        }

        #endregion

        #region Timer

        private void handleTimer()
        {
            foreach (Timer timer in _timerList.Where(x => _totalGameTime >= x.Time).ToArray())
            {
                timer.Action();
                _timerList.Remove(timer);
            }
        }

        public void PerformActionAfterDelay(Action action, TimeSpan delay)
        {
            _timerList.Add(new Timer() { Action = action, Time = _totalGameTime.Add(delay) });
        }

        #endregion

        #region Navigation

        public void Back(bool animated)
        {
            if (_animationInfo.State == AnimationState.Visible)
            {
                _animationInfo.FadeOut();
                _navigateView = null;

                if (!animated)
                {
                    _animationInfo.Visible();
                    _navigationStack.Pop();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Navigate(View view, bool animated)
        {
            if (_animationInfo.State != AnimationState.FadeOut)
            {
                while (!_animationInfo.Value.Inc()) ;

                initializeView(view);
                view.LoadContent();
                _navigateView = view;

                if (animated)
                {
                    _animationInfo.FadeOut();
                }
                else
                {
                    _animationInfo.State = AnimationState.Visible;
                    doNavigate();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private void doNavigate()
        {
            _navigationStack.Push(_navigateView);
            _navigateView = null;
        }

        #endregion

        #region Helper

        private void handleTouches()
        {
            TouchCollection touches = TouchPanel.GetState();

            if (!_touching && touches.Count > 0)
            {
                _touching = true;

                if (_navigationStack.Peek().TouchInside(touches.First()))
                {
                    _navigationStack.Peek().TouchDown(touches.First());
                }

            }
            else if (touches.Count == 0)
            {
                _touching = false;
            }
        }

        private void initializeView(View view)
        {
            view.Viewport = _graphics.GraphicsDevice.Viewport;
            view.NavigationController = this;

            view.Initialize();
        }

        public T Load<T>(String assetName)
        {
            if (!_assetDictionary.Keys.Contains(assetName))
            {
                _assetDictionary.Add(assetName, _contentManager.Load<T>(assetName));
            }

            return (T)_assetDictionary[assetName];
        }

        #endregion
    }
}
