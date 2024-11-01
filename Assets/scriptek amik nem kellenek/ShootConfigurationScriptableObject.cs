using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Shoot",menuName ="Guns/ShootConfiguration",order =2)]
public class ShootConfigurationScriptableObject :ScriptableObject
{
    // Start is called before the first frame update
    public Vector3 Spread = new Vector3(0.1f, 0.1f, 0.1f);
    public float FireRate = 0.25f;
}
