using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LegoGenerator : MonoBehaviour
{
    // VR Tools
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    [SerializeField] Text debugItemDisplay;

    [SerializeField] Vector2Int Dimentions = new Vector2Int(25, 25);
    [SerializeField] GameObject Stud;
    [SerializeField] List<GameObject> GrassBricks;
    [SerializeField] List<GameObject> StoneBricks;
    [SerializeField] GameObject Cactus;
    [SerializeField] GameObject Tree;
    [SerializeField] GameObject Pig;
    [SerializeField] GameObject Pidgeon;
    [SerializeField] Material Transparent;
    [SerializeField] int seed = -1;
    [SerializeField] float worldScale = 0.5f;
    [SerializeField] int elevatorSize = 10;

    private List<Brick> cloudPool = new List<Brick>();
    private List<Animal> animals = new List<Animal>();
    private LegoTools legoTools;
    private LegoInteraction objInteraction;
    private LegoVRTools vrTools;
    private LegoInventory inventory;

    private Material Grey, BrightGreen, Sand, tWhite, Brown, tGrey;
    private int previousSeed, cloudSeed;
    private int ePosX, ePosZ;

    private void Start() 
    {
        legoTools = new LegoTools(Stud, worldScale);
        vrTools = new LegoVRTools();
        inventory = new LegoInventory(legoTools);
        objInteraction = new LegoInteraction(legoTools);

        // Create the Materials and add the colors and instancing
        CreateMaterials();

        // Generate a random seed and supply it for pseudo-randomness, if the seed is not -1, use the provided seed.
        if (seed == -1)
            seed = Random.Range(0, (int)Mathf.Pow(9, 8));

        Random.InitState(seed);
        cloudSeed = seed;
        previousSeed = seed;

        // Generate the map and the clouds
        GenerateMap();
        GenerateClouds();

        // Add some starter bricks
        for (int i = 0; i < 5; i++)
            inventory.AddItem(1, 1, 0.6f);

        for (int i = 0; i < 10; i++)
            inventory.AddItem(1, 1, 0.2f);
    }

    private void CreateMaterials()
    {
        Grey = legoTools.CreateMaterial();
        BrightGreen = legoTools.CreateMaterial();
        Sand = legoTools.CreateMaterial();
        Brown = legoTools.CreateMaterial();
        tWhite = Object.Instantiate(Transparent);
        tGrey = Object.Instantiate(Transparent);
        Grey.color = new Color(0.63f, 0.63f, 0.63f);
        BrightGreen.color = new Color(0, 0.57f, 0.28f);
        Sand.color = new Color(0.90f, 0.87f, 0.76f);
        Brown.color = new Color(0.42f, 0.25f, 0.13f);
        tWhite.color = new Color(0.95f, 0.95f, 0.95f, 0.5f);
        tGrey.color = new Color(0.63f, 0.63f, 0.63f, 0.5f);
    }

    private void FixedUpdate()
    {
        cloudSeed++;
        if (Time.frameCount % 20 == 0) 
            GenerateClouds();

        foreach(Animal a in animals)
            a.FixedUpdate();

        // Update item ui
        inventory.UpdateDisplay(leftHand.position + new Vector3(0, 0.1f, 0));
    }

    private void Update()
    {
        // Update controllers and get wether the grip buttons are pressed
        vrTools.UpdateControllers();
        bool[] gripStates = vrTools.GetButtonStates(UnityEngine.XR.CommonUsages.gripButton, false);

        if (gripStates[1])
        {
            // Get controller position and round it to a stud position
            Vector3 controllerPos = gripStates[1] ? rightHand.position : leftHand.position;
            Vector3 legoPos = legoTools.RoundToPlate(controllerPos / worldScale);
            legoPos.x += 0.25f;
            legoPos.z += 0.25f;

            // Check if a stud exists at this position
            for (float i = -2; i <= 1; i++)
            {
                legoPos.y += (i / 5);

                if (legoTools.studs.Has(legoPos))
                {
                    inventory.PlaceItem(legoPos);
                    break;
                }
            }
        }

        // Suck up dropped lego bricks
        if (gripStates[0])
        {
            foreach (Brick droppedBrick in legoTools.droppedBricksPool)
            {
                // Make sure brick is active in the world
                if (!droppedBrick.cube.activeInHierarchy)
                    continue;

                // Check if the brick is close enough to be sucked up
                Transform tCube = droppedBrick.cube.transform;
                float distance = Vector3.Distance(leftHand.position, tCube.position);
                if (distance < 2.0f)
                {
                    Vector3 lerp = Vector3.Lerp(tCube.position, leftHand.position, 10f * Time.deltaTime);
                    droppedBrick.SetAbsolutePosition(lerp);

                    // Check if the brick is so close it can be added to the player's inventory
                    if (distance < 0.35f)
                    {
                        inventory.AddItem(droppedBrick);
                        droppedBrick.SetActive(false);
                    }
                }
            }
        }

        // Check wether to switch to the next item
        bool[] buttonStates = vrTools.GetButtonStates(UnityEngine.XR.CommonUsages.primaryButton);

        if (buttonStates[0])
            inventory.SetSelectedIndex(inventory.selectedIndex - 1);
        else if (buttonStates[1])
            inventory.SetSelectedIndex(inventory.selectedIndex + 1);

        // Update controller positions
        objInteraction.UpdateController(leftHand.position);

        // Check if the map should be regenerated. Seed changed.
        if (seed != previousSeed)
        {
            legoTools.studs = new StudDictionary();
            foreach (Transform child in transform)
                GameObject.Destroy(child.gameObject);

            GenerateMap();
        }

        previousSeed = seed;
    }

    private void GenerateClouds()
    {
        float cloudMultiplier = cloudSeed * 0.0003f;
        int poolIndex = 0;

        // Used to move the clouds along the map using Perlin Noise
        for (int x = 0; x < Dimentions.x; x++)
            for (int z = 0; z < Dimentions.y; z++)
            {
                float y = Mathf.PerlinNoise(x * 0.1f + cloudMultiplier, z * 0.1f + cloudMultiplier) * 10.0f;

                if (y > 5.75f)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        // Get the final Y coord for the original block and the mirrored block
                        float yf = i == 0 ? y : -y + 11.5f;
                        yf += 7.5f / worldScale;

                        // Check if there already is a cloud block in the object pool
                        if (poolIndex < cloudPool.Count)
                        {
                            // Move the exising cloud block to the new position
                            Brick cloudBlock = cloudPool[poolIndex];
                            cloudBlock.SetPosition(legoTools.RoundToPlate(x, yf, z));
                            cloudBlock.SetActive(true);
                            poolIndex++;
                        }
                        else
                        {
                            // Create a new cloud block, set the position and move it into the cloud pool
                            Brick cloudBlock = new Brick(legoTools, 2, 2, tWhite, 1.8f);
                            cloudBlock.SetPosition(legoTools.RoundToPlate(x, yf, z));
                            cloudPool.Add(cloudBlock);
                        }
                    }
                }
            }

        // Make the unused cloud blocks inactive
        if (cloudPool.Count > poolIndex)
            for (int i = poolIndex; i < cloudPool.Count; i++)
            {
                cloudPool[i].SetActive(false);
            }
    } 

    void GenerateMap()
    {
        float multiplier = seed * 0.01f;
        float[,] map = new float[Dimentions.x * 2, Dimentions.y * 2];

        for (int x = 0; x < Dimentions.x * 2; x += 2)
            for (int z = 0; z < Dimentions.y * 2; z += 2)
            {
                // Generate base terrain and hills.
                float y = Mathf.PerlinNoise(x * 0.01f + multiplier, z * 0.01f + multiplier) * 15.0f;
                y *= Mathf.PerlinNoise(x * 0.025f + multiplier, z * 0.025f + multiplier);
                y *= Mathf.PerlinNoise(x * 0.05f + multiplier, z * 0.05f + multiplier);

                // Increase the intensity of the terrain and lift it up a bit
                y *= 1.1f; y += 0.5f;

                // Dig small holes
                float lakeHeight = Mathf.PerlinNoise(x * 0.05f + multiplier, z * 0.05f + multiplier) * 15.0f;
                if (lakeHeight < 10.0f)
                    lakeHeight = 10.0f;
                
                y -= (lakeHeight - 10.0f);

                // Round down the Y to plates
                y = legoTools.Round(y, 0.4f);
                map[x, z] = y;
            }

        // Make a platform for the broken elevator
        ePosX = Random.Range(2, Dimentions.x / 2 - (2 + elevatorSize / 2)) * 2;
        ePosZ = Random.Range(2, Dimentions.y / 2 - (2 + elevatorSize / 2)) * 2;

        float lowestPoint = Mathf.Min(map[ePosX, ePosZ], map[ePosX + elevatorSize, ePosZ + elevatorSize]);
        lowestPoint = Mathf.Min(lowestPoint, map[ePosX + elevatorSize, ePosZ]);
        lowestPoint = Mathf.Min(lowestPoint, map[ePosX, ePosZ + elevatorSize]);
        lowestPoint += 0.8f;

        for (int x = ePosX; x < ePosX + elevatorSize; x++)
            for (int z = ePosZ; z < ePosZ + elevatorSize; z++)
                map[x, z] = lowestPoint;

        GenerateElevator(map[ePosX, ePosZ] + 0.6f);

        for (int x = 0; x < Dimentions.x * 2; x += 2)
            for (int z = 0; z < Dimentions.y * 2; z += 2)
            {
                // Get y from previous loop
                float y = map[x, z];

                if (x >= ePosX && x < ePosX + elevatorSize && z >= ePosZ && z < ePosZ + elevatorSize)
                {
                    Brick newBrick = new Brick(legoTools, 2, 2, Grey, 2.4f, false);
                    newBrick.SetPosition(x, y, z);
                    newBrick.SetParent(this.transform);
                }
                else
                {
                    Brick newBrick = new Brick(legoTools, 2, 2, y > 1.2f ? BrightGreen : Sand, 2.4f);
                    newBrick.SetPosition(x, y, z);
                    newBrick.SetParent(this.transform);

                    if (y <= 1.2f)
                    {
                        DistributeStone(x, y, z, Grey);
                        DistributeCactus(x, y, z);
                    }
                    else
                    {
                        DistributeGrass(x, y, z);
                        DistributeTrees(x, y, z);
                    }
                }
            }
    }

    private void GenerateElevator(float y)
    {
        for (int x = ePosX; x < ePosX + elevatorSize; x++)
            for (int z = ePosZ; z < ePosZ + elevatorSize; z++)
                for (float fy = y; fy < y + 5 * 1.2f; fy++)
                {
                    if (x != ePosX && x != ePosX + elevatorSize - 1 && z != ePosZ && z != ePosZ + elevatorSize - 1)
                        continue;

                    Blueprint bp = new Blueprint(legoTools, 1, 1, tGrey);
                    bp.SetParent(transform);
                    bp.SetPosition(x, fy + 1.2f, z);
                }
    }

    private void DistributeGrass(float x, float y, float z)
    {
        // Generate 2 random numbers to check wether grass should be placed, and where to position tiles.
        int rand = Random.Range(0, 12);    // Determine the X position and wether to place
        int rand2 = Random.Range(0, 12);   // Determine the Z position

        if (rand == 0 || rand == 6)
        {
            y += 1.2f;
            x += rand == 0 ? 0f : 1f;
            z += rand2 < 6 ? 0f : 1f;

            GameObject tile = legoTools.RandomListItem(GrassBricks);
            GameObject newGrass = legoTools.Clone(tile, new Vector3(x, y, z));
            newGrass.transform.rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90.0f, 0);

            foreach (Renderer rend in newGrass.GetComponentsInChildren<Renderer>())
                rend.material = BrightGreen;

            newGrass.transform.parent = this.transform;
        }
    }

    private void DistributeStone(float x, float y, float z, Material m)
    {
        // Generate 3 random numbers to check wether stone should be placed, and where to position tiles.
        int rand = Random.Range(0, 24);    // Determine the X position and wether to place
        int rand2 = Random.Range(0, 12);   // Determine the Z position

        if (rand == 0)
        {
            y += 1.2f;
            x += rand < 12 ? 0f : 1f;
            z += rand2 < 6 ? 0f : 1f;

            GameObject tile = legoTools.RandomListItem(StoneBricks);
            GameObject newStone = legoTools.Clone(tile, new Vector3(x, y, z));
            newStone.transform.rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);

            foreach (Renderer rend in newStone.GetComponentsInChildren<Renderer>())
                rend.material = m;

            newStone.transform.parent = this.transform;
        }
    }

    private void DistributeCactus(float x, float y, float z)
    {
        // Generate 2 random numbers to check wether cactus should be placed, and where to position tiles.
        int rand = Random.Range(0, 190);    // Determine the X position and wether to place
        int rand2 = Random.Range(0, 12);    // Determine the Z position and the rotation

        float rotation = rand2 * 90.0f % 360;

        if (rand == 0)
        {
            y += 1.2f;
            x += rand < 95 ? 0f : 1f;
            z += rand2 < 6 ? 0f : 1f;

            GameObject newCactus = legoTools.Clone(Cactus, new Vector3(x, y, z));
            newCactus.AddComponent<BoxCollider>();
            newCactus.transform.rotation = Quaternion.Euler(0, rotation, 0);

            foreach (Renderer rend in newCactus.GetComponentsInChildren<Renderer>())
                rend.material = BrightGreen;

            newCactus.transform.parent = this.transform;
            objInteraction.AddObject(newCactus);
        }
    }

    private void DistributeTrees(float x, float y, float z)
    {
        int rand = Random.Range(0, 128);

        if (rand == 0)
        {
            y += 1.2f;
            x += 0.5f;
            z += 0.5f;

            GameObject newTree = legoTools.Clone(Tree, new Vector3(x, y, z));
            newTree.AddComponent<BoxCollider>();

            foreach (Renderer rend in newTree.GetComponentsInChildren<Renderer>())
                rend.material = rend.name == "Leaves" ? BrightGreen : Brown;

            newTree.transform.parent = this.transform;
            objInteraction.AddObject(newTree);
        }
    }
}