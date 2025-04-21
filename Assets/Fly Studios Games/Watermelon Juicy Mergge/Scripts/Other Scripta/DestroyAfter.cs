using UnityEngine;

/* Cube Escape 3D : v1.0
 * By Fly Studios Assets
 * 
 * Support : flystudiosassets@gmail.com
 */

namespace CubeEscape3D
{
    public class DestroyAfter : MonoBehaviour
    {
        public float destroyTime; // Time in seconds after which the object will be destroyed

        void Start()
        {
            Destroy(gameObject, destroyTime); // Destroy the object after the specified time
        }
    }
}