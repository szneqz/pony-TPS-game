using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public int amountToPool;
    public GameObject objectToPool;
    public bool shouldExpand;
}

public class PoolScript : MonoBehaviour {

    public List<ObjectPoolItem> itemsToPool;

    List<GameObject> pooledObjects;
    public static PoolScript SharedInstance;

    private WepList wepScript;
    public List<GameObject> weapons;

    void Awake()
    {
        SharedInstance = this;
    }

    void searchTrough() //skrypt przeszukuje sobie skrypt w NDGO przez wszystkie instancje broni, zeby stworzyc sobie ladna liste broni posegregowanych od 0 do MAX
    {                   //UWAGA! skrypt sie wysypie jezeli bedzie dziura w numeracji gunID, wiec nalezy zachowac kolejnosc!
        weapons.Clear();
        for (int i = 0; ; i++)
        {
            bool ifstop = false;
            foreach (datList a in wepScript.guns)
            {
                foreach (GameObject b in a.pri)
                {
                    if (b.GetComponent<GunAddInfo>().weaponNr == i)
                    {
                        weapons.Add(b);
                        ifstop = true;
                        break;
                    }
                }
                if (ifstop)
                    break;
                foreach (GameObject b in a.sec)
                {
                    if (b.GetComponent<GunAddInfo>().weaponNr == i)
                    {
                        weapons.Add(b);
                        ifstop = true;
                        break;
                    }
                }
                if (ifstop)
                    break;
                foreach (GameObject b in a.mel)
                {
                    if (b.GetComponent<GunAddInfo>().weaponNr == i)
                    {
                        weapons.Add(b);
                        ifstop = true;
                        break;
                    }
                }
                if (ifstop)
                    break;
            }
            if (!ifstop)
                break;
        }
    }

    void Start()
    {
        wepScript = StaticInfo.datScript.GetComponent<WepList>();
        searchTrough();

        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
        //NIE MA SPAWNOWANIA BO MULTI SOBIE AUTOMATYCZNIE POSPAWNUJE
        //this.gameObject.AddComponent<SpawnPlayers>();   //odwolanie do spawnu graczy po wywolaniu tej funkcji
    }

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
}
