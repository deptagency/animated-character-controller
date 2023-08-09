using UnityEngine;

namespace DEPT.Unity
{
    public interface ILocomotionInput
    {
        public Vector2 RawDirectionXY { get; }
        public Vector3 DirectionXZ { get; }
        public float SpeedXZ { get; }
        public Quaternion RotationY { get; }
        public bool Jump { get; }
    }
}
