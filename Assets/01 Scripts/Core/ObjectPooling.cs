using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : Singleton<ObjectPooling>
{
    Dictionary<GameObject, List<GameObject>> poolDictionary = new Dictionary<GameObject, List<GameObject>>();

    public virtual GameObject GetOBJ(GameObject prefab)
    {
        List<GameObject> objectList = new List<GameObject>();

        if (poolDictionary.ContainsKey(prefab))
        {
            objectList = poolDictionary[prefab];
        }
        else
        {
            poolDictionary.Add(prefab, objectList);
        }

        foreach (GameObject obj in objectList)
        {
            if (obj.activeSelf)
            {
                continue;
            }
            return obj;
        }
        GameObject newObj = Instantiate(prefab, this.transform.position, Quaternion.identity);
        objectList.Add(newObj);
        return newObj;
    }

    public virtual T GetCOMP<T>(T prefab) where T : MonoBehaviour
    {
        return GetOBJ(prefab.gameObject).GetComponent<T>();
    }
}
