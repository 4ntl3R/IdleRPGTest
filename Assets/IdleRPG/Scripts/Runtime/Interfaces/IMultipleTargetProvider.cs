using System.Collections.Generic;
using UnityEngine;

namespace IdleRPG.Scripts.Runtime.Interfaces
{
    public interface IMultipleTargetProvider : ITargetProvider
    {
        IHittable GetClosestToPosition(Vector3 position, float rangeLimit = float.MaxValue, List<IHittable> exceptions = null);
    }
}
