using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public SphereCollider AtkSphereCollider = null;
    private readonly float gravity = 9.8f;
    public float verticalSpd = 0;
    public Animator animator = null;
    private Inventory inventory;
    private float smoothness = 10f;
    private bool isSit = false;
    [SerializeField]
    private Transform followCamera;
    public float playerHP = 100;
    private bool isGun = false;
    Camera _camera;
    [SerializeField]
    private DinosaurCtrl rapter;
    private Rigidbody rigid;
    [SerializeField]
    private GameObject gun;
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
        Debug.Log(playerHP);
    }

    void LateUpdate()
    {
        Vector3 playerDir = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerDir), Time.deltaTime * smoothness);
        if (isSit == true)
        {
            followCamera.localPosition = new Vector3(0, 0.6f, 0);
        }
        else
        {
            followCamera.localPosition = new Vector3(0, 1f, 0);
        }
    }

    void Move()
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
            spd = runMoveSpd;
            animator.SetFloat("MoveSpd", 1f);
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                spd = movespd * 0.5f;
                isSit = !isSit;
                animator.SetBool("isSit", isSit);
                animator.SetBool("isSitMove_F", isSit);
            }
            else if(isSit == true)
            {
                spd = movespd * 0.5f;
                animator.SetBool("isSitMove_F", true);
            }
            else if(isSit == false)
            {
                animator.SetFloat("MoveSpd", 0.1f);
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                spd = movespd * 0.5f;
                isSit = !isSit;
                animator.SetBool("isSit", isSit);
                animator.SetBool("isSitMove_L", isSit);
            }
            else if (isSit == true)
            {
                spd = movespd * 0.5f;
                animator.SetBool("isSitMove_L", true);
            }
            else if(isSit == false)
            {
                spd = movespd * 0.8f;
                animator.SetFloat("LeftMoveSpd", 0.1f);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                spd = movespd * 0.5f;
                isSit = !isSit;
                animator.SetBool("isSit", isSit);
                animator.SetBool("isSitMove_R", isSit);
            }
            else if (isSit == true)
            {
                spd = movespd * 0.5f;
                animator.SetBool("isSitMove_R", true);
            }
            else if(isSit == false)
            {
                spd = movespd * 0.8f;
                animator.SetFloat("RightMoveSpd", 0.1f);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                isSit = !isSit;
                animator.SetBool("isSit", isSit);
            }
            animator.SetBool("isSitMove_F", false);
            animator.SetBool("isSitMove_L", false);
            animator.SetBool("isSitMove_R", false);
            animator.SetFloat("MoveSpd", 0f);
            animator.SetFloat("LeftMoveSpd", 0f);
            animator.SetFloat("RightMoveSpd", 0f);
        }

        Vector3 amount = (targetDirect.normalized * spd * Time.deltaTime);
        collisionFlags = characterCtrl.Move(amount);
    }

    void InputAttackCtrll()
    {
        if (Input.GetMouseButtonDown(0) == true && isGun == false)
        {
            animator.SetBool("isPunch", true);
            AtkSphereCollider.enabled = true;
        }
        else
        {
            AtkCapsuleCollider.enabled = false;
            AtkSphereCollider.enabled = false;
        }
    }

    void Swap()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            isGun = !isGun;
            animator.SetBool("isGun", isGun);
            gun.SetActive(isGun);
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
        playerHP -= 20;
        yield return  new WaitForSeconds(0.1f);
        if(playerHP > 0)
        {
            animator.SetBool("isDamaged", true);
        }
        else
        {
            Debug.Log("Dead");
            animator.Play("Dead");
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
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
}
