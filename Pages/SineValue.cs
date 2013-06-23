using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private const double PI_2 = Math.PI / 2.0;

        public double Value
        {
            get
            {
                if (Mode == ProgressMode.SoftBeginSteepEnd)
                {
                    return _max * (1 - Math.Cos(_sineValue));
                }
                else
                {
                    return _max * Math.Sin(_sineValue);
                }
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
                return _sineValue >= PI_2;
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
            _inc = PI_2 / (double)steps;
            _sineValue = 0;
            Mode = ProgressMode.SteepBeginSoftEnd;
        }

        public bool Inc()
        {
            _sineValue = Math.Min(_sineValue + _inc, PI_2);
            return IsMax;
        }

        public bool Dec()
        {
            _sineValue = Math.Max(_sineValue - _inc, 0.0);
            return IsMin;
        }

        public void setMax()
        {
            _sineValue = PI_2;
        }

        public void setMin()
        {
            _sineValue = 0;
        }

        public double Linear
        {
            get
            {
                return _sineValue / PI_2;
            }
        }
    }
}
