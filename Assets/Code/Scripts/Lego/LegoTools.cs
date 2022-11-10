using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegoTools
{
    public float worldScale;
    public GameObject stud;
    public StudDictionary studs;

    public List<Brick> droppedBricksPool = new List<Brick>();

    public LegoTools(GameObject _stud, float _worldScale)
    {
        this.stud = _stud;
        this.worldScale = _worldScale;
        this.studs = new StudDictionary();
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
    /// This method will round a Vector3 position to a Lego Plate position.
    /// <example>
    /// For example:
    /// <code>
    /// Vector3 p = new Vector3(0.36f, 1.63f, 1.94f);
    /// p = RoundToPlate(p.x, p.y, p.z);
    /// </code>
    /// results in <c>p</c> having the value (0.5f, 1.6f, 2.0f).
    /// </example>
    /// </summary>
    public Vector3 RoundToPlate(float x, float y, float z)
    {
        return new Vector3(Mathf.Round(x * 2) / 2, Round(y, 0.2f), Mathf.Round(z * 2) / 2);
    } 

    public Vector3 RoundToPlate(Vector3 vec)
    {
        return RoundToPlate(vec.x, vec.y, vec.z);
    } 

    public Vector3 FloorToPlate(float x, float y, float z)
    {
        return new Vector3(Mathf.Floor(x * 2) / 2, Mathf.Floor(y * 5) / 5, Mathf.Floor(z * 2) / 2);
    } 

    public Vector3 FloorToPlate(Vector3 vec)
    {
        return FloorToPlate(vec.x, vec.y, vec.z);
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
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        material.SetFloat("_Mode", 3.0f);
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
}