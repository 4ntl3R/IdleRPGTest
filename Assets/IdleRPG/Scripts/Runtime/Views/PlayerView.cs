using System;
using IdleRPG.Scripts.Runtime.Interfaces;
using UnityEngine;

namespace IdleRPG.Scripts.Runtime.Views
{
    public class PlayerView : MonoBehaviour, IHittable, ITargetProvider
    {
        public event Action<float> OnHitReceived;
        public event Action OnHealthReset;
        public Transform Target => transform;
        public void ReceiveHit(float hitPower)
        {
            Debug.Log("hit");
        }

        public IHittable GetTarget()
        {
            return this;
        }
    }
}
