using UnityEngine;

public class Animal : MonoBehaviour 
{
    private GameObject animal;
    private Vector3 orientation = new Vector3(-90.0f, 0, 0);
    private int remainingMoves = 500;

    /// <summary>
    /// Use this contructor to spawn a new animal in the world. 
    /// Be sure to provide a copy of the animal and not the actual animal object.
    /// </summary>
    public Animal(GameObject _animalCopy, float x, float z)
    {
        this.animal = _animalCopy;
    }    

    private void RandomOrientation()
    {
        Vector3 animalRot = animal.transform.rotation.eulerAngles;
        orientation = animalRot + new Vector3(0, 0, Random.Range(-90.0f, 90.0f));
    }

    public void FixedUpdate() 
    {
        if (Random.Range(0, 255) == 0)
            RandomOrientation();
        if (Random.Range(0, 1024) == 0)
            remainingMoves = Random.Range(200, 500);
        
        Quaternion animalRot = animal.transform.rotation;
        animal.transform.rotation = Quaternion.Slerp(animalRot, Quaternion.Euler(orientation), 0.05f) * Quaternion.Euler(0, 0, 1);;

        if (remainingMoves > 0)
        {
            animal.transform.Translate(-animal.transform.forward * 0.01f);
            remainingMoves--;
        }
    }
}