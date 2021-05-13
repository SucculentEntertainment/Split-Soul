using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileProfile
{
    public string dimension;
    public GameObject projectile;
}

public class ProjectileSpawner : MonoBehaviour
{
    public bool useDifferentProfiles = false;
    public List<ProjectileProfile> profiles;

    public void spawnProjectile(Vector2 aimPosition)
    {
        ProjectileProfile profile = profiles[0];
        if(useDifferentProfiles) profile = profiles.Find(x => x.dimension == LevelManager.dimension);

        GameObject instance = Instantiate(profile.projectile, transform.position, Quaternion.identity, LevelManager.current.projectileContainer.transform);
        Vector2 dir = (aimPosition - (Vector2) transform.position).normalized;
        instance.GetComponent<ProjectileBase>().init(dir, this.name);
    }
}
