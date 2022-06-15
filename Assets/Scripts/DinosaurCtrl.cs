using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;
public class DinosaurCtrl : MonoBehaviour
{
    public enum RapterState { None, Idle, Move, Wait, GoTarget, Atk, Damage, Die }

    public RapterState rapterState = RapterState.None;
    public float spdMove = 1f;
    public GameObject targetCharactor = null;
    public Transform targetTransform = null;
    public Vector3 posTarget = Vector3.zero;

    private Animation rapterAnimation = null;
    private Transform rapterTransform = null;

    public AnimationClip IdleAnimClip = null;
    public AnimationClip MoveAnimClip = null;
    public AnimationClip AtkAnimClip = null;
    public AnimationClip DamageAnimClip = null;
    public AnimationClip DieAnimClip = null;

    public int hp = 100;
    public float AtkRange = 1.5f;
    public GameObject effectDamage = null;
    public GameObject effectDie = null;

    private Tweener effectTweener = null;
    private SkinnedMeshRenderer skinnedMeshRenderer = null;


    void OnAtkAnmationFinished()
    {
        Debug.Log("Atk Animation finished");
    }

    void OnDmgAnmationFinished()
    {
        Debug.Log("Dmg Animation finished");
    }

    void OnDieAnmationFinished()
    {
        Debug.Log("Die Animation finished");

        gameObject.SetActive(false);
    }

    void OnAnimationEvent(AnimationClip clip, string funcName)
    {
        AnimationEvent retEvent = new AnimationEvent();
        retEvent.functionName = funcName;
        retEvent.time = clip.length - 0.1f;
        clip.AddEvent(retEvent);
    }



    void Start()
    {

        rapterState = RapterState.Idle;

        rapterAnimation = GetComponent<Animation>();
        rapterTransform = GetComponent<Transform>();

        rapterAnimation[IdleAnimClip.name].wrapMode = WrapMode.Loop;
        rapterAnimation[MoveAnimClip.name].wrapMode = WrapMode.Loop;
        rapterAnimation[AtkAnimClip.name].wrapMode = WrapMode.Once;
        rapterAnimation[DamageAnimClip.name].wrapMode = WrapMode.Once;

        rapterAnimation[DamageAnimClip.name].layer = 10;
        rapterAnimation[DieAnimClip.name].wrapMode = WrapMode.ClampForever;
        rapterAnimation[DieAnimClip.name].layer = 10;

        OnAnimationEvent(AtkAnimClip, "OnAtkAnmationFinished");
        OnAnimationEvent(DamageAnimClip, "OnDmgAnmationFinished");
        OnAnimationEvent(DieAnimClip, "OnDieAnmationFinished");

        skinnedMeshRenderer = rapterTransform.Find("Retopo").GetComponent<SkinnedMeshRenderer>();
    }

    void CkState()
    {
        switch (rapterState)
        {
            case RapterState.Idle:
                SetIdle();
                break;
            case RapterState.GoTarget:
            case RapterState.Move:
                SetMove();
                break;
            case RapterState.Atk:
                SetAtk();
                break;
            default:
                break;
        }
    }

    void Update()
    {
        CkState();
        AnimationCtrl();
    }

    void SetIdle()
    {
        if (targetCharactor == null)
        {
            posTarget = new Vector3(rapterTransform.position.x + Random.Range(-10f, 10f),
                                    rapterTransform.position.y + 1000f,
                                    rapterTransform.position.z + Random.Range(-10f, 10f)
                );
            Ray ray = new Ray(posTarget, Vector3.down);
            RaycastHit infoRayCast = new RaycastHit();
            if (Physics.Raycast(ray, out infoRayCast, Mathf.Infinity) == true)
            {
                posTarget.y = infoRayCast.point.y;
            }
            rapterState = RapterState.Move;
        }
        else
        {
            rapterState = RapterState.GoTarget;
        }
    }

    void SetMove()
    {
        Vector3 distance = Vector3.zero;
        Vector3 posLookAt = Vector3.zero;

        switch (rapterState)
        {
            case RapterState.Move:
                if (posTarget != Vector3.zero)
                {
                    distance = posTarget - rapterTransform.position;

                    if (distance.magnitude < AtkRange)
                    {
                        StartCoroutine(SetWait());
                        return;
                    }

                    posLookAt = new Vector3(posTarget.x, rapterTransform.position.y, posTarget.z);
                }
                break;
            case RapterState.GoTarget:
                if (targetCharactor != null)
                {
                    distance = targetCharactor.transform.position - rapterTransform.position;
                    if (distance.magnitude < AtkRange)
                    {
                        rapterState = RapterState.Atk;
                        return;
                    }
                    posLookAt = new Vector3(targetCharactor.transform.position.x, rapterTransform.position.y, targetCharactor.transform.position.z);
                }
                break;
            default:
                break;

        }

        Vector3 direction = distance.normalized;

        direction = new Vector3(direction.x, 0f, direction.z);

        Vector3 amount = direction * spdMove * Time.deltaTime;

        rapterTransform.Translate(amount, Space.World);
        rapterTransform.LookAt(posLookAt);
        transform.rotation *= Quaternion.Euler(new Vector3(0f, 180f, 0f));

    }

    IEnumerator SetWait()
    {
        rapterState = RapterState.Wait;
        float timeWait = Random.Range(1f, 3f);
        yield return new WaitForSeconds(timeWait);
        rapterState = RapterState.Idle;
    }

    void AnimationCtrl()
    {
        switch (rapterState)
        {
            case RapterState.Wait:
            case RapterState.Idle:
                rapterAnimation.CrossFade(IdleAnimClip.name);
                break;
            case RapterState.Move:
            case RapterState.GoTarget:
                rapterAnimation.CrossFade(MoveAnimClip.name);
                break;
            case RapterState.Atk:
                rapterAnimation.CrossFade(AtkAnimClip.name);
                break;
            case RapterState.Die:
                rapterAnimation.CrossFade(DieAnimClip.name);
                break;
            default:
                break;

        }
    }

    void OnCkTarget(GameObject target)
    {
        targetCharactor = target;
        targetTransform = targetCharactor.transform;

        rapterState = RapterState.GoTarget;
    }

    void SetAtk()
    {
        float distance = Vector3.Distance(targetTransform.position, rapterTransform.position); 

        if (distance > AtkRange + 0.5f)
        {
            rapterState = RapterState.GoTarget;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAtk") == true)
        {
            hp -= 10;
            if (hp > 0)
            {
                rapterAnimation.CrossFade(DamageAnimClip.name);

                effectDamageTween();
            }
            else
            {
                rapterState = RapterState.Die;
            }
        }
    }

    void effectDamageTween()
    {
        if (effectTweener != null && effectTweener.isComplete == false)
        {
            return;
        }

        Color colorTo = Color.red;

        effectTweener = HOTween.To(skinnedMeshRenderer, 0.2f, new TweenParms().Prop("color", colorTo).Loops(1, LoopType.Yoyo).OnStepComplete(OnDamageTweenFinished));
    }

    void OnDamageTweenFinished()
    {
        skinnedMeshRenderer.material.color = Color.white;
    }
}
