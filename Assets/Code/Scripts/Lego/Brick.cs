using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick
{
    public float sizeX;
    public float sizeZ;
    public float height;
    public GameObject cube { get; private set; }

    private LegoTools tools;
    private Vector3 offset;
    private StudDictionary myStuds;
    private bool canBuild, dropped;

    public Brick(LegoTools _tools, float _sizeX, float _sizeZ, Material material, float _height = 1.2f, bool _canBuild = true, bool _dropped = false)
    {
        sizeX = _sizeX;
        sizeZ = _sizeZ;
        height = _height;
        canBuild = _canBuild;
        dropped = _dropped;
        tools = _tools;
        myStuds = new StudDictionary();
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        offset = new Vector3(Mathf.Floor(sizeX / 2) / 2, 0, Mathf.Floor(sizeZ / 2) / 2);

        if (dropped)
            cube.AddComponent<Rigidbody>();

        cube.transform.localScale = new Vector3(sizeX, height, sizeZ) * tools.worldScale;

        for (float x = 0; x < sizeX; x++)
            for (float z = 0; z < sizeZ ; z++)
            {
                float fX = x - (sizeX / 2) + 0.5f;
                float fZ = z - (sizeZ / 2) + 0.5f;

                Vector3 studPos = new Vector3(fX, 0.5f * height, fZ);
                GameObject newStud = tools.Clone(tools.stud, studPos);
                newStud.transform.parent = cube.transform;

                if (canBuild && !dropped)
                {
                    tools.studs.Add(studPos, newStud);
                    myStuds.Add(studPos, newStud);
                }
            }

        foreach (Renderer rend in cube.GetComponentsInChildren<Renderer>())
            rend.material = material;
    }

    public void SetParent(Transform parent)
    {
        cube.transform.parent = parent;
    }

    public void AddChild(Transform child)
    {
        child.parent = cube.transform;
    }

    public void AddChild(Brick child)
    {
        child.SetParent(cube.transform);
    }

    public void SetPosition(Vector3 position)
    {
        cube.transform.position = position * tools.worldScale + offset * tools.worldScale;

        if (canBuild && !dropped)
            foreach(KeyValuePair<Vector3, GameObject> v in myStuds.map)
            {
                Vector3 oldPosition = v.Key;
                Vector3 pStud = v.Value.transform.position / tools.worldScale;

                tools.studs.Add(pStud, v.Value);
                tools.studs.Remove(oldPosition);
            }
    }

    public void SetPosition(float x, float y, float z)
    {
        SetPosition(new Vector3(x, y, z));
    }

    /// <summary>
    /// Only works for dropped and non-build bricks.
    /// Can be used for lerping
    /// </summary>
    public void SetAbsolutePosition(Vector3 position)
    {
        if (!canBuild || dropped)
            cube.transform.position = position;
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

    public void BuildBrick(GameObject stud)
    {
        if (!canBuild || dropped)
            return;
            
        Vector3 studPos = stud.transform.position / tools.worldScale;
        studPos.y += this.cube.transform.localScale.y / tools.worldScale / 2;
        SetPosition(studPos);
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