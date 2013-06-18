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
        private Button _button, _overlayButton;

        public override void Initialize()
        {
            base.Initialize();

            _label = new Label();
            AddSubview(_label);

            _button = new Button();
            AddSubview(_button);

            _overlayButton = new Button();
            AddSubview(_overlayButton);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _label.Text = "Hallo Welt";
            _label.Font = Load<SpriteFont>("TestFont");
            _label.BackgroundColor = Color.Black * 0;
            CenterSubview(_label, -100);

            _button.Text = "Next Page";
            _button.Font = Load<SpriteFont>("TestFont");
            _button.AutoResize = false;
            _button.Y = 200;
            _button.X = 200;
            _button.Height = 200;
            _button.Width = 200;
            _button.Tap += _button_Tap;

            _overlayButton.Text = "Overlay";
            _overlayButton.Font = Load<SpriteFont>("TestFont");
            _overlayButton.AutoResize = false;
            _overlayButton.Y = 200;
            _overlayButton.X = 400;
            _overlayButton.Height = 200;
            _overlayButton.Width = 200;
            _overlayButton.Tap += showOveray;
        }

        void _button_Tap(object sender)
        {
            NavigationController.Navigate(new InfoView());
        }

        void showOveray(object sender)
        {
            Overlay(new Overlay());
        }

        public override bool Update(GameTime gameTime, FadeInfo fadeInfo)
        {
            if (!base.Update(gameTime, fadeInfo))
            {
                return false;
            }

            return true;
        }

        public override void Draw(GameTime gameTime, FadeInfo fadeInfo)
        {
            base.Draw(gameTime, fadeInfo);
        }
    }
}
