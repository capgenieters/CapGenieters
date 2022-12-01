using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint
{
    public Vector3Int size;
    public GameObject cube { get; private set; }

    private LegoTools tools;

    public Blueprint(LegoTools tools, Vector3Int size, Material material, bool filled = false)
    {
        this.size = size;
        this.tools = tools;
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Scale the bricks down to worldScale
        cube.transform.localScale = new Vector3(size.x, size.y * 0.4f, size.z) * tools.worldScale;

        // Add studs to the brick
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.z ; z++)
            {
                Vector3 studPos = new Vector3(x - 0.5f, size.y * 0.2f, z - 0.5f);
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

    /// <summary>
    /// Set the brick position in BrickSpace
    /// </summary>
    public void SetPosition(Vector3Brick position)
    {
       cube.transform.position = position.ToVector3(new Vector2Int(size.x, size.z));
    }

    /// <summary>
    /// Set the brick position in BrickSpace
    /// </summary>
    public void SetPosition(int x, int y, int z)
    {
        SetPosition(new Vector3Brick(x, y, z, tools.worldScale));
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