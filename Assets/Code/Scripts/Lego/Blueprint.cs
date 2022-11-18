using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint
{
   public float sizeX;
    public float sizeZ;
    public float height;
    public GameObject cube { get; private set; }

    private Vector3 offset;
    private LegoTools tools;
    private StudDictionary myStuds;
    private bool canBuild, dropped;

    public Blueprint(LegoTools _tools, float _sizeX, float _sizeZ, Material material, float _height = 1.2f, bool filled = false)
    {
        sizeX = _sizeX;
        sizeZ = _sizeZ;
        height = _height;
        tools = _tools;
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        offset = new Vector3(Mathf.Floor(sizeX / 2) / 2, 0, Mathf.Floor(sizeZ / 2) / 2);

        cube.transform.localScale = new Vector3(sizeX, height, sizeZ) * tools.worldScale;

        for (float x = 0; x < sizeX; x++)
        {
            for (float z = 0; z < sizeZ ; z++)
            {
                float fX = x - (sizeX / 2) + 0.5f;
                float fZ = z - (sizeZ / 2) + 0.5f;

                Vector3 studPos = new Vector3(fX, 0.5f * height, fZ);
                GameObject newStud = tools.Clone(tools.stud, studPos);
                newStud.transform.parent = cube.transform;
            }
        }

        foreach (Renderer rend in cube.GetComponentsInChildren<Renderer>())
            rend.material = material;

        SetFilled(filled);
    }

    public bool GetFilled()
    {
        foreach (Renderer rend in cube.GetComponentsInChildren<Renderer>())
            return rend.material.color.a == 0.7;

        return false;
    }

    public void SetFilled(bool filled)
    {
        foreach (Renderer rend in cube.GetComponentsInChildren<Renderer>())
        {
            Color c = rend.material.color;
            c.a = filled ? 0.7f : 1f;
            rend.material.color = c;
        }
    }

    public void SetParent(Transform parent)
    {
        cube.transform.parent = parent;
    }

    public void SetPosition(Vector3 position)
    {
        cube.transform.position = (position * tools.worldScale) + (offset * tools.worldScale);
    }

    public void SetPosition(float x, float y, float z)
    {
        SetPosition(new Vector3(x, y, z));
    }

    public void SetRotation(Quaternion rotation)
    {
        cube.transform.rotation = rotation;

        // TODO: Either fix studs for this, or remove the studs to prevent building errors
    }

    public Vector3 GetScale()
    {
        return cube.transform.localScale;
    }

    public void ScaleBrick(float amount)
    {
        Vector3 scale = cube.transform.localScale;
        cube.transform.localScale = scale * amount;
    }

    public void SetActive(bool active)
    {
        cube.SetActive(active);
    }

    public void Destroy()
    {
        Object.Destroy(cube);
    }
}