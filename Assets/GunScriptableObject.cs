using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunScriptableObject : ScriptableObject
{
    public GunType gunType;
    public string Name;
    public GameObject modelPrefab;
    public Vector3 SpawnPosint;
    public Vector3 SpawnRotation;
    public ShootConfigurationScriptableObject shootConfigurationScriptableObject;
    public TrailConfigurationScriptableObject trailConfigurationScriptableObject;
    private MonoBehaviour ActiveMono;
    private GameObject model;
    private float lastShootTime;
    private ParticleSystem shootSystem;
    [SerializeField] GameObject trailStartPosition;
    private ObjectPool<TrailRenderer> TrailPool;

    public void Spawn(Transform parent,MonoBehaviour ActiveMonobehaviour)
    {
        Debug.Log("ShootSystem: " + shootSystem);
        this.ActiveMono = ActiveMonobehaviour;
        lastShootTime = 0;
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        model=Instantiate(modelPrefab);
        model.transform.SetParent(parent,false);
        model.transform.localPosition = SpawnPosint;
        model.transform.localRotation=Quaternion.Euler(SpawnRotation);
        shootSystem=model.GetComponentInChildren<ParticleSystem>();
    }

    public void Shoot()
    {
        if (shootSystem == null)
        {
            Debug.LogError("Particle system not found in the model prefab!");
        }
        if (Time.time >= lastShootTime)
        { 
            lastShootTime= Time.time;
            shootSystem.Play();
            Vector3 shootDirection=shootSystem.transform.forward+ 
                new Vector3(
                    Random.Range(-shootConfigurationScriptableObject.Spread.x,shootConfigurationScriptableObject.Spread.y),
                    Random.Range(-shootConfigurationScriptableObject.Spread.x, shootConfigurationScriptableObject.Spread.y),
                    Random.Range(-shootConfigurationScriptableObject.Spread.x, shootConfigurationScriptableObject.Spread.y)
                    );
            shootDirection.Normalize();
            if (Physics.Raycast(shootSystem.transform.position,
                shootDirection, out RaycastHit hit
               ))
            {
                ActiveMono.StartCoroutine(PlayTrail
                    (shootSystem.transform.position,
                    hit.point, hit));
            }
            else
            {
                ActiveMono.StartCoroutine(PlayTrail
                        (shootSystem.transform.position,
                        shootSystem.transform.position+(trailConfigurationScriptableObject.missDintance*shootDirection),
                        new RaycastHit()));
            }

        }
    }
    private IEnumerator PlayTrail(Vector3 startPoint,Vector3 endPoint,RaycastHit hit)
    {
        Debug.Log("ShootSystem: " + shootSystem);
        TrailRenderer instance=TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;
        yield return null;
        instance.emitting = false;
        float distance=Vector3.Distance(startPoint, endPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                startPoint,
                endPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance))
                );
            remainingDistance -=trailConfigurationScriptableObject.SimulationSpeed*Time.deltaTime;  
            yield return null;
        }
        instance.transform.position = endPoint;
        yield return new WaitForSeconds(trailConfigurationScriptableObject.Duration);
        yield return null;
        instance.emitting=false;
        instance.gameObject.SetActive(false);
        TrailPool.Release(instance);
    }
    private TrailRenderer CreateTrail()
    {
        Debug.Log("ShootSystem: " + shootSystem);

        GameObject intance = new GameObject("BulletTrail");
        TrailRenderer trail=intance.AddComponent<TrailRenderer>();
        trail.colorGradient = trailConfigurationScriptableObject.color;
        trail.material=trailConfigurationScriptableObject.material;
        trail.widthCurve = trailConfigurationScriptableObject.animcurver;
        trail.time = trailConfigurationScriptableObject.Duration;
        trail.minVertexDistance=trailConfigurationScriptableObject.MinvertexDistance;
        trail.emitting = false;
        trail.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;
        return trail;   
    }
}
