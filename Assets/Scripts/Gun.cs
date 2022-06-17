using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    enum State
    {
        Ready,
        Fire,
        Reload,
        Empty
    }

    public Transform fireTransform;
    public AudioClip fireClip;
    public AudioClip reloadClip;

    [SerializeField] private AudioSource audio;
    [SerializeField] private ParticleSystem muzzleFlashEffect;
    [SerializeField] private ParticleSystem shellEjectEffect;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private State state;

    [SerializeField] private const float magSize = 30f;
    [SerializeField] private float totalMagSize;
    [SerializeField] private float currentMagSize;
    [SerializeField] private float accuracy;
    [SerializeField] private float rpm;
    [SerializeField] private float lastFireTime;
    [SerializeField] private float fireDistance;
    [SerializeField] private float currentSpread;
    [SerializeField] private float maxSpread;
    [SerializeField] private float spreadSmoothTime;
    [SerializeField] private float refSpreadVelocity;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();

        state = State.Ready;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;

        lastFireTime = Time.time;
        rpm = 0.2f;
        accuracy = 1f;
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

    void Fire(Vector3 aim)
    {
        if (Time.time >= rpm + lastFireTime && currentMagSize > 0)
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

    void Reload()
    {
        if (currentMagSize <= magSize || totalMagSize <= 0)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        audio.PlayOneShot(reloadClip);
        yield return new WaitForSeconds(1.8f);

        totalMagSize -= Mathf.Clamp(magSize - currentMagSize, 0, totalMagSize);
        currentMagSize = magSize;
    }
}
