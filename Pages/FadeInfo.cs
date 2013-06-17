using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pages
{
    public enum FadingState
    {
        FadeIn,
        Viewing,
        FadeOut
    }

    public struct FadeInfo
    {
        public FadingState State;
        public SineValue Value;
    }
}
