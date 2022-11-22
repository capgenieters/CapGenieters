using UnityEngine;

public class LegoElevator
{
    public Vector3Int position;
    public Vector2Int size;
    private LegoTools tools;
    private Material grey;

    public LegoElevator(LegoTools tools, Vector3Int position, Vector2Int size)
    {
        this.tools = tools;
        this.position = position;
        this.size = size;

        this.grey = tools.CreateMaterial(true);
        this.grey.color = new Color(0.63f, 0.63f, 0.63f, 0.5f);
    }

    public void GenerateElevator(Transform parent)
    {
        for (int x = position.x; x < position.x + size.x; x++)
            for (int z = position.z; z < position.z + size.y; z++)
                for (int fy = position.y; fy < position.y + 5; fy++)
                {
                    // Check if the current brick is a wall
                    if (x != position.x && x != position.x + size.x - 1)
                        continue;

                    if (z != position.z && z != position.z + size.y - 1)
                        continue;

                    // Create a new blueprint brick
                    Blueprint bp = new Blueprint(tools, 1, 1, grey);
                    bp.SetParent(parent);
                    bp.SetPosition(x, fy + 1, z);
                }
    }
} 
