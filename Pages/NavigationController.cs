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


            _navigationStack.Push(rootView);

            rootView.Viewport = _graphics.GraphicsDevice.Viewport;
            rootView.NavigationController = this;

            rootView.Initialize();
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

        virtual public bool Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                _navigationStack.Pop();

                if (_navigationStack.Count == 0)
                {
                    return false;
                }
            }

            HandleTouches();

            return _navigationStack.Peek().Update(gameTime);
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

            _navigationStack.Peek().Draw(gameTime);

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
