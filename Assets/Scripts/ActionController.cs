using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range;
    private bool isPickUp = false;
    private RaycastHit ray;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private Text text;
    [SerializeField]
    private PlayerCtrl player;
    [SerializeField]
    private Inventory inventory;
    void Update() 
    {
        CheckItem();
        PickUp();   
    }

    private void PickUp()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            player.animator.SetBool("isPick", true);
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if(isPickUp)
        {
            if(ray.transform != null)
            {
                inventory.AcquireItem(ray.transform.GetComponent<ItemPickUp>().item);
                Destroy(ray.transform.gameObject);
                rayDisApear();
            }
        }
    }
    private void CheckItem()
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out ray, range, layerMask))
        {
            if(ray.transform.tag == "Item")
            {
                rayAppear();
            }
        }
        else
        {
            rayDisApear();
        }
    }

    private void rayAppear()
    {
        isPickUp = true;
        text.gameObject.SetActive(true);
        text.text = ray.transform.GetComponent<ItemPickUp>().item.itemName + " È¹µæ(F)";
    }

    private void rayDisApear()
    {
        isPickUp = false;
        text.gameObject.SetActive(false);
    }
}
