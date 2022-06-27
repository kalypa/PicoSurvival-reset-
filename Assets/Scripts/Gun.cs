using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public string gunName;
    public int bulletsPerMag;
    public int bulletsTotal;
    public int currentBullets;
    public float range;
    public float fireRate;
    private float fireTimer;
    public Transform shootPoint;
    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip emptySound;
    public ParticleSystem muzzleFlash;
    [SerializeField]
    private Animator animator;
    public Text bulletsText;
    public bool isReload = false;
    public Vector3 aimPosition;
    private Vector3 originalPosition;
    public Transform aimTransform;
    public bool isAiming;
    [SerializeField]
    private DinosaurCtrl rapter;
    [SerializeField]
    private PlayerCtrl playerCtrl;
    void Start()
    {
        currentBullets = bulletsPerMag;
        bulletsText.text = currentBullets + " / " + bulletsTotal;
        originalPosition = transform.localPosition;
    }
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * range, Color.blue);
        AimDownSights();
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        isReload = info.IsName("Reload");

        if (Input.GetButton("Fire1"))
        {
            if(!isReload)
            {
                if (currentBullets > 0 && playerCtrl.verticalSpd != 0)
                {
                    Fire();
                }
                else
                {
                    audioSource.clip = emptySound;
                    audioSource.Play();
                }
            }
        }
        if(fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            DoReload();
        }
    }
    void Fire()
    {
        if(fireTimer <fireRate || isReload == true)
        {
            return;
        }
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range, 1 << LayerMask.NameToLayer("Enemy")))
        {
            HealthManager healthManager = hit.transform.GetComponent<HealthManager>();
            if (healthManager)
            {
                healthManager.ApplyDamage(10);
            }


        }
        currentBullets--;
        fireTimer = 0.0f;
        audioSource.PlayOneShot(shootSound);
        animator.SetTrigger("isRifle");
        muzzleFlash.Play();
        bulletsText.text = currentBullets + " / " + bulletsTotal;
    }
    private void AimDownSights()
    {
        if (Input.GetButton("Fire2") && !isReload)
        {
            animator.SetBool("isSight", true);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 40f, Time.deltaTime * 8f);
            isAiming = true;
        }
        else
        {
            animator.SetBool("isSight", false);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60f, Time.deltaTime * 8f);
            isAiming = false;
        }
    }
    private void DoReload()
    {
        if (!isReload && currentBullets < bulletsPerMag && bulletsTotal > 0)
        {
            if(playerCtrl.verticalSpd !=0)
            {
                animator.CrossFadeInFixedTime("Reload", 0.01f);
                audioSource.PlayOneShot(reloadSound);
            }
            else if(playerCtrl.verticalSpd == 0)
            {
                audioSource.clip = emptySound;
                audioSource.Play();
            }
        }
    }

    public void Reload()
    {
        int bulletsToReload = bulletsPerMag - currentBullets;
        if (bulletsToReload > bulletsTotal)
        {
            bulletsToReload = bulletsTotal;
        }
        currentBullets += bulletsToReload;
        bulletsTotal -= bulletsToReload;
        bulletsText.text = currentBullets + " / " + bulletsTotal;
    }


}
