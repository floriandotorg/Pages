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
using Pages;

namespace PagesTest
{
    class MenuView : View
    {
        private Label _label;
        private Button _button;

        public override void Initialize()
        {
            base.Initialize();

            _label = new Label();
            AddSubview(_label);

            _button = new Button();
            AddSubview(_button);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _label.Text = "Hallo Welt";
            _label.Font = Load<SpriteFont>("TestFont");
            _label.BackgroundColor = Color.Black * 0;
            CenterView(_label, -200);

            _button.Text = "Button";
            _button.Font = Load<SpriteFont>("TestFont");
            _button.Viewport.Y = 200;
            _button.Viewport.Height = 200;
            _button.Viewport.Width = 200;
            _button.Tap += _button_Tap;
        }

        void _button_Tap(object sender)
        {
            System.Diagnostics.Debug.WriteLine("_button_Tap");
        }

        public override bool Update(GameTime gameTime)
        {
            if (!base.Update(gameTime))
            {
                return false;
            }

            return true;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
