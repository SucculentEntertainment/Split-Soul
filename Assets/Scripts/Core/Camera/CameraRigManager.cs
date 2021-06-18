using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRigManager : MonoBehaviour
{
	public float sizeNormal = 3;
	public float sizeTitleScreen = 5;
	public Camera camera;

	[HideInInspector] public bool titleScreen = false;

    public void setTitleScreenMode(bool mode)
	{
		if(mode) camera.orthographicSize = sizeTitleScreen;
		else     camera.orthographicSize = sizeNormal;

		titleScreen = mode;
	}
}
