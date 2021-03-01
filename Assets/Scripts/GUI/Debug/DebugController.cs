using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    public Camera cam;
    public Font font;

    public Transform player;
    public Transform enemies;
    public Transform npcs;

    bool showConsole = false;
    bool showHelp = false;
    bool showIDs = false;
    private bool showUnknown = false;

    private string unknownCommand = "";

    Vector2 scroll;
    string input = "";

    public List<object> commandList;

    public static DebugCommand Help;
    public static DebugCommand<string> Kill;
    public static DebugCommand<string> Revive;
    public static DebugCommand<string, float> Damage;
    public static DebugCommand<string, float> Heal;
    public static DebugCommand<string> ChangeDimension;
    public static DebugCommand IDs;
    public static DebugCommand Paths;

    // ================================
    //  Events
    // ================================
    public void OnConsole(InputValue val)
    {
        showConsole = !showConsole;
        showHelp = false;
        showUnknown = false;
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
        showHelp = false;
        showUnknown = false;
        input = "";
    }

    private void OnGUI()
    {
        if (showIDs) drawIDs();
        if (!showConsole) return;

        GUI.skin.font = font;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;

        float y = 0f;
        if (showHelp) y = drawHelp(y);

        GUI.Box(new Rect(0, y, Screen.width, 32), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
        y += 32;

        if (showUnknown) y = drawUnknown(y);
    }

    private float drawHelp(float y)
	{
        GUI.Box(new Rect(0, y, Screen.width, 128), "");

        Rect viewport = new Rect(0, y, Screen.width - 30, 20 * commandList.Count);
        scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 114), scroll, viewport);

        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase command = commandList[i] as DebugCommandBase;

            string label = $"{command.commandFormat}";
            Rect cmdRect = new Rect(5, 20 * i, (viewport.width - 100) / 3, 20);
            GUI.Label(cmdRect, label);

            label = $"{command.commandDescription}";
            Rect labelRect = new Rect(5 + (viewport.width - 100) / 3, 20 * i, (viewport.width - 100) / 3 * 2, 20);
            GUI.Label(labelRect, label);
        }

        GUI.EndScrollView();

        return y + 128;
    }

    private float drawUnknown(float y)
    {
        GUI.color = Color.red;
        GUI.Box(new Rect(0, y, Screen.width, 32), "Unknown command: " + unknownCommand);
        GUI.color = Color.white;

        return y + 32;
    }

    private void drawIDs()
	{
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;

        GUI.color = Color.cyan;
        Vector2 playerPos = cam.WorldToScreenPoint(player.position - new Vector3(0.5f, 0, 0));
        GUI.Box(new Rect(playerPos.x, Screen.height - playerPos.y, 100, 22), player.name);

        GUI.color = Color.yellow;
        foreach (Transform e in enemies)
        {
            Vector2 ePos = cam.WorldToScreenPoint(e.position - new Vector3(0.5f, 0, 0));
            GUI.Box(new Rect(ePos.x, Screen.height - ePos.y, 100, 22), e.name);
        }

        GUI.color = Color.green;
        foreach (Transform n in npcs)
        {
            Vector2 nPos = cam.WorldToScreenPoint(n.position - new Vector3(0.5f, 0, 0));
            GUI.Box(new Rect(nPos.x, Screen.height - nPos.y, 100, 22), n.name);
        }

        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.color = Color.white;
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
        
        Kill = new DebugCommand<string>("kill", "Kills the specified entity", "kill [entity ID]", (id) =>
        {
            GameEventSystem.current.GiveDamage(id, 999999999f);
        });

        Revive = new DebugCommand<string>("revive", "Revives the specified entity", "revive [entity ID]", (id) =>
        {
            GameEventSystem.current.Revive(id);
        });

        Damage = new DebugCommand<string, float>("damage", "Damages the specified entity by specified damage", "damage [entity ID] [damage]", (id, damage) =>
        {
            GameEventSystem.current.GiveDamage(id, damage);
        });
        
        Heal = new DebugCommand<string, float>("heal", "Heals the specified entity by amount", "heal [entity ID] [amount]", (id, damage) =>
        {
            GameEventSystem.current.Heal(id, damage);
        });

        ChangeDimension = new DebugCommand<string>("dimension", "Changes the current dimension to the specified one", "dimension [dimension]", (dim) =>
        {
            LevelManager.dimension = dim;
        });

        IDs = new DebugCommand("ids", "Displays the entity IDs above the entities", "ids", () =>
        {
            showIDs = !showIDs;
        });

        Paths = new DebugCommand("paths", "Displays all AI paths", "path", () =>
        {
            GameEventSystem.current.Debug("path");
        });

        commandList = new List<object>
        {
            Help,
            Kill,
            Revive,
            Damage,
            Heal,
            ChangeDimension,
            IDs,
            Paths,
        };
    }

    void HandleInput()
    {
        showUnknown = false;
        string[] args = input.Split(' ');

        bool found = false;
        for(int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
            if(input.Contains(commandBase.commandID))
            {
                found = true;

                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if (commandList[i] as DebugCommand<string> != null && args.Length == 2)
                {
                    (commandList[i] as DebugCommand<string>).Invoke(args[1]);
                }
                else if (commandList[i] as DebugCommand<string, float> != null && args.Length == 3)
                {
                    (commandList[i] as DebugCommand<string, float>).Invoke(args[1], float.Parse(args[2]));
                }
            }
        }

        if(!found)
        {
            unknownCommand = args[0];
            showUnknown = true;
        }
    }
}
