using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;

namespace Pages
{
    public class GameSettings<T> where T : GameSettings<T>, new()
    {
        private static GameSettings<T> _instance;

        private static void Renew()
        {
            _instance = new T();
        }

        private static GameSettings<T> Instance
        {
            get 
            {
                if (_instance == null)
                {
                    Renew();
                }

                return _instance;
            }
        }

        private IsolatedStorageSettings _applicationSettings = IsolatedStorageSettings.ApplicationSettings;

        protected GameSettings()
        {
            try
            {
                Initialize();
            }
            catch
            {
                _applicationSettings.Clear();
                Initialize();
            }
        }

        protected static Object Get(string key)
        {
            try
            {
                return Instance._applicationSettings[key];
            }
            catch
            {
                Renew();
                return Instance._applicationSettings[key];
            }
        }

        protected static void Set(string key, Object value)
        {
            try
            {
                Instance._applicationSettings[key] = value;
            }
            catch
            {
                Renew();
                Instance._applicationSettings[key] = value;
            }
        }

        protected void AddSetting(string key, Object defaultValue)
        {
            if (!_applicationSettings.Contains(key))
            {
                _applicationSettings.Add(key, defaultValue);
            }
        }

        protected virtual void Initialize()
        { }
    }
}
