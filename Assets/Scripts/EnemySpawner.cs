using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject monsterSpawner = null;

    public List<GameObject> monsters = new List<GameObject>();

    public int spawnMaxCnt = 50;

    void Spawn()
    {
        if (monsters.Count > spawnMaxCnt)
        {
            return;
        }

        Vector3 vecSpawn = new Vector3(Random.Range(10, 100), 1f, Random.Range(10, 100));

        Ray ray = new Ray(vecSpawn, Vector3.down);

        RaycastHit raycastHit = new RaycastHit();
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity) == true)
        {
            vecSpawn.y = raycastHit.point.y;
        }

        GameObject newMonster = Instantiate(monsterSpawner, vecSpawn, Quaternion.identity);

        monsters.Add(newMonster);
    }

    private void Start()
    {
        InvokeRepeating("Spawn", 3f, 5f);
    }
}
