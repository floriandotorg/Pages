using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pages
{
    public enum AnimationState
    {
        FadeIn,
        Visible,
        FadeOut
    }

    public class AnimationInfo
    {
        public static int NumFadingUpdates = 10;

        public AnimationState State;
        public SineValue Value;

        public double Linear
        {
            get
            {
                return Value.Linear;
            }
        }

        public AnimationInfo()
        {
            Value = new SineValue(1, NumFadingUpdates);
            FadeIn();
        }

        public void FadeIn()
        {
            State = AnimationState.FadeIn;
            Value.setMin();
            Value.Mode = ProessMode.SteepBeginSoftEnd;
        }

        public void FadeOut()
        {
            State = AnimationState.FadeOut;
            Value.setMax();
            Value.Mode = ProessMode.SoftBeginSteepEnd;
        }

        public void Visible()
        {
            State = AnimationState.Visible;
            Value.setMax();
        }
    }
}
