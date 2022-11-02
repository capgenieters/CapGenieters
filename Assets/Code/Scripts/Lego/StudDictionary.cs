using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudDictionary
{
    public Dictionary<Vector3, GameObject> map = new Dictionary<Vector3, GameObject>();
    public Vector3 fac = new Vector3(0.25f, 0.2f, 0.25f);

    public bool Add(Vector3 key, GameObject stud)
    {
        Vector3 fKey = Round(key, fac);
        bool containsStud = map.ContainsKey(fKey);

        if (!containsStud)
            map.Add(fKey, stud);

        return !containsStud;
    }

    public bool SafeRemove(Vector3 key)
    {
        Vector3 fKey = Round(key, fac);
        bool containsStud = map.ContainsKey(fKey);

        if (containsStud)
            map.Remove(fKey);

        return containsStud;
    }

    public void Remove(Vector3 key)
    {
        Vector3 fKey = Round(key, fac);

        map.Remove(fKey);
    }

    public GameObject SafeGet(Vector3 key)
    {
        Vector3 fKey = Round(key, fac);

        return map.ContainsKey(fKey) ? map[fKey] : null;
    }

    public GameObject Get(Vector3 key)
    {
        Vector3 fKey = Round(key, fac);

        return map[fKey];
    }

    public bool Has(Vector3 key)
    {
        Vector3 fKey = Round(key, fac);

        return map.ContainsKey(fKey);
    }

    public float Round(float value, float factor)
    {
        // Use the modulo of the value to get the value to round off.
        float remainder = value % factor;
        // Check if the value should either round up, or down.
        float add = remainder < (factor / 2) ? -remainder : (factor - remainder);
        return value + add;
    }

    public Vector3 Round(Vector3 value, Vector3 factor)
    {
        return new Vector3(Round(value.x, factor.x), Round(value.y, factor.y), Round(value.z, factor.z));
    }
}