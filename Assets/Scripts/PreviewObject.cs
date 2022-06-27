using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    private List<Collider> colliderList = new List<Collider>();
    [SerializeField]
    private int layerGround;
    private const int IGNORE_RAYCAST_LAYER = 2;
    [SerializeField]
    private Material green;
    [SerializeField]
    private Material red;
    void Start()
    {
        
    }

    void Update()
    {
        ChangeColor();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderList.Remove(other);
        }
    }
    private void ChangeColor()
    {
        if(colliderList.Count > 0)
        {
            SetColor(red);
        }
        else
        {
            SetColor(green);
        }
    }
    private void SetColor(Material mat)
    {
        foreach(Transform child in this.transform)
        {
            var newMaterial = new Material[child.GetComponent<Renderer>().materials.Length];
            for(int i = 0; i < newMaterial.Length; i++)
            {
                newMaterial[i] = mat;
            }
            child.GetComponent<Renderer>().materials = newMaterial;
        }
    }
    public bool isBuild()
    {
        return colliderList.Count == 0;
    }
}
