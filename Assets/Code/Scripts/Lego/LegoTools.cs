using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegoTools
{
    public float worldScale;
    public GameObject stud;

    public List<Vector3Brick> bricks = new List<Vector3Brick>();
    public List<Brick> droppedBricksPool = new List<Brick>();

    public LegoTools(GameObject _stud, float _worldScale)
    {
        this.stud = _stud;
        this.worldScale = _worldScale;
    }

    /// <summary>
    /// This method allows you to get the y value for a x and z.
    /// </summary>
    public int GetTop(int x, int z)
    {
        for (int y = -5; y < 64; y++)
        {
            if (bricks.Contains(new Vector3Brick(x, y, z, worldScale)))
            {
                Debug.Log(x + ", " + y + ", " + z);
                return y;
            }
        }

        return 0;
    }

    /// <summary>
    /// This method will generate a random value in a certain range.
    /// The method will scale the number according to the world scale.
    /// </summary>
    public float RandomRange(float min, float max)
    {
        return Random.Range(min, max) * worldScale;
    }

    /// <summary>
    /// This method will round a float value to a certain factor using the modulo operator.
    /// <example>
    /// For example:
    /// <code>
    /// float f = 2.56f;
    /// f = Round(f, 0.3f);
    /// </code>
    /// results in <c>f</c> having the value (2.7f).
    /// </example>
    /// </summary>
    public float Round(float value, float factor)
    {
        // Use the modulo of the value to get the value to round off.
        float remainder = value % factor;
        // Check if the value should either round up, or down.
        float add = remainder < (factor / 2) ? -remainder : (factor - remainder);
        return value + add;
    }

    public Vector3 Round(Vector3 value, float factor)
    {
        return new Vector3(Round(value.x, factor), Round(value.y, factor), Round(value.z, factor));
    }

    /// <summary>
    /// This method will round a Vector3 world position to a BrickSpace position.
    /// <example>
    /// For example:
    /// <code>
    /// Vector3 p = new Vector3(0.36f, 1.63f, 1.94f);
    /// p = RoundToPlate(p.x, p.y, p.z);
    /// </code>
    /// results in <c>p</c> having the value (0, 1.6f, 2).
    /// </example>
    /// </summary>
    public Vector3Int RoundToPlate(float x, float y, float z)
    {
        return new Vector3Int((int)Mathf.Round(x), (int)(Round(y, 0.4f) * 3), (int)Mathf.Round(z));
    } 

    public Vector3Int RoundToPlate(Vector3 vec)
    {
        return RoundToPlate(vec.x, vec.y, vec.z);
    } 

    public Material CreateMaterial(bool isTransparent = false)
    {
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        mat.enableInstancing = true;

        if (isTransparent)
            MakeTransparent(mat);

        return mat;
    }

    public void MakeTransparent(Material material)
    {
        material.SetOverrideTag("SurfaceType", "Transparent");
    }

    /// <summary>
    /// This method will select a random item from a list.
    /// </summary>
    public T RandomListItem<T>(List<T> list)
    {
        int item = Random.Range(0, list.Count);
        return list[item];
    }

    /// <summary>
    /// This method is just a more advanced wrapper for the original instantiate.
    /// </summary>
    public GameObject Clone(GameObject obj, Vector3 pos, Quaternion rot = new Quaternion(), bool isActive = true, bool scaleToWorld = true)
    {
        Vector3 fPos = pos * (scaleToWorld ? worldScale : 1.0f);
        GameObject clone = GameObject.Instantiate(obj, fPos, rot);
        clone.transform.localScale *= scaleToWorld ? worldScale : 1.0f;
        clone.SetActive(isActive);
        return clone;
    }

    /// <summary>
    /// This method is just a more advanced wrapper for the original instantiate.
    /// </summary>
    public GameObject Clone(GameObject obj, Vector3Brick pos, Quaternion rot = new Quaternion(), bool isActive = true, bool scaleToWorld = true)
    {
        Vector3 fPos = pos.ToVector3();
        GameObject clone = GameObject.Instantiate(obj, fPos, rot);
        clone.transform.localScale *= scaleToWorld ? worldScale : 1.0f;
        clone.SetActive(isActive);
        return clone;
    }

    public void AddBrickToBricks(Brick brick)
    {
        for (int iteration = 0; iteration < 2; iteration++)
        {
            if (brick.previousPosition == null && iteration == 0)
                continue;

            for (int x = 0; x < brick.size.x; x++)
            {
                for (int y = 0; y < brick.size.y; y++)
                {
                    for (int z = 0; z < brick.size.z; z++)
                    {
                        if (iteration == 0)
                        {
                            bricks.Remove(brick.previousPosition + new Vector3Brick(x, y, z, worldScale));
                        }
                        else
                        {
                            Vector3Brick newPosition = brick.position + new Vector3Brick(x, y, z, worldScale);
                            bricks.Add(newPosition);
                            
                            if (newPosition.x == 40 && newPosition.z == 40)
                                Debug.Log(newPosition);
                        }
                    }
                }
            }
        }
    }

    public bool CanBePlaced(Vector3Brick position, Vector3Int size)
    {
        bool canBePlaced = true;

        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.z; z++)
            {
                if (!bricks.Contains(position + new Vector3Brick(x, -1, z, worldScale)))
                {
                    canBePlaced = false;
                }

                for (int y = 0; y < size.y; y++)
                {
                    if (bricks.Contains(position + new Vector3Brick(x, y, z, worldScale)))
                    {
                        canBePlaced = false;
                    }
                }
            }
        }

        return canBePlaced;
    }
}