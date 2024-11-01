using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="TrailConfig",menuName ="Guns/GunTrailConfig",order =4)]
public class TrailConfigurationScriptableObject : ScriptableObject
{
    public Material material;
    public AnimationCurve animcurver;
    public float Duration = 2.0f;
    public float MinvertexDistance = 2.0f;
    public Gradient color;
    public float missDintance=100f;
    public float SimulationSpeed=25;
}
