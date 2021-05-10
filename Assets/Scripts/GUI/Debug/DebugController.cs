using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DebugController : MonoBehaviour
{
    public Camera cam;
    public Font font;

	public Transform outputTextContainer;
	public GameObject fadingTextElement;

    private bool showIDs = false;
	private int prevChildCount = 0;

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



	// ================================
    //  Functions
    // ================================

	private void OnEnable()
	{
		transform.GetChild(0).GetComponent<InputField>().ActivateInputField();
	}

	private void Update()
	{
		int childCount = outputTextContainer.childCount;
		if(prevChildCount == childCount) return;
		prevChildCount = childCount;

		float width = outputTextContainer.GetComponent<RectTransform>().sizeDelta.x;
		float height = childCount * fadingTextElement.GetComponent<RectTransform>().sizeDelta.y;

		outputTextContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
	}

	private void textOutput(string text, Font font, float lifetime, Color startColor, Color endColor)
	{
		//Instantiate
		GameObject obj = Instantiate(fadingTextElement, outputTextContainer);
		FadingTextElement fte = obj.GetComponent<FadingTextElement>();

		//Set text, font and lifetime
		fte.text = text;
		fte.font = font;
		fte.lifetime = lifetime;

		//Set Gradient
		if(startColor == null) startColor = Color.white;
		if(endColor == null) endColor = startColor;

		GradientColorKey[] colorKey = new GradientColorKey[2];
        colorKey[0].color = startColor;
        colorKey[0].time = 0.0f;
        colorKey[1].color = endColor;
        colorKey[1].time = 1.0f;

        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        fte.gradient.SetKeys(colorKey, alphaKey);

	}

    private void printHelp()
	{
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase command = commandList[i] as DebugCommandBase;

            string text = $"{command.commandFormat}" + " => " + $"{command.commandDescription}";
            textOutput(text, font, 10, Color.white, Color.white);
        }
    }

    // ================================
    //  Commands
    // ================================

    private void Awake()
    {
        Help = new DebugCommand("help", "Shows a list of available commands", "help", () =>
        {
            printHelp();
        });

        Kill = new DebugCommand<string>("kill", "Kills the specified entity", "kill [entity ID]", (id) =>
        {
            GameEventSystem.current.GiveDamage(id, 999999999f);
            textOutput("Killed " + id, font, 5, Color.white, Color.white);
        });

        Revive = new DebugCommand<string>("revive", "Revives the specified entity", "revive [entity ID]", (id) =>
        {
            GameEventSystem.current.Revive(id);
            textOutput("Revived " + id, font, 5, Color.white, Color.white);
        });

        Damage = new DebugCommand<string, float>("damage", "Damages the specified entity by specified damage", "damage [entity ID] [damage]", (id, damage) =>
        {
            GameEventSystem.current.GiveDamage(id, damage);
            textOutput("Dealt " + damage + " damage to " + id, font, 5, Color.white, Color.white);
        });

        Heal = new DebugCommand<string, float>("heal", "Heals the specified entity by amount", "heal [entity ID] [amount]", (id, damage) =>
        {
            GameEventSystem.current.Heal(id, damage);
            textOutput("Healed " + id + " by " + damage + " HP", font, 5, Color.white, Color.white);
        });

        ChangeDimension = new DebugCommand<string>("dimension", "Changes the current dimension to the specified one", "dimension [dimension]", (dim) =>
        {
            LevelManager.dimension = dim;
            textOutput("Changed dimension to " + dim, font, 5, Color.white, Color.white);
        });

        IDs = new DebugCommand("ids", "Displays the entity IDs above the entities", "ids", () =>
        {
            textOutput("Toggled ID Labels", font, 5, Color.white, Color.white);
        });

        Paths = new DebugCommand("paths", "Displays all AI paths", "paths", () =>
        {
            GameEventSystem.current.Debug("path");
            textOutput("Toggled display of pathfinding paths", font, 5, Color.white, Color.white);
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

    public void HandleInput()
    {
		string input = transform.GetChild(0).GetComponent<InputField>().text;
		transform.GetChild(0).GetComponent<InputField>().text = "";
		transform.GetChild(0).GetComponent<InputField>().ActivateInputField();

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

        if(!found) { textOutput("Unknown command: " + args[0], font, 5, Color.red, Color.red); }
    }
}
