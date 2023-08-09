using UnityEngine;

namespace DEPT.Unity
{

    public class SphericalGravityField : MonoBehaviour, IGravityField
    {
        private SphereCollider _collider;

        public float Force = 9.81f;

        public void Awake()
        {
            _collider = GetComponent<SphereCollider>();
        }

        public Vector3 CalculateAcceleration(Rigidbody subject)
        {
            var sphericalGravity = ((transform.position + _collider.center) - subject.position).normalized * Force;

            return sphericalGravity;
        }
    }
}