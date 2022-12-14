using System.Collections.Generic;
using UnityEngine;

public class LegoItem
{
    public float[] dimentions = new float[3];
    public int amount = 1;

    public LegoItem(Brick brick)
    {
        dimentions[0] = brick.sizeX;
        dimentions[1] = brick.sizeZ;
        dimentions[2] = brick.height;
    }

    public LegoItem(float sizeX, float sizeZ, float height)
    {
        dimentions[0] = sizeX;
        dimentions[1] = sizeZ;
        dimentions[2] = height;
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
        displayBrick = new Brick(tools, item.dimentions[0], item.dimentions[1], tools.CreateMaterial(), item.dimentions[2], false, false);
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

    public void AddItem(Brick brick)
    {
        AddItem(brick.sizeX, brick.sizeZ, brick.height);
    }

    public void AddItem(float sizeX, float sizeZ, float height)
    {
        // Find existing item in inventory
        for (int i = 0; i < items.Count; i++)
        {
            LegoItem iItem = items[i];

            if (sizeX == iItem.dimentions[0] && sizeZ == iItem.dimentions[1] && height == iItem.dimentions[2])
            {
                items[i].amount++;
                return;
            }
        }

        // Item doesn't exist yet, add it
        LegoItem item = new LegoItem(sizeX, sizeZ, height);
        items.Add(item);
    }

    public void PlaceItem(Vector3 position)
    {
        // Prevent placing a brick when the list is empty
        if (items.Count < 1)
            return;

        // Prevent the selectedIndex being higher then the list goes
        while (selectedIndex > items.Count - 1)
            selectedIndex--;

        // Get the target item and place the brick
        LegoItem item = items[selectedIndex];
        Brick newBrick = new Brick(tools, item.dimentions[0], item.dimentions[1], new Material(Shader.Find("Standard")), item.dimentions[2]);

        newBrick.BuildBrick(tools.studs.Get(position));
        tools.studs.Remove(position);

        // Remove one of the bricks and if 0, remove the brick from the list
        items[selectedIndex].amount--;

        if (items[selectedIndex].amount <= 0)
            items.RemoveAt(selectedIndex);
    }
}