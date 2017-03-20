using System;
namespace SmartList
{
    public interface IApplicationState
    {
        void SetState (bool isAlive);

        bool GetState ();
    }
}
