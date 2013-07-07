using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pages
{
    public enum ProgressMode
    {
        SteepBeginSoftEnd, // Sin
        SoftBeginSteepEnd  // Cos
    }

    public class SineValue
    {
        public static implicit operator float(SineValue sineValue)
        {
            return (float)sineValue.Value;
        }

        public static implicit operator double(SineValue sineValue)
        {
            return sineValue.Value;
        }

        public ProgressMode Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                double backup = Value;
                _mode = value;
                Value = backup;
            }
        }

        private double _max;
        private double _sineValue;
        private double _inc;
        private ProgressMode _mode;

        public double Value
        {
            get
            {
                double result = 0;

                if (Mode == ProgressMode.SoftBeginSteepEnd)
                {
                    result = _max * (1 - Math.Cos(_sineValue));
                }
                else
                {
                    result = _max * Math.Sin(_sineValue);
                }

                return Math.Min(_max, Math.Max(0, result));
            }

            set
            {
                if (Mode == ProgressMode.SoftBeginSteepEnd)
                {
                    _sineValue = Math.Acos(1 - value / _max);
                }
                else
                {
                    _sineValue = Math.Asin(value / _max);
                }
            }
        }

        public bool IsMax
        {
            get
            {
                return _sineValue >= MathHelper.PiOver2;
            }
        }

        public bool IsMin
        {
            get
            {
                return _sineValue <= 0.0;
            }
        }

        public SineValue(double max, int steps)
        {
            _max = max;
            _inc = MathHelper.PiOver2 / (double)steps;
            _sineValue = 0;
            Mode = ProgressMode.SteepBeginSoftEnd;
        }

        public bool Inc()
        {
            _sineValue = Math.Min(_sineValue + _inc, MathHelper.PiOver2);
            return IsMax;
        }

        public bool Dec()
        {
            _sineValue = Math.Max(_sineValue - _inc, 0.0);
            return IsMin;
        }

        public void setMax()
        {
            _sineValue = MathHelper.PiOver2;
        }

        public void setMin()
        {
            _sineValue = 0;
        }

        public double Linear
        {
            get
            {
                return _sineValue / MathHelper.PiOver2;
            }
        }
    }
}
