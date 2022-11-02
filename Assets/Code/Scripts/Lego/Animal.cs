using UnityEngine;

public class Animal 
{
    private GameObject animal;
    private Vector3 orientation = new Vector3(0, 0, 0);
    private bool flyingAnimal;
    private int remainingMoves = 500;

    /// <summary>
    /// Use this contructor to spawn a new animal in the world. 
    /// Be sure to provide a copy of the animal and not the actual animal object.
    /// </summary>
    public Animal(GameObject _animalCopy, float x, float z, bool _flyingAnimal = false)
    {
        this.animal = _animalCopy;
        this.flyingAnimal = _flyingAnimal;
    }    

    private void RandomOrientation()
    {
        Vector3 animalRot = animal.transform.rotation.eulerAngles;
        orientation = new Vector3(0, animalRot.y + Random.Range(-90.0f, 90.0f), 0);
    }

    public void FixedUpdate() 
    {
        if (Random.Range(0, 255) == 0)
            RandomOrientation();
            
        if (Random.Range(0, 1024) == 0)
            remainingMoves = Random.Range(200, 500);
        
        Vector3 animalRot = animal.transform.rotation.eulerAngles;
        Vector3 a = new Vector3(0, animalRot.y, 0);
        Vector3 b = orientation;
        animal.transform.rotation = Quaternion.Euler(Vector3.Lerp(a, b, 0.05f));

        if (remainingMoves > 0)
        {
            animal.transform.Translate(animal.transform.forward * 0.01f);

            // Make sure flying animals are always moving
            if (!flyingAnimal)
                remainingMoves--;
        }
    }
}