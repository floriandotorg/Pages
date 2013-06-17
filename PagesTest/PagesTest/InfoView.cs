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
    public class InfoView : View
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

            _label.Text = "Info Page";
            _label.Font = Load<SpriteFont>("TestFont");
            _label.BackgroundColor = Color.Black * 0;
            CenterView(_label, -200);

            _button.Text = "Back";
            _button.Font = Load<SpriteFont>("TestFont");
            _button.AutoResize = false;
            _button.Y = 200;
            _button.Height = 200;
            _button.Width = 200;
            _button.Tap += _button_Tap;
        }

        void _button_Tap(object sender)
        {
            NavigationController.Back();
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
