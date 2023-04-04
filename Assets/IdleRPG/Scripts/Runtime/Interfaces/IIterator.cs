using System;

namespace IdleRPG.Scripts.Runtime.Interfaces
{
    public interface IIterator
    {
        event Action<float> OnIterate;
        void StopIterating();
        void StartIterating(float? interval = null);
    }
}
