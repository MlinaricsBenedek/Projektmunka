using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField] private GunType Gun;
    [SerializeField] private Transform GunParent;
    [SerializeField] private List<GunScriptableObject> Guns;
    [Space]
    [Header("Runtime Filled")]
    public GunScriptableObject ActiveGun;
    private void Start()
    {
        GunScriptableObject gun = Guns.Find(gun => gun.gunType == Gun);
        if (gun == null)
        {
            Debug.Log("No scriptableObject found as a Guntype");
            return;
        }
        ActiveGun = gun;
        gun.Spawn(GunParent, this);
    }


}
