﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitEvent : MonoBehaviour
{
    public List<string> ignored;

    private void Start()
    {
        GameEventSystem.current.onProjectileHit += onProjectileHit;
    }

    private void onProjectileHit(string id, ProjectileData data)
    {
        if(ignored.Contains(data.name)) return;
        if (id == gameObject.name) gameObject.SendMessage("OnProjectileHit", data);
    }

    public void unregister()
    {
        GameEventSystem.current.onProjectileHit -= onProjectileHit;
    }
}