using System.Collections.Generic;
using UnityEngine;

public class LegoItem
{
    public Vector3Int size;
    public int amount = 1;

    public LegoItem(Brick brick)
    {
        this.size = brick.size;
    }

    public LegoItem(int sizeX, int sizeY, int sizeZ)
    {
        this.size = new Vector3Int(sizeX, sizeY, sizeZ);
    }

    public LegoItem(Vector3Int size)
    {
        this.size = size;
    }
}

public class LegoInventory
{
    public List<LegoItem> items { get; private set; }
    public int selectedIndex { get; private set; } = 0;
    private Brick displayBrick;
    private Vector3 displayPosition;
    private LegoTools tools;

    public LegoInventory(LegoTools _tools) 
    {
        items = new List<LegoItem>();
        tools = _tools;

        UpdateItems();
    }

    // TODO: Know what it does
    private void UpdateItems() 
    {
        if (items.Count == 0)
            return;

        int i = selectedIndex;
        if (items.Count <= selectedIndex)
            i = items.Count - 1;

        Vector3 displayPosition = Vector3.zero;
        if (displayBrick != null) 
        {
            displayPosition = displayBrick.cube.transform.position;
            displayBrick.Destroy();
        }

        LegoItem item = items[i];
        displayBrick = new Brick(tools, item.size, tools.CreateMaterial(), false);
        displayBrick.ScaleBrick(0.2f);
        displayBrick.SetAbsolutePosition(displayPosition);
    }

    public void UpdateDisplay(Vector3 newPosition)
    {
        if (displayBrick == null)
            return;

        Vector3 pOld = displayBrick.cube.transform.position;
        Vector3 pLerp = Vector3.Lerp(pOld, newPosition, 0.1f);

        displayBrick.SetAbsolutePosition(pLerp);
    }

    public void SetSelectedIndex(int index)
    {
        if (index < items.Count)
        {
            if (index < 0)
                index = items.Count - 1;

            selectedIndex = index;
        } 
        else
        {
            index = 0;
        }

        UpdateItems();
    }

    public void AddItem(int x, int y, int z)
    {
        AddItem(new Vector3Int(x, y, z));
    }

    public void AddItem(Brick brick)
    {
        AddItem(brick.size);
    }

    public void AddItem(Vector3Int size)
    {
        // Find existing item in inventory
        for (int i = 0; i < items.Count; i++)
        {
            LegoItem iItem = items[i];

            if (size == iItem.size)
            {
                items[i].amount++;
                return;
            }
        }

        // Item doesn't exist yet, add it
        items.Add(new LegoItem(size));
    }

    public void PlaceItem(Vector3Brick position)
    {
        // Prevent placing a brick when the list is empty
        if (items.Count < 1)
            return;

        // Prevent the selectedIndex being higher then the list goes
        while (selectedIndex > items.Count - 1)
            selectedIndex--;

        // Get the target item and place the brick
        LegoItem item = items[selectedIndex];
        Brick newBrick = new Brick(tools, item.size, tools.CreateMaterial());

        newBrick.SetPosition(position.x, position.y + 1, position.z);
        tools.bricks.Remove(position);

        // Remove one of the bricks and if 0, remove the brick from the list
        items[selectedIndex].amount--;

        if (items[selectedIndex].amount <= 0)
            items.RemoveAt(selectedIndex);
    }
}