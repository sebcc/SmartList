using System;
using System.Windows.Input;

namespace SmartList
{
    public interface IEventCommand : ICommand
    {
        event EventHandler Executed;
    }
}
