using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool showConsole = false;
    bool showHelp = false;
    Vector2 scroll;

    string input = "";

    public List<object> commandList;
    public static DebugCommand Help;
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

        if(showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 128), "");

            Rect viewport = new Rect(0, y, Screen.width - 30, 20 * commandList.Count);
            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 114), scroll, viewport);

            for(int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;

                string label = $"{command.commandFormat} - {command.commandDescription}";
                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                GUI.Label(labelRect, label);
            }

            GUI.EndScrollView();

            y += 128;
        }

        GUI.Box(new Rect(0, y, Screen.width, 32), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
    }

    // ================================
    //  Commands
    // ================================

    private void Awake()
    {
        Help = new DebugCommand("help", "Shows a list of available commands", "help", () =>
        {
            showHelp = true;
        });

        ChangeDimension = new DebugCommand<string>("dimension", "Changes the current dimension to the specified one", "dimension <dimension>", (dim) =>
        {
            LevelManager.dimension = dim;
        });

        commandList = new List<object>
        {
            ChangeDimension,
            Help,
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
