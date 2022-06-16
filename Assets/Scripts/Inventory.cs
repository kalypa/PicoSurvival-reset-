using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion
    public List<Item> item = new();
    public delegate void OnSlotItem();
    public OnSlotItem onSlotItem;
    private bool fdown;
    private bool onedown;
    private bool zerodown;
    public GameObject nearObject;
    public GameObject[] weapon;
    public bool[] hasweapon;
    public bool[] hasItem;
    private PlayerCtrl player;
    private Slot slot;
    private void Start()
    {
        player = GetComponent<PlayerCtrl>();
        slot = FindObjectOfType<Slot>();
    }
    void Update()
    {
        Interation();
        GetInput();
        Swap();
    }

    public bool AddItem(Item _item)
    {
        if (item.Count < 19)
        {
            item.Add(_item);
            if (onSlotItem != null)
            {
                onSlotItem.Invoke();
            }
            return true;
        }
        return false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            nearObject = other.gameObject;
        }
        else if (other.CompareTag("Ingredient"))
        {
            nearObject = other.gameObject;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            nearObject = null;
        }
        else if (other.CompareTag("Ingredient"))
        {
            nearObject = null;
        }
    }
    void Interation()
    {
        if (fdown && nearObject != null)
        {
            player.animator.SetBool("isPick", true);
            //player.playerState = PlayerCtrl.PlayerState.PickUp;
            Item item = nearObject.GetComponent<Item>();
            if (nearObject.CompareTag("Weapon"))
            {
                int weaponIndex = item.value;
                AddItem(item);
                hasweapon[weaponIndex] = true;
            }
            else if (nearObject.CompareTag("Ingredient") || nearObject.CompareTag("Food"))
            {
                int itemIndex = item.value;
                if(slot.itemCount == 0)
                {
                    AddItem(item);
                }
                hasItem[itemIndex] = true;
            }
            Destroy(nearObject);
        }
    }
    void Swap()
    {
        int weaponIndex = -1;
        if (onedown)
        {
            weaponIndex = 0;
        }
        if (onedown && hasweapon[weaponIndex] == true)
        {
            weapon[weaponIndex].SetActive(true);
        }
        if (zerodown)
        {
            weaponIndex = 0;
            weapon[weaponIndex].SetActive(false);
        }
    }
    void GetInput()
    {
        fdown = Input.GetKeyDown(KeyCode.F);
        zerodown = Input.GetKeyDown(KeyCode.Alpha0);
        onedown = Input.GetKeyDown(KeyCode.Alpha1);
    }
}
