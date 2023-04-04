using System;
using UnityEngine;

namespace IdleRPG.Scripts.Runtime.Interfaces
{
    public interface IMovable
    {
        event Action<Vector3, Vector3> OnResetPositions;
        void MoveToPosition(Vector3 position);
        void LookAt(Vector3 target);
    }
}
