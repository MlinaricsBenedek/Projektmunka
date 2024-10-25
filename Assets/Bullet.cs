using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    public float bulletSpeed = 20f;
    // Start is called before the first frame update
    
    public void shootBullet(Transform startPosition)
    { 
    GameObject gm=Instantiate(bullet,startPosition.position,bullet.transform.rotation);
       Rigidbody rb = gm.GetComponent<Rigidbody>();
        rb.AddForce(startPosition.forward*bulletSpeed,ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        { 
        Destroy(gameObject);
        }
    }
}
