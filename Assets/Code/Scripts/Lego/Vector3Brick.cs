using UnityEngine;

public class Vector3Brick
{
    private Vector3Int position;
    public int x
    { 
        get { return position.x; }
        set { position.x = value; } 
    }
    public int y
    { 
        get { return position.y; }
        set { position.y = value; } 
    }
    public int z
    { 
        get { return position.z; }
        set { position.z = value; } 
    }

    float worldScale;

    public Vector3Brick(Vector3Int position, float worldScale)
    {
        this.position = position;
        this.worldScale = worldScale;
    }

    public Vector3Brick(int x, int y, int z, float worldScale)
    {
        this.position = new Vector3Int(x, y, z);
        this.worldScale = worldScale;
    }

    public Vector3Brick(Vector3 position, float worldScale = 1)
    {
        this.x = (int)(position.x / worldScale);
        this.z = (int)(position.z / worldScale);

        // First calculate the global y, then divide by 0.4f to convert to brick space
        float ly = position.y / worldScale;
        this.y = (int)(ly / 0.4f);
        this.worldScale = worldScale;
    }

    public Vector3 ToVector3(Vector2Int brickSize = new Vector2Int())
    {
        Vector3 offset = Vector3.zero;
        if (brickSize != new Vector2Int())
        {
            offset = new Vector3(brickSize.x * 0.25f * worldScale, 0, brickSize.y * 0.25f * worldScale);
        }
        
        return new Vector3(x * worldScale, y * 0.4f * worldScale, z * worldScale) + offset;
    }
}