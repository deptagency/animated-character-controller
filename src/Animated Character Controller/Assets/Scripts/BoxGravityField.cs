using UnityEngine;

namespace DEPT.Unity
{
    public class BoxGravityField : MonoBehaviour, IGravityField
    {
        private BoxCollider _collider;

        public float Force = 9.81f;

        public void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        public Vector3 CalculateAcceleration(Rigidbody subject)
        {
            return -transform.up.normalized * Force;
        }
    }
}