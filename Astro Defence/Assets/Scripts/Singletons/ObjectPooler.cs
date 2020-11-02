using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    
    public List<ObjectPool> pooledObjects;
    private Dictionary<string, ObjectPool> keyedPooledObjects = new Dictionary<string, ObjectPool>();

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        foreach (ObjectPool op in pooledObjects)
        {
            op.objects = new List<GameObject>();

            GameObject parent = new GameObject(name = op.objectName);
            parent.transform.parent = transform;

            GameObject temp;

            for (int i = 0; i < op.poolQuantity; i++)
            {
                temp = Instantiate(op.objectToPool, parent.transform);
                temp.SetActive(false);
                op.objects.Add(temp);
            }
            keyedPooledObjects.Add(op.objectToPool.name, op);
        }
        pooledObjects = null; //we don't need these now that we have converted them to a dictionary.

      //  gameObject.name = "Object Pooler";
    }

    public GameObject GetPooledObject(string name) //this is called when we want to spawn a new object. I.E bullet, asteroid, etc.
    {
        ObjectPool pool = keyedPooledObjects[name];
        if (pool == null) { Debug.Log("Didn't get object."); return null; }//we couldnt find the item in the list. Perhaps it hasn't been added to the list in the editor.

        for (int i = 0; i < pool.poolQuantity; i++)
        {
            if (!pool.objects[i].activeInHierarchy)
            {
                return pool.objects[i];
            }
        }
        return null;
    }

    public void ResetSceneObjects()
    {
        //we call this when the player dies and we want to "destroy" all powerups, asteroids, and bullets.
        foreach(KeyValuePair<string, ObjectPool> kv in keyedPooledObjects)
        {
            foreach(var op in kv.Value.objects)
            {
                op.SetActive(false);
            }
        }
    }
}
