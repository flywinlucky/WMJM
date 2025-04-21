using Sirenix.OdinInspector;
using UnityEngine;

public class GenerateID : MonoBehaviour
{
    [Header("Generated ID")]
    [Space]
    [Required]
    public string uniqueID;

    [ButtonGroup("First")]
    public string GenerateUniqueID()
    {
        uniqueID = HashKeyGenerator.GenerateUniqueHashKeyID();
        return uniqueID;
    }

    //[ButtonGroup("First")]
    public void RegenerateUniqueID()
    {
        uniqueID = HashKeyGenerator.GenerateUniqueHashKeyID();
    }
}