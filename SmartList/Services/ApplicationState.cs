using System;
using Xamarin.Forms;

namespace SmartList
{
    public class ApplicationState : IApplicationState
    {
        private object applicationStateLock = new object ();

        public bool GetState ()
        {
            object isAlive;
            lock (this.applicationStateLock)
            {
                if (!Application.Current.Properties.TryGetValue ("Alive", out isAlive))
                {
                    return false;
                }
            }

            return (bool)isAlive;
        }

        public void SetState (bool isAlive)
        {
            lock (this.applicationStateLock)
            {
                if (Application.Current.Properties.ContainsKey ("Alive"))
                {
                    Application.Current.Properties ["Alive"] = isAlive;
                }
                else
                {
                    Application.Current.Properties.Add ("Alive", isAlive);
                }
            }
        }
    }
}
