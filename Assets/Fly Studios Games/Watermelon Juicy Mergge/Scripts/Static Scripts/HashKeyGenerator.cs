using UnityEngine;

public static class HashKeyGenerator
{
    public static string GenerateUniqueHashKeyID(int min = 10, int max = 15)
    {
        int length = Random.Range(min, max);
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string unique_id = "";

        for (int i = 0; i < length; i++)
        {
            unique_id += chars[Random.Range(0, chars.Length - 1)];
        }

        Debug.LogWarning("Created unique_id : " + unique_id);
        return unique_id;
    }
}