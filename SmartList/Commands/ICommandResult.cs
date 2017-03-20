using System;
using System.Windows.Input;

namespace SmartList
{
    public interface ICommandResult<T> : IEventCommand
    {
        T Result { get; }
    }
}
