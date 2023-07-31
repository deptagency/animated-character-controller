using UnityEngine;

namespace DEPT.Unity
{

    public class CharacterController : MonoBehaviour
    {
        public LayerMask layerMask;
        public bool _isGrounded;
        public Vector3 _surfaceNormal;
        public float Rebound = 0.01f;
        public float SpeedMultiplier = 10f;
        public float MaxMagnitudeXZ = 1f;
        public float RotationSpring = 1f;
        public float RotationDamper = 1f;
        public float Tilt = 1f;

        public bool EnableDebugging = false;

        public Rigidbody rb { get; private set; }
        public CapsuleCollider capsule { get; private set; }
        public Input input { get; private set; }
        public SceneBehavior sceneBehavior { get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            capsule = GetComponent<CapsuleCollider>();
            input = GetComponent<Input>();
            sceneBehavior = GetComponent<SceneBehavior>();
        }

        private void FixedUpdate()
        {
            var targetMovement = input.MovementXZ;

            var ray = new Ray(rb.position + capsule.center, -capsule.transform.up);

            if(Physics.Raycast(ray, out var hitInfo, (capsule.height * 0.5f) + 0.51f, layerMask))
            {
                _isGrounded = true;
                _surfaceNormal = hitInfo.normal;
            }
            else
            {
                _isGrounded = false;
                _surfaceNormal = -sceneBehavior.GravityDirection;
            }

            var rotation = Quaternion.LookRotation(input.MovementXZ != Vector3.zero ? input.MovementXZ : rb.transform.forward, _surfaceNormal);

            rb.MoveRotation(rotation);

            var intendedTranslation = (input.SpeedXZ * SpeedMultiplier * Time.deltaTime * targetMovement);
            var actualTranslation = HandleCollisions(intendedTranslation);

            // Now handle gravity, snapping etc
            if(!_isGrounded)
            {
                // test for and apply gravity
                actualTranslation += new Vector3(0f, -0.01f, 0f);
            }
            else
            {
                // align translation to 
            }

            rb.MovePosition(rb.position + actualTranslation);
        }

        public Vector3 HandleCollisions(Vector3 translation)
        {
            var referencePosition = rb.position;

            return HandleTranslationCollisions(translation, referencePosition, capsule, Vector3.zero);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="translation">Intended translation subject to collision-based adjustments</param>
        /// <param name="referencePosition">Starting position (e.g. rb.position)</param>
        /// <returns></returns>
        private Vector3 HandleTranslationCollisions(Vector3 translation, Vector3 referencePosition, CapsuleCollider collider, Vector3 cumulativeOffset)
        {
            var magnitude = translation.magnitude;

            collider.ToWorldSpaceCapsule(out var capsuleTop, out var capsuleBottom, out var capsuleRadius);

            //if (EnableDebugging && magnitude > 0f)
            //{
            //    Debug.Log("Collision Data");
            //    Debug.Log(translation);
            //    Debug.Log(referencePosition);
            //    Debug.Log(collider);
            //    Debug.Log(cumulativeOffset);
            //}

            if (Physics.CapsuleCast(capsuleTop + cumulativeOffset, capsuleBottom + cumulativeOffset, capsuleRadius, translation.normalized, out RaycastHit hitInfo, magnitude, layerMask))
            {
                if (hitInfo.distance < 0.01f)
                {
                    return Vector3.zero;
                }

                var hitTranslation = translation.normalized * hitInfo.distance;

                if(hitTranslation.magnitude > Rebound)
                {
                    var reboundTranslation = hitTranslation + Vector3.Reflect(translation, hitInfo.normal).normalized * (magnitude - hitInfo.distance);

                    if (EnableDebugging)
                    {
                        Debug.Log($"Collision from [{referencePosition}] in direction [{translation}] hit at {hitTranslation}");
                        Debug.Log($"Reflected normal to test from [{reboundTranslation}]");
                    }

                    return reboundTranslation;
                }

                return hitTranslation;
            }

            return translation;
        }
    }
}
