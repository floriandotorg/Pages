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
        private const int NumFadingUpdates = 10;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ContentManager _contentManager;

        private bool _touching = false;
        private Stack<View> _navigationStack;
        private Dictionary<String, Object> _assetDictionary;
        private FadeInfo _fadeInfo;
        private View _navigateView;

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

        public NavigationController(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
        }

        virtual public void Initialize(View rootView)
        {
            _navigationStack = new Stack<View>();
            _assetDictionary = new Dictionary<String, Object>();
            _fadeInfo = new FadeInfo() { State = FadingState.FadeIn, Value = new SineValue(1, NumFadingUpdates) };

            _navigationStack.Push(rootView);

            InitializeView(rootView);
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

        private void HandleTouches()
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

        public void Back()
        {
            if (_fadeInfo.State == FadingState.Viewing)
            {
                _fadeInfo.State = FadingState.FadeOut;
                _fadeInfo.Value.Reverse();
                _navigateView = null;
            }
        }

        private void InitializeView(View view)
        {
            view.Viewport = _graphics.GraphicsDevice.Viewport;
            view.NavigationController = this;

            view.Initialize();
        }

        public void Navigate(View view)
        {
            if (_fadeInfo.State != FadingState.FadeOut)
            {
                while (!_fadeInfo.Value.Inc());

                _fadeInfo.State = FadingState.FadeOut;
                _fadeInfo.Value.Reverse();

                InitializeView(view);
                view.LoadContent();

                _navigateView = view;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        virtual public bool Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Back();
            }

            HandleTouches();

            if (_fadeInfo.State == FadingState.FadeIn && _fadeInfo.Value.Inc())
            {
                _fadeInfo.State = FadingState.Viewing;
            }
            else if (_fadeInfo.State == FadingState.FadeOut && _fadeInfo.Value.Dec())
            {
                if (_navigateView == null)
                {
                    _navigationStack.Pop();

                    if (_navigationStack.Count == 0)
                    {
                        return false;
                    }
                }
                else
                {
                    _navigationStack.Peek().PrepareForNavigation(_navigateView);
                    _navigationStack.Push(_navigateView);

                    _fadeInfo.State = FadingState.FadeIn;
                    _fadeInfo.Value.Reverse();
                    _navigateView = null;
                }

                _fadeInfo.State = FadingState.FadeIn;
                _fadeInfo.Value.Reverse();
            }

            return _navigationStack.Peek().Update(gameTime, _fadeInfo);
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

            _navigationStack.Peek().Draw(gameTime, _fadeInfo);

            _spriteBatch.End();
        }

        public T Load<T>(String assetName)
        {
            if (!_assetDictionary.Keys.Contains(assetName))
            {
                _assetDictionary.Add(assetName, _contentManager.Load<T>(assetName));
            }

            return (T)_assetDictionary[assetName];
        }
    }
}
