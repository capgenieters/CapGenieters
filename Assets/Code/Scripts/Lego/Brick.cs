using UnityEngine;

public class Brick
{
    public Vector3Int size { get; private set; }
    public GameObject cube { get; private set; }
    public Vector3Brick previousPosition { get; private set; }
    public Vector3Brick position { get; private set; }

    private LegoTools tools;
    private bool canBuild, dropped;

    public Brick(LegoTools tools, Vector3Int size, Material material, bool canBuild = true, bool dropped = false)
    {
        this.size = size;
        this.canBuild = canBuild;
        this.dropped = dropped;
        this.tools = tools;
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (dropped)
            cube.AddComponent<Rigidbody>();

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

    /// <summary>
    /// Set the brick position in BrickSpace
    /// </summary>
    public void SetPosition(Vector3Brick position)
    {
        cube.transform.position = position.ToVector3(new Vector2Int(size.x, size.z));
        this.position = position;

        if (canBuild)
        {
            tools.AddBrickToBricks(this);
        }

        this.previousPosition = position;
    }

    /// <summary>
    /// Set the brick position in BrickSpace
    /// </summary>
    public void SetPosition(int x, int y, int z)
    {
        SetPosition(new Vector3Brick(x, y, z, tools.worldScale));
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

    public void SetActive(bool active)
    {
        cube.SetActive(active);
    }

    public void Destroy()
    {
        Object.Destroy(cube);
    }
}