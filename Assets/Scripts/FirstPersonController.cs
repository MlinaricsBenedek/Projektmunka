using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class FirstPersonController : MonoBehaviour
{
    private Rigidbody rb;

    public Camera playerCamera;
    public Bullet bullet;
    public GameObject UpperBody;
    public Animator animator;
    public Transform GunPosisition;
    private bool playerCanShoot = true;
    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;
    public bool canShootBullet;
    // Crosshair
    public bool lockCursor = true;
    // Internal Variables
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float horizontalYaw=0.0f;
    private Image crosshairObject;

   // public GameObject aimTarget;

    public bool enableZoom = true;
    public bool holdToZoom = false;
    public KeyCode zoomKey = KeyCode.Mouse1;
    public float zoomFOV = 30f;
    public float zoomStepTime = 5f;

    // Internal Variables
    private bool isZoomed = false;
    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 5.0f;

    // Internal Variables
    private bool isWalking = false;
    // Internal Variables
    private Vector3 originalScale;
    float zDinstance = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Set internal variables
        playerCamera.fieldOfView = fov;
        originalScale = transform.localScale;    
    }

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

      //aimTarget.transform.position = new Vector3(0, 0, 0);
      //  aimTarget.transform.localEulerAngles = new Vector3(270, 0, 90);
    }

    float camRotation;

    private void Update()
    {
        #region Camera

        // Control camera movement
        if (cameraCanMove)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
           // horizontalYaw = UpperBody.transform.localEulerAngles.z + Input.GetAxis("Mouse Y") * mouseSensitivity;

            if (!invertCamera)
            {
                pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
            }
            else
            {
                // Inverted Y
                pitch += mouseSensitivity * Input.GetAxis("Mouse Y");
            }

            // Clamp pitch between lookAngle
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
           // horizontalYaw = Mathf.Clamp(horizontalYaw, -maxLookAngle, maxLookAngle);
           
        //    UpperBody.transform.localEulerAngles=new Vector3(0, 0, horizontalYaw);
            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
            //a kamer�nak v�ltozik a locallocalEulerAngles-e, jelenleg az a hiba hogy ez van �tadva a aimtargetnek, pedig ez csak a helyben mozg�st v�ltoztatja.
            //ehelyett egy rotation-t kellene �tadni, vagy valami fajta elmozduk�l�st.
            //�tlet, k�rszelet alapon, hat�rozzuk meg, a kamera �s ennek a t�vols�g�b�l a sugarat, �s azon az �ven pr�b�ljuk meg mozgatni.
            //   float distance=aimTarget.transform.position.x- playerCamera.transform.position.x;
            Vector3 mouseScreenPosition = Input.mousePosition;

            // 2. Az eg�r poz�ci�j�t vil�g koordin�t�kra konvert�ljuk, egy adott Z t�vols�g megad�s�val
            mouseScreenPosition.z = zDinstance; // Ez hat�rozza meg a kamer�t�l m�rt t�vols�got
            Vector3 mouseWorldPosition = playerCamera.ScreenToWorldPoint(mouseScreenPosition);

            // 3. Be�ll�tjuk a GameObject poz�ci�j�t az eg�r poz�ci�j�nak megfelel�en
        //    aimTarget.transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, mouseWorldPosition.z);
            // Jelenlegi poz�ci� ment�se
            //Vector3 currentPosition = aimTarget.transform.position;

            // �j poz�ci� l�trehoz�sa az x tengelyen
            //aimTarget.transform.position = new Vector3(currentPosition.x+ (Mathf.Tan(playerCamera.transform.eulerAngles.y) *Mathf.Deg2Rad) * (-1f) * distance , currentPosition.y, currentPosition.z);
            //aimTarget.transform.position=playerCamera.transform.position;

              //aimTarget.transform.localEulerAngles = playerCamera.transform.localEulerAngles;
        }

        #region Camera Zoom

        if (enableZoom)
        {
            // Changes isZoomed when key is pressed
            // Behavior for toogle zoom
            if (Input.GetKeyDown(zoomKey) && !holdToZoom)
            {
                if (!isZoomed)
                {
                    isZoomed = true;
                }
                else
                {
                    isZoomed = false;
                }
            }

            if (holdToZoom)
            {
                if (Input.GetKeyDown(zoomKey))
                {
                    isZoomed = true;
                }
                else if (Input.GetKeyUp(zoomKey))
                {
                    isZoomed = false;
                }
            }

            // Lerps camera.fieldOfView to allow for a smooth transistion
            if (isZoomed)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
            }
            else if (!isZoomed)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, zoomStepTime * Time.deltaTime);
            }
        }
        Shoot();
        #endregion
        #endregion
    }

    void FixedUpdate()
    {
        if (playerCanMove)
        {

            // Calculate how fast we should be moving
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            // Checks if player is walking and isGrounded
            // Will allow head bob
            if (targetVelocity.x != 0 || targetVelocity.z != 0)
            {

                isWalking = true;
                animator.SetBool("IsMoving", false);
            }
            else
            {
                animator.SetBool("IsMoving", true);
                isWalking = false;
            }
                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;
               // Debug.Log("Target Velocity: " + targetVelocity);
                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.linearVelocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                //Debug.Log("velocitychange:" + velocityChange);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                //Debug.Log("velocitychange X-tengelyen" + velocityChange.x);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }
   
    private void Shoot()
    {
      
            //RaycastHit hit;
            //Debug.DrawRay(GunPosisition.transform.position, GunPosisition.transform.forward * 10f, Color.red);
            if (Input.GetMouseButton(0) && playerCanShoot)
            {
                playerCanShoot = false;

                animator.SetBool("IsShooting", true);
                canShootBullet = true ;
                StartCoroutine(WaitForShoot());
                //if (Physics.Raycast(GunPosisition.transform.position, GunPosisition.transform.forward, out hit, 10f))
                //{

                //    bullet.shootBullet(GunPosisition);
                //}   
            }
    }
    IEnumerator WaitForShoot()
    {
        //first for the animation!!!
        
        yield return new WaitForSeconds(1f);//el�g?
        bullet.shootBullet(GunPosisition,canShootBullet);

        canShootBullet = false;
        animator.SetBool("IsShooting", false);

        //Handle the weapon reloading time
        yield return new WaitForSeconds(3f);
        playerCanShoot = true;
    }

}