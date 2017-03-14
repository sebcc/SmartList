using System;
namespace SmartList
{
    public interface ILocalNotification
    {
        void Notify (string title, string message);
    }
}
