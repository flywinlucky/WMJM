using Sirenix.OdinInspector;
using UnityEngine;

namespace WatermelonGameClone
{
    [RequireComponent(typeof(GenerateID))]
    public class Sphere : MonoBehaviour
    {
        [Header("Sphere Information")]

        public bool isSmallItem;
        public bool isBigItem;
        [Space]
        [Required]
        public string itemName;

        public int SphereNo;

        public SpriteRenderer itemSpriteRenderer;

        [HideInInspector]
        public string itemID;

        [HideInInspector]
        public Vector3 currentItemPosition;
      
        [HideInInspector]
        public Quaternion currentItemRotation;

        [HideInInspector]
        public GameObject thisObjectPrefab;

        private Rigidbody2D _rb;
        public bool IsMerge;
        public bool isFirstCollision;

        public AudioSource collisionSfx;

        public Animator animator;
        public Color sphereColor;

        public string ItemID()
        {
            return itemID;
        }

        public Color GetSphereColor()
        {
            return sphereColor;
        }

        public void CollectPositionAndRotation()
        {
            currentItemPosition = CurrentItemPosition();
            currentItemRotation = CurrentItemRotation();
        }

        public Vector3 CurrentItemPosition()
        {
            return transform.localPosition;
        }

        public Quaternion CurrentItemRotation()
        {
            // Utilizăm transform.localRotation pentru a obține rotirea locală a obiectului
            
            return transform.localRotation;
        }

        private void OnEnable()
        {
            animator.Play("Base Layer.Open", 0, 0.25f);

            //itemName = gameObject.name;

            thisObjectPrefab = gameObject;
                 
            itemID = GetComponent<GenerateID>().uniqueID;

            _rb = GetComponent<Rigidbody2D>();
            _rb.simulated = false;
        }

        public void SphereRigidBodyGravity(bool state)
        {
            _rb.simulated = state;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!isFirstCollision)
            {
                //Debug.Log(itemName + " | First Collision !");
                animator.Play("Base Layer.Open", 0, 0.25f);

                collisionSfx.Play();

                isFirstCollision = true;
            }

            if (IsEligibleForMerge(collision, out Sphere colSphere))
                PerformMerge(colSphere);
        }

        private bool IsEligibleForMerge(Collision2D collision, out Sphere colSphere)
        {
            colSphere = null;
            GameObject colObj = collision.gameObject;
            if (!colObj.CompareTag("Sphere"))
                return false;

            colSphere = colObj.GetComponent<Sphere>();
            return SphereNo == colSphere.SphereNo &&
                !IsMerge &&
                !colSphere.IsMerge;
        }

        private void PerformMerge(Sphere colSphere)
        {
            GameManager.Instance.GameEvent.Execute(GameManager.GameState.Merging);

            IsMerge = true;
            colSphere.IsMerge = true;

            if (SphereNo < GameManager.Instance.MaxSphereNo - 1)
            {
                GameManager.Instance.MergeNext(transform.position, SphereNo);
            }

            GameManager.Instance.GameEvent.Execute(GameManager.GameState.SphereMoving);

            Destroy(gameObject);
            Destroy(colSphere.gameObject);
        }

        [Button]
        public void DestroyItem()
        {
            Destroy(gameObject);
            GameManager.Instance.GameEvent.Execute(GameManager.GameState.Merging);
            GameManager.Instance.GameEvent.Execute(GameManager.GameState.SphereMoving);
        }
    }
}