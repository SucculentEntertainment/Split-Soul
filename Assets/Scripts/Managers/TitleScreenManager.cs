using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleScreenManager : MonoBehaviour
{
    private void OnEscape(InputValue val)
    {
		GameEventSystem.current.UIAction("ESC");
    }

	private void OnReturn(InputValue val)
    {
		GameEventSystem.current.UIAction("Enter");
    }
}
