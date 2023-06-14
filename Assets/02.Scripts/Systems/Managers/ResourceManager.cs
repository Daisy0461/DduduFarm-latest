using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : SingletonBase<ResourceManager>
{
    public bool TryGetResource<T>(string resourceName, out T resource) where T : MonoBehaviour
    {
        resource = null;
        
        var prefab = Resources.Load<T>(resourceName);
        if (prefab == null) return false;

        resource = MonoBehaviour.Instantiate(prefab);
        if (resource == null) return false;
        return true;
    }
}
