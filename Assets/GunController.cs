using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;
    public FirstPersonController fps;
    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;
    public bool equipped = false;
    public static bool slotFull;
    float ChangeDiagnole = 0.25f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    { 
        //Check if player is in range and "E" is pressed
        //Drop if equipped and "Q" is pressed
        if (equipped && slotFull && Input.GetKeyDown(KeyCode.Q)) Drop();
        if(!equipped&& !slotFull && Input.GetKeyDown(KeyCode.E)) PickUp();
    }
   
    private void PickUp()
    {
            equipped = true;
            slotFull = true;
            transform.SetParent(null);
            transform.SetParent(gunContainer);
            transform.position = Vector3.zero;
            transform.localScale = Vector3.one;
   
            ////Make Rigidbody kinematic and BoxCollider a trigger
            rb.isKinematic = true;
            coll.isTrigger = true;
        Gunposition();
    }
    private void Drop()
    {
        equipped = false;
        transform.SetParent(null);
        slotFull = false;
        rb.isKinematic = false;
        coll.isTrigger = false;
        //Gun carries momentum of player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;
        transform.localScale = new Vector3(0,0,0);
        //AddForce
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 3);
    }
    private void Gunposition()
    { 
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        transform.localEulerAngles = Vector3.zero; 
    }
}
