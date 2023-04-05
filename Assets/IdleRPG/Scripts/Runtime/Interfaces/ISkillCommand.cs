using System;

namespace IdleRPG.Scripts.Runtime.Interfaces
{
    public interface ISkillCommand
    {
        event Action OnSuccess;
        void Execute();
        float CooldownTime { get; }
    }
}
