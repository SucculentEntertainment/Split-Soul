using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CollectionManager : MonoBehaviour
{
    public bool useLightCollection;
    public LightCollection lightCollection;
    [SerializeField] public List<Light2D> lights;

    public bool useParticleCollection;
    public ParticleCollection particleCollection;
    [SerializeField] public List<ParticleSystem> particleSystems;

    private void OnDimensionEnable(string dimension)
    {
        LightProfile lProfile = lightCollection.profiles.Find(x => x.dimension == dimension);
        foreach(Light2D l in lights) { l.color = lProfile.color; }

        ParticleProfile pProfile = particleCollection.profiles.Find(x => x.dimension == dimension);
        foreach(ParticleSystem p in particleSystems)
        {
            ParticleSystem.ColorOverLifetimeModule col = p.colorOverLifetime;
            col.color = pProfile.gradient;
        }
    }

    private void OnDimensionDisable(string dimension) { }
}