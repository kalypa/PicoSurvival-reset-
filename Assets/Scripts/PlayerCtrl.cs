using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public float movespd = 2.0f;
    public float runMoveSpd = 3.5f;
    public float directRotateSpd = 100.0f;
    public float bodyRotateSpd = 5.0f;
    public float velocityChangeSpd = 0.1f;
    public bool stopMove = false;
    private CharacterController characterCtrl = null;
    private CollisionFlags collisionFlags = CollisionFlags.None;
    public CapsuleCollider AtkCapsuleCollider = null;
    public CapsuleCollider AtkCapsuleCollider2 = null;
    public SphereCollider AtkSphereCollider = null;
    private readonly float gravity = 9.8f;
    public float verticalSpd = 0;
    public Animator animator = null;
    private Inventory inventory;
    private float smoothness = 10f;
    public bool isSit = false;
    [SerializeField]
    private Transform followCamera;
    public float playerHP = 100;
    public bool isGun = false;
    public bool isAxe = false;
    public bool isPickAxe = false;
    Camera _camera;
    [SerializeField]
    private DinosaurCtrl rapter;
    private Rigidbody rigid;
    [SerializeField]
    private GameObject gun;
    [SerializeField]
    private GameObject axe;
    [SerializeField]
    private GameObject pickAxe;
    [SerializeField]
    private Gun gunScript;
    [SerializeField]
    private GameObject bulletText;
    [SerializeField]
    private GameObject Hpbar;
    [SerializeField]
    private GameObject Hpbar2;
    [SerializeField]
    private GameObject fullbar;
    [SerializeField]
    private CraftManual craft;
    [SerializeField]
    private GameObject deadPanel;
    void Start()
    {
        _camera = Camera.main;
        characterCtrl = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();
        rapter = GetComponent<DinosaurCtrl>();
        rigid = GetComponent<Rigidbody>();
    }


    void Update()
    {
        Move();
        SetGravity();
        InputAttackCtrll();
        Swap();
        Debug.Log(verticalSpd);
    }

    void LateUpdate()
    {
        Vector3 playerDir = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerDir), Time.deltaTime * smoothness);
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if (isSit == true)
            {
                followCamera.localPosition = new Vector3(0, 0.6f, 0);
            }
            else
            {
                followCamera.localPosition = new Vector3(0, 1f, 0);
            }
        }
    }

    void Move()
    {
        if (!gunScript.isReload)
        {
            if (stopMove == true)
            {
                return;
            }
            Transform CameraTransform = Camera.main.transform;
            Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
            forward.y = 0.0f;

            Vector3 right = CameraTransform.TransformDirection(Vector3.right);
            float vertical = Input.GetAxisRaw("Vertical");
            float horizontal = Input.GetAxisRaw("Horizontal");

            Vector3 targetDirect = horizontal * right + vertical * forward;
            float spd = movespd;
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            {
                if (isSit == false)
                {
                    if (isGun == false)
                    {
                        spd = runMoveSpd;
                        animator.SetFloat("MoveSpd", 1f);
                    }
                    else if (isGun == true)
                    {
                        spd = runMoveSpd;
                        animator.SetFloat("GunMoveSpd", 1f);
                    }
                }
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                if (isSit == false)
                {
                    if (isGun == false)
                    {
                        animator.SetFloat("MoveSpd", 0.1f);
                    }
                    else if (isGun == true)
                    {
                        animator.SetFloat("GunMoveSpd", 0.1f);
                    }
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (isSit == false)
                {
                    if (isGun == false)
                    {
                        spd = movespd * 0.8f;
                        animator.SetFloat("LeftMoveSpd", 0.1f);
                    }
                    else if (isGun == true)
                    {
                        spd = movespd * 0.8f;
                        animator.SetFloat("GunLeftMoveSpd", 0.1f);
                    }
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (isSit == false)
                {
                    if (isGun == false)
                    {
                        spd = movespd * 0.8f;
                        animator.SetFloat("RightMoveSpd", 0.1f);
                    }
                    else if (isGun == true)
                    {
                        spd = movespd * 0.8f;
                        animator.SetFloat("GunRightMoveSpd", 0.1f);
                    }
                }
            }
            else
            {
                animator.SetFloat("MoveSpd", 0f);
                animator.SetFloat("LeftMoveSpd", 0f);
                animator.SetFloat("RightMoveSpd", 0f);
                animator.SetFloat("GunMoveSpd", 0f);
                animator.SetFloat("GunLeftMoveSpd", 0f);
                animator.SetFloat("GunRightMoveSpd", 0f);
            }
            Vector3 amount = (targetDirect.normalized * spd * Time.deltaTime);
            collisionFlags = characterCtrl.Move(amount);
        }
    }

    void InputAttackCtrll()
    {
        if (Input.GetMouseButtonDown(0) == true && isGun == false && craft.isPreview == false)
        {
            if(isAxe == true)
            {
                animator.SetBool("isAtk", true);
                AtkCapsuleCollider.enabled = true;
            }
            else if(isPickAxe == true)
            {
                animator.SetBool("isPickAtk", true);
                AtkCapsuleCollider2.enabled = true;
            }
            else
            {
                animator.SetBool("isPunch", true);
                AtkSphereCollider.enabled = true;
            }
        }
        else
        {
            AtkCapsuleCollider.enabled = false;
            AtkSphereCollider.enabled = false;
            AtkCapsuleCollider2.enabled = false;
        }
    }

    void Swap()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            animator.SetBool("isAxe", false);
            axe.SetActive(false);
            animator.SetBool("isPickAxe", false);
            pickAxe.SetActive(false);
            animator.SetBool("isGun", false);
            gun.SetActive(false);

        }
        else if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            isAxe = !isAxe;
            animator.SetBool("isAxe", isAxe);
            animator.SetBool("isPickAxe", false);
            animator.SetBool("isGun", false);
            axe.SetActive(isAxe);
            pickAxe.SetActive(false);
            gun.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            isPickAxe = !isPickAxe;
            animator.SetBool("isPickAxe", isPickAxe);
            animator.SetBool("isAxe", false);
            animator.SetBool("isGun", false);
            pickAxe.SetActive(isPickAxe);
            axe.SetActive(false);
            gun.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            isGun = !isGun;
            bulletText.SetActive(isGun);
            animator.SetBool("isGun", isGun);
            animator.SetBool("isAxe", false);
            animator.SetBool("isPickAxe", false);
            gun.SetActive(isGun);
            axe.SetActive(false);
            pickAxe.SetActive(false);
        }
    }
    void SetGravity()
    {
        if ((collisionFlags & CollisionFlags.CollidedBelow) != 0)
        {
            verticalSpd = 0f;
        }
        else
        {
            verticalSpd -= gravity * Time.deltaTime;
        }
    }

    IEnumerator Damaged()
    {
        playerHP -= 10;
        yield return  new WaitForSeconds(0.1f);
        if(playerHP > 0)
        {
            Hpbar.transform.localEulerAngles = new Vector3(0, 0, Hpbar.transform.localEulerAngles.z - 75);
            if(Hpbar.transform.localEulerAngles.z <= 0)
            {
                fullbar.SetActive(false);
            }
            else if (Hpbar.transform.localEulerAngles.z <= -50)
            {
                Hpbar2.SetActive(false);
            }
                animator.SetBool("isDamaged", true);
        }
        else
        {
            Hpbar.SetActive(false);
            Debug.Log("Dead");
            animator.Play("Dead");
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
            deadPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("EnemyAtk") == true)
        {
            StartCoroutine(Damaged());
        }
    }
    void PickUpEnd()
    {
        animator.SetBool("isPick", false);
    }
    void AtkEnd()
    {
        animator.SetBool("isAtk", false);
    }
    void PunchEnd()
    {
        animator.SetBool("isPunch", false);
    }
    void DamagedEnd()
    {
        animator.SetBool("isDamaged", false);
    }
    void PickAxeEnd()
    {
        animator.SetBool("isPickAtk", false);
    }
}
