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
    public class Overlay : View
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

            BackgroundColor = Color.Red * .8f;
            
            _label.Text = "This is Overlay";
            _label.Font = Load<SpriteFont>("TestFont");
            _label.BackgroundColor = Color.Transparent;
            
            _button.Text = "Dismiss";
            _button.Font = Load<SpriteFont>("TestFont");
            _button.AutoResize = false;
            _button.Tap += _button_Tap;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            Height = (int)(Superview.Height * .8f);
            Width = (int)(Superview.Width * .8f);
            Superview.CenterSubview(this, 0);

            CenterSubview(_label, -50);

            _button.Height = 200;
            _button.Width = 200;
            CenterSubview(_button, 0);
        }

        void _button_Tap(object sender)
        {
            Dismiss(true);
        }
    }
}
