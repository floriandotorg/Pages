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
        private Button _nextPageAnimatedButton, _nextPageButton, _overlayButton;
        private ProgressBar _progressBar;

        public override void Initialize()
        {
            base.Initialize();

            _label = new Label();
            AddSubview(_label);

            _nextPageAnimatedButton = new Button();
            AddSubview(_nextPageAnimatedButton);

            _nextPageButton = new Button();
            AddSubview(_nextPageButton);

            _overlayButton = new Button();
            AddSubview(_overlayButton);

            _progressBar = new ProgressBar();
            AddSubview(_progressBar);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _label.Text = "Hallo Welt";
            _label.Font = Load<SpriteFont>("TestFont");
            _label.BackgroundColor = Color.Black * 0;

            _nextPageAnimatedButton.Text = "Next Page Animated";
            _nextPageAnimatedButton.Font = Load<SpriteFont>("TestFont");
            _nextPageAnimatedButton.AutoResize = false;
            _nextPageAnimatedButton.BackgroundColor = Color.Red;
            _nextPageAnimatedButton.Tap += nextPageAnimated;

            _nextPageButton.Text = "Next Page";
            _nextPageButton.Font = Load<SpriteFont>("TestFont");
            _nextPageButton.AutoResize = false;
            _nextPageButton.BackgroundColor = Color.Green;
            _nextPageButton.Tap += nextPage;

            _overlayButton.Text = "Overlay";
            _overlayButton.Font = Load<SpriteFont>("TestFont");
            _overlayButton.AutoResize = false;
            _overlayButton.BackgroundColor = Color.Blue;
            _overlayButton.Tap += showOveray;

            updateProgressBar();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            CenterSubview(_label, -100);

            _nextPageAnimatedButton.Y = 200;
            _nextPageAnimatedButton.X = 200;
            _nextPageAnimatedButton.Height = 50;
            _nextPageAnimatedButton.Width = 200;

            _nextPageButton.Y = 260;
            _nextPageButton.X = 200;
            _nextPageButton.Height = 50;
            _nextPageButton.Width = 200;

            _overlayButton.Y = 200;
            _overlayButton.X = 400;
            _overlayButton.Height = 200;
            _overlayButton.Width = 200;

            _progressBar.Y = 100;
            _progressBar.X = 100;
            _progressBar.Height = 20;
            _progressBar.Width = 100;
        }

        void updateProgressBar()
        {
            if (_progressBar.Progress >= 1f)
            {
                _progressBar.Progress = 0f;
            }
            else
            {
                _progressBar.Progress = _progressBar.Progress + .1f;
            }
            
            NavigationController.PerformActionAfterDelay(updateProgressBar, TimeSpan.FromMilliseconds(500));
        }

        void nextPageAnimated(object sender)
        {
            NavigationController.Navigate(new InfoView(), true);
        }

        void nextPage(object sender)
        {
            NavigationController.Navigate(new InfoView(), false);
        }

        void showOveray(object sender)
        {
            Overlay(new Overlay(), true);
        }
    }
}
