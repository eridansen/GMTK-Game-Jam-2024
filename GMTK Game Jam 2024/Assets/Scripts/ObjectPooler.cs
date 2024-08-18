using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool
{
    public Pool() { }
    public Pool(Component type) { this.PoolType = type; }

    public readonly Component PoolType;
    
    public List<Component> Instances = new List<Component>();

    // To refactor. Find more descriptive name
    public int usedLastSessions = 0;
    public bool wasUsedThisSession = false;
}



public class ObjectPooler : MonoBehaviour
{
    private static List<Pool> poolsList = new List<Pool>();

    private static GameObject DefaultParent;
    private static GameObject ParticlePoolParent;
    private static GameObject SoundPoolParent;
    private static GameObject GameObjectPoolParent;
    public enum PoolType
    {
        None,
        Particle,
        Sound,
        GameObject,
    }

    private void Awake()
    {
        SetUpEmpties();
    }

    private void SetUpEmpties()
    {
        DefaultParent = new GameObject("Objects Pool");
        DefaultParent.transform.position = Vector3.zero;

        ParticlePoolParent = new GameObject("Particle Pool");
        ParticlePoolParent.transform.SetParent(DefaultParent.transform, false);

        SoundPoolParent = new GameObject("Sound Pool");
        SoundPoolParent.transform.SetParent(DefaultParent.transform, false);

        GameObjectPoolParent = new GameObject("Game object Pool");
        GameObjectPoolParent.transform.SetParent(DefaultParent.transform, false);
    }

    public static Component ProvideObject(Component component, Vector3 position, Quaternion rotation,
        PoolType poolType = PoolType.None)
    {
        Pool pool = null;
        // easier to debug, than with LINQ syntax
        foreach(Pool p in poolsList)
        {
            if (p.PoolType.GetType() == component.GetType())
            {
                pool = p;
                break;
            }
        }

        if(pool == null)
        {
            pool = new Pool(component);
            poolsList.Add(pool);
        }

        if (!pool.wasUsedThisSession)
        {
            pool.wasUsedThisSession = true;
            pool.usedLastSessions++;
        }

        Component instance = pool.Instances.FirstOrDefault();

        if(instance == null)
        {
            GameObject newInstance = null;

            // if no instances found, we populate pool with three new ones
            for (int i = 0; i < 3; i++)
            {
                GameObject parent = ParentGameObject(poolType);

                newInstance = Instantiate(component.gameObject, position, rotation);
                newInstance.transform.SetParent(parent.transform, false);
                newInstance.gameObject.SetActive(false);

                pool.Instances.Add(newInstance.GetComponent(component.GetType()));
            }

            // and return the last one added
            newInstance.gameObject.SetActive(true);
            return newInstance.GetComponent(component.GetType());
        }
        else
        {
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.gameObject.SetActive(true);

            pool.Instances.Remove(instance);
            return instance;
        }
    }

    public static void ReturnGameObject(Component component)
    {
        Pool pool = poolsList.Find((pool) => component.GetType() == pool.PoolType.GetType());

        if (pool == null)
        {
            Debug.LogWarning("No appropriate pool found");
        }
        else
        {
            component.gameObject.SetActive(false);
            pool.Instances.Add(component);
        }
    }
    private static GameObject ParentGameObject(PoolType type)
    {
        switch (type)
        {
            case PoolType.None:
                return DefaultParent;
            case PoolType.Particle:
                return ParticlePoolParent;
            case PoolType.Sound: 
                return SoundPoolParent;
            case PoolType.GameObject: 
                return GameObjectPoolParent;

            default: return null;
        }
    }
    private void OnDisable()
    {
        // remove list if this game object pool is not used
        foreach(var pool in poolsList)
        {
            if(pool.wasUsedThisSession == false)
                pool.usedLastSessions--;

            if(pool.usedLastSessions == -3)
                poolsList.Remove(pool);
        }
    }
}


