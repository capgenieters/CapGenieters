using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick
{
    private LegoTools tools;
    private StudDictionary myStuds;
    private GameObject cube;
    private bool canBuild;

    public Brick(LegoTools _tools, float sizeX, float sizeY, float sizeZ, Material material, float height = 0.6f, bool _canBuild = true, bool canCollide = true)
    {
        canBuild = _canBuild;
        tools = _tools;
        myStuds = new StudDictionary();
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (!canCollide)
            Object.DestroyImmediate(cube.GetComponent<Collider>());

        cube.transform.localScale = new Vector3(sizeX * 0.5f, sizeY * height, sizeZ * 0.5f) * tools.worldScale;

        for (float x = 0; x < sizeX; x++)
        {
            for (float z = 0; z < sizeZ ; z++)
            {
                float fX = x / 2 - (sizeX / 4) + 0.25f;
                float fZ = z / 2 - (sizeZ / 4) + 0.25f;

                Vector3 studPos = new Vector3(fX, sizeY / 2 * height, fZ);

                GameObject newStud = tools.Clone(tools.stud, studPos);
                newStud.transform.parent = cube.transform;

                if (canBuild)
                {
                    tools.studs.Add(studPos, newStud);
                    myStuds.Add(studPos, newStud);
                }
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

    Vector3 oldPosition, pStud;
    public void SetPosition(Vector3 position)
    {
        cube.transform.position = position * tools.worldScale;

        if (canBuild)
            foreach(KeyValuePair<Vector3, GameObject> v in myStuds.map)
            {
                oldPosition = v.Key;
                pStud = v.Value.transform.position / tools.worldScale;

                tools.studs.Add(pStud, v.Value);
                tools.studs.Remove(oldPosition);
            }
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

    public void BuildBrick(GameObject stud)
    {
        if (!canBuild)
            return;
            
        Vector3 studPos = stud.transform.position / tools.worldScale;
        studPos.y += this.cube.transform.localScale.y / tools.worldScale / 2;
        SetPosition(studPos);
    }

    public void SetActive(bool active)
    {
        cube.SetActive(active);
    }
}