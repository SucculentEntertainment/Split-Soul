using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightRegisterEvent : MonoBehaviour
{
    private void Start()
    {
        GameEventSystem.current.onLightRegister += onLightRegister;
        GameEventSystem.current.onLightUnregister += onLightUnregister;
    }

    private void onLightRegister(Light2D light)
    {
        gameObject.SendMessage("OnLightRegister", light);
    }

    private void onLightUnregister(Light2D light)
    {
        gameObject.SendMessage("OnLightUnregister", light);
    }

    public void unregister()
    {
        GameEventSystem.current.onLightRegister += onLightRegister;
        GameEventSystem.current.onLightUnregister -= onLightUnregister;
    }
}
