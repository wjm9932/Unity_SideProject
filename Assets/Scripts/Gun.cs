using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Fire,
        Reload,
        Empty
    }

    public Transform fireTransform;
    public AudioClip fireClip;
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;
    public AudioClip reloadClip;

    public State state { get; private set; }
    public float fireDistance { get; private set; }
    
    private LineRenderer lineRenderer;
    private AudioSource audio;
    private const float magSize = 30f;
    private float totalMagSize;
    private float currentMagSize;
    private float accuracy;
    private float rpm;
    private float lastFireTime;
    private float currentSpread;
    private float maxSpread;
    private float spreadSmoothTime;
    private float refSpreadVelocity;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();

        state = State.Ready;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;

        lastFireTime = Time.time;
        rpm = 0.1f;
        accuracy = 10f;
        fireDistance = 100f;
        spreadSmoothTime = 2f;
        maxSpread = 3f;

        totalMagSize = 120f;
        currentMagSize = magSize;
        currentSpread = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpread = Mathf.SmoothDamp(currentSpread, 0f, ref refSpreadVelocity, spreadSmoothTime);
    }

    public bool Fire(Vector3 aim)
    {
        if (Time.time >= rpm + lastFireTime && state == State.Ready)
        {
            Vector3 dir = aim - fireTransform.position;
            float xOffset = Utility.GetRandomNormalDistribution(0, currentSpread);
            float yOffset = Utility.GetRandomNormalDistribution(0, currentSpread);

            dir = Quaternion.AngleAxis(xOffset, Vector3.right) * dir;
            dir = Quaternion.AngleAxis(yOffset, Vector3.up) * dir;

            if (currentSpread < maxSpread)
            {
                currentSpread += 1 / accuracy;
            }
            lastFireTime = Time.time;

            RaycastHit ray;
            Physics.Raycast(fireTransform.position, dir, out ray, fireDistance);

            --currentMagSize;

            StartCoroutine(ShotEffect(ray.point));
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        audio.PlayOneShot(fireClip);

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, fireTransform.position);
        lineRenderer.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(0.02f);

        lineRenderer.enabled = false;
    }

    public bool Reload()
    {
        if (currentMagSize < magSize || totalMagSize <= 0)
        {
            state = State.Reload;
            StartCoroutine(ReloadCoroutine());
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator ReloadCoroutine()
    {
        totalMagSize -= Mathf.Clamp(magSize - currentMagSize, 0, totalMagSize);
        currentMagSize = magSize;

        audio.PlayOneShot(reloadClip);
   
        yield return new WaitForSeconds(1.8f);

        state = State.Ready;
    }
}
