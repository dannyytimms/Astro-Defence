using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Poolable Object", menuName = "ScriptableObjects/ObjectPoolScriptableObject", order = 1)]
public class ObjectPool : ScriptableObject
{
    public string objectName { get { return objectToPool.name; } }
    public List<GameObject> objects { get; set; }
    public GameObject objectToPool;
    public int poolQuantity;
}