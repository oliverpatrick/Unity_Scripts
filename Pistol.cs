using System.Collections;
using UnityEngine;

public class Pistol : MonoBehaviour
{

    [SerializeField] public float damage = 10f;
    [SerializeField] public float range = 100f;
    [SerializeField] public float fireRate = 15f;
    [SerializeField] public float impactForce = 40f;

    public int maxAmmo = 9;
    private int currentAmmo;
    public float reloadTime = 1f;
    public bool isReloading = false;

    [SerializeField] public Camera fpsCam;
    //public ParticleSystem muzzleFlash; if adding muzzle flash
    [SerializeField] public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    public Animator animator;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("PistolReloading", false);
    }

    void Update()
    {
        if(isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shot();
        }
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        animator.SetBool("PistolReloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("PistolReloading", false);
        yield return new WaitForSeconds(.25f);


        currentAmmo = maxAmmo;
        isReloading = false;
    }

    public void Shot()
    {
        //muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            EnemyPlayer enemyPlayer = hit.transform.GetComponent<EnemyPlayer>();

            if (enemyPlayer != null)
            {
                enemyPlayer.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject hitImpact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(hitImpact, 2f);
        }

    }
}
