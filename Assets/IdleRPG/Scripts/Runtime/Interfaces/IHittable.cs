using System;
using UnityEngine;

namespace IdleRPG.Scripts.Runtime.Interfaces
{
    public interface IHittable
    {
        event Action<float> OnHitReceived;
        Transform Target { get; }
        void ReceiveHit(float hitPower);
    }
}
