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

    public Animator animator;
    public Transform GunPosisition; 
    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    // Crosshair
    public bool lockCursor = true;
    public bool crosshair = true;
    public Sprite crosshairImage;
    public Color crosshairColor = Color.white;

    // Internal Variables
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Image crosshairObject;



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



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.Log("nem talaltuk a rigit");
        }
        crosshairObject = GetComponentInChildren<Image>();

        // Set internal variables
        playerCamera.fieldOfView = fov;
        originalScale = transform.localScale;    }

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (crosshair)
        {
            crosshairObject.sprite = crosshairImage;
            crosshairObject.color = crosshairColor;
        }
        else
        {
            crosshairObject.gameObject.SetActive(false);
        }
    }

    float camRotation;

    private void Update()
    {
        #region Camera

        // Control camera movement
        if (cameraCanMove)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

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

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
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
                Debug.Log("isWalking:" + isWalking);
            }
            else
            {
                animator.SetBool("IsMoving", true);
                isWalking = false;
            }
                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;
               // Debug.Log("Target Velocity: " + targetVelocity);
                // Apply a force that attempts to reach our target velocity
                Debug.Log("Current Velocity: " + rb.velocity);
                Vector3 velocity = rb.velocity;
                Debug.Log("mozog");
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
        RaycastHit hit;
       Debug.DrawRay(GunPosisition.transform.position, GunPosisition.transform.forward * 10f, Color.red);
        if (Input.GetMouseButton(0))
        {
            animator.SetBool("IsShooting", true);
            StartCoroutine(WaitForShootAnimation());
            if (Physics.Raycast(GunPosisition.transform.position, GunPosisition.transform.forward, out hit, 10f))
            {
                bullet.shootBullet(GunPosisition);
            }
        }
    }
    IEnumerator WaitForShootAnimation()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("IsShooting", false);
    }

}