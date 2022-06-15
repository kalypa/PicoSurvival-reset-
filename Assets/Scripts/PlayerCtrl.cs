using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float movespd = 2.0f;
    public float runMoveSpd = 3.5f;
    public float directRotateSpd = 100.0f;
    public float bodyRotateSpd = 2.0f;
    public float velocityChangeSpd = 0.1f;
    private Vector3 currentVelocitySpd = Vector3.zero;
    private Vector3 moveDirect = Vector3.zero;
    public bool stopMove = false;
    private CharacterController characterCtrl = null;
    private CollisionFlags collisionFlags = CollisionFlags.None;
    public CapsuleCollider AtkCapsuleCollider = null;
    public SphereCollider AtkSphereCollider = null;
    private readonly float gravity = 9.8f;
    public float verticalSpd = 0;
    private Animator animator = null;
    public enum PlayerState { None, Idle, Walk, Run, Charge, Atk, PickUp}
    public enum AtkState { None, Punch, Sword, Axe, PickAxe }
    public PlayerState playerState = PlayerState.None;
    public AtkState atkState = AtkState.None;
    public bool flagNextAttack = false;
    private Inventory inventory;
    private InventoryUI UI;
    void Start()
    {
        UI = GetComponent<InventoryUI>();
        characterCtrl = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();

        playerState = PlayerState.Idle;
        inventory = GetComponent<Inventory>();
    }


    void Update()
    {
        Move();
        SetGravity();
        BodyDirectChange();
        CkAnimationState();
        InputAttackCtrll();
        AtkAnimationCtrl();
        AtkComponentCtrl();
    }

    void Move()
    {
        Transform CameraTransform = Camera.main.transform;

        Vector3 forward = CameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;

        Vector3 right = new(forward.z, 0.0f, -forward.x);

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        Vector3 targetDirect = horizontal * right + vertical * forward;

        moveDirect = Vector3.RotateTowards(moveDirect, targetDirect, directRotateSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        
        moveDirect = moveDirect.normalized;

        float spd = movespd;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            spd = runMoveSpd;
        }

        Vector3 amount = (moveDirect * spd * Time.deltaTime);

        collisionFlags = characterCtrl.Move(amount);
    }

    float GetVelocitySpd()
    {
        if (characterCtrl.velocity == Vector3.zero)
        {
            currentVelocitySpd = Vector3.zero;
        }
        else
        {
            Vector3 retVelocity = characterCtrl.velocity;
            retVelocity.y = 0;
            currentVelocitySpd = Vector3.Lerp(currentVelocitySpd, retVelocity, velocityChangeSpd * Time.fixedDeltaTime);
        }
        return currentVelocitySpd.magnitude;
    }

    private void OnGUI()
    {
        var labelStyle = new GUIStyle
        {
            fontSize = 200
        };
        labelStyle.normal.textColor = Color.white;
        if (Input.GetKey(KeyCode.Escape))
        {
            if (GUI.Button(new Rect(800, 400, 200, 100), "Exit", labelStyle))
            {
                Application.Quit();
            }
        }

    }

    void BodyDirectChange()
    {
        if (GetVelocitySpd() > 0.0f)
        {
            Vector3 newForward = characterCtrl.velocity;
            newForward.y = 0;

            transform.forward = Vector3.Lerp(transform.forward, newForward, bodyRotateSpd * Time.deltaTime);
        }
    }

    void CkAnimationState()
    {
        float nowspd = GetVelocitySpd();

        switch (playerState)
        {
            case PlayerState.None:
                break;
            case PlayerState.Idle:
                animator.SetFloat("MoveSpd", 0.0f);
                if (nowspd > 0.0f)
                {
                    playerState = PlayerState.Walk;
                }
                break;
            case PlayerState.Walk:
                animator.SetFloat("MoveSpd", 0.1f);
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    playerState = PlayerState.Charge;
                }
                else if (nowspd < 0.01f)
                {
                    playerState = PlayerState.Idle;
                }
                break;
            case PlayerState.Charge:
                animator.SetFloat("MoveSpd", 1f);
                if (nowspd < 0.5f || Input.GetKeyUp(KeyCode.LeftShift))
                {
                    playerState = PlayerState.Walk;
                }

                if (nowspd < 0.01f)
                {
                    playerState = PlayerState.Idle;
                }
                break;
            case PlayerState.Atk: 
                break;
            case PlayerState.PickUp:
                animator.SetBool("isPick", true);
                break;
            default:
                break;
        }
    }

    void AtkAnimationCtrl()
    {
        switch (atkState)
        {
            case AtkState.Punch:
                if (playerState == PlayerState.Atk)
                    animator.SetBool("isPunch", true);
                break;
            case AtkState.Axe:
                if(playerState == PlayerState.Atk)
                animator.SetBool("isAtk", true);
                break;
        }
    }
    void InputAttackCtrll()
    {
        //if (UI.inventory == false)
        //{
            if (Input.GetMouseButtonDown(0) == true)
            {
                if (playerState != PlayerState.Atk)
                {
                    playerState = PlayerState.Atk;
                    if (inventory.hasweapon[0] == true)
                    {
                        atkState = AtkState.Axe;
                    }
                    else
                    {
                        atkState = AtkState.Punch;
                    }
                }
                Debug.Log(atkState);
            }
        //}
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

    void AtkComponentCtrl()
    {
        switch (playerState)
        {
            case PlayerState.Atk:
                AtkCapsuleCollider.enabled = true;
                AtkSphereCollider.enabled = true;
                break;
            default:
                AtkCapsuleCollider.enabled = false;
                AtkSphereCollider.enabled = false;
                break;
        }
    }
    public void PickUpEnd()
    {
        playerState = PlayerState.Idle;
        animator.SetBool("isPick", false);
    }
    public void AtkEnd()
    {
        animator.SetBool("isAtk", false);
        playerState = PlayerState.Idle;
    }
    public void PunchEnd()
    {
        animator.SetBool("isPunch", false);
        playerState = PlayerState.Idle;
    }
}
