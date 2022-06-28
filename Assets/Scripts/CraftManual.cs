using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Craft
{
    public string craftName;
    public GameObject buildPrefab;
    public GameObject previewPrefab;
}
public class CraftManual : MonoBehaviour
{
    private bool isActive = false;
    public bool isPreview = false;
    [SerializeField]
    public GameObject craftUI;
    [SerializeField]
    private Craft[] craftBox;
    private GameObject preview;
    private GameObject prefab;
    private RaycastHit hit;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Inventory inven;
    public void SlotClick(int slotNum)
    {
        preview = Instantiate(craftBox[slotNum].previewPrefab, player.position + player.forward, Quaternion.identity);
        prefab = craftBox[slotNum].buildPrefab;
        isPreview = true;
        craftUI.SetActive(false);
    }
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isPreview)
        {
            isActive = !isActive;
            craftUI.SetActive(isActive);
        }
        if(isPreview)
        {
            PreviewPositionUpdate();
        }
        if(Input.GetButtonDown("Fire1") && isPreview && inven.slot[0].itemCount >= 9)
        {
            Build();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Cancel();
        }
    }
    private void Build()
    {
        if(isPreview && preview.GetComponent<PreviewObject>().isBuild())
        {
            Instantiate(prefab, hit.point, Quaternion.identity);
            Destroy(preview);
            isActive = false;
            isPreview = false;
            preview = null;
            prefab = null;
            craftUI.SetActive(false);
            inven.slot[0].itemCount -= 9;
        }
    }
    private void Cancel()
    {
        if(isPreview)
        {
            Destroy(preview);
        }
        isActive = false;
        isPreview = false;
        preview = null;
        craftUI.SetActive(false);
    }
    private void PreviewPositionUpdate()
    {
        if(Physics.Raycast(player.position, player.forward, out hit, range, layerMask))
        {
            if(hit.transform != null)
            {
                Vector3 location = hit.point;
                preview.transform.position = location;
            }
        }
    }
}
