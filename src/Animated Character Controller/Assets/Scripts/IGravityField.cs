using UnityEngine;

namespace DEPT.Unity
{
    public interface IGravityField
    {
        public Vector3 CalculateAcceleration(Rigidbody subject);
    }
}