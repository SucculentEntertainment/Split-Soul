using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool showConsole = false;
    bool showHelp = false;

    string input = "";

    public List<object> commandList;
    public static DebugCommand<string> ChangeDimension;

    // ================================
    //  Events
    // ================================

    public void OnConsole(InputValue val)
    {
        showConsole = !showConsole;
    }

    public void OnReturn(InputValue val)
    {
        if (!showConsole) return;
        HandleInput();
        input = "";
    }

    public void OnEscape(InputValue val)
    {
        if (!showConsole) return;
        showConsole = false;
        input = "";
    }

    private void OnGUI()
    {
        if (!showConsole) return;

        float y = 0f;

        GUI.Box(new Rect(0, y, Screen.width, 32), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
    }

    // ================================
    //  Commands
    // ================================

    private void Awake()
    {
        ChangeDimension = new DebugCommand<string>("dimension", "Changes the current dimension to the specified one", "dimension <dimension>", (dim) =>
        {
            LevelManager.dimension = dim;
        });

        commandList = new List<object>
        {
            ChangeDimension,
        };
    }

    void HandleInput()
    {
        string[] args = input.Split(' ');

        for(int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
            if(input.Contains(commandBase.commandID))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if (commandList[i] as DebugCommand<string> != null)
                {
                    (commandList[i] as DebugCommand<string>).Invoke(args[1]);
                }
            }
        }
    }
}
