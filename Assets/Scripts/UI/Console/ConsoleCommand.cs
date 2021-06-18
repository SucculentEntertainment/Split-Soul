using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConsoleCommandBase
{
    private string _commandID;
    private string _commandDescription;
    private string _commandFormat;

    public string commandID { get { return _commandID; } }
    public string commandDescription { get { return _commandDescription; } }
    public string commandFormat { get { return _commandFormat; } }

    public ConsoleCommandBase(string id, string description, string foramt)
    {
        _commandID = id;
        _commandDescription = description;
        _commandFormat = foramt;
    }
};

public class ConsoleCommand : ConsoleCommandBase
{
    private Action command;

    public ConsoleCommand(string id, string description, string format, Action command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
};

public class ConsoleCommand<T1> : ConsoleCommandBase
{
    private Action<T1> command;

    public ConsoleCommand(string id, string description, string format, Action<T1> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T1 val)
    {
        command.Invoke(val);
    }
};

public class ConsoleCommand<T1, T2> : ConsoleCommandBase
{
    private Action<T1, T2> command;

    public ConsoleCommand(string id, string description, string format, Action<T1, T2> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T1 val1, T2 val2)
    {
        command.Invoke(val1, val2);
    }
};
