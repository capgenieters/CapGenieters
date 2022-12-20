using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LegoElevator
{
    public Vector3Int position;
    public Vector2Int size;
    public List<Blueprint> blueprints = new List<Blueprint>();
    
    private LegoTools tools;
    private Material grey;

    public LegoElevator(LegoTools tools, Vector3Int position, Vector2Int size)
    {
        this.tools = tools;
        this.position = position + new Vector3Int(0, 2, 0);
        this.size = size;

        this.grey = tools.CreateMaterial(true);
        this.grey.color = new Color(0.63f, 0.63f, 0.63f, 0.3f);
    }

    public void GenerateElevator(Transform parent)
    {
        for (int x = position.x; x < position.x + size.x; x++)
            for (int z = position.z; z < position.z + size.y; z++)
                for (int fy = position.y; fy < position.y + 15; fy += 3)
                {
                    // Check if the current brick is a wall
                    if (x != position.x && x != position.x + size.x - 1)
                        if (z != position.z && z != position.z + size.y - 1)
                            continue;

                    // Create a new blueprint brick
                    Blueprint bp = new Blueprint(tools, new Vector3Int(1, 3, 1), grey);
                    bp.SetParent(parent);
                    bp.SetPosition(x, fy + 1, z);
                    blueprints.Add(bp);
                }
    }
} 
