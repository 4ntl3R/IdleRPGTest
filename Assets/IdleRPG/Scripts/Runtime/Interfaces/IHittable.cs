using System;

namespace IdleRPG.Scripts.Runtime.Interfaces
{
    public interface IHittable
    {
        event Action<float> OnHitReceived;
        void ReceiveHit(float hitPower);
    }
}
