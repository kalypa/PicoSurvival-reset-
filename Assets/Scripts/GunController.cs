using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;

    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
    }

    private void GunFireRateCalc()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    private void TryFire()
    {
        if(Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            currentGun.animator.SetTrigger("isShoot");
            Fire();
        }
    }

    private void Fire()
    {
        if(currentGun.currentBulletCount > 0)
        {
            Shoot();
        }
        else
        {
            PlaySound(currentGun.empty_Sound);
        }
    }

    private void Shoot()
    {
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;
        currentGun.muzzleFlash.Play();
        PlaySound(currentGun.fire_Sound);
    }

    private void Reload()
    {
        if(currentGun.carryBulletCount > 0)
        {
            currentGun.animator.SetTrigger("Reload");
        }
    }

    private void PlaySound(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
