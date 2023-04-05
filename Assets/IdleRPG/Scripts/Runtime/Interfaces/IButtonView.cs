using System;

namespace IdleRPG.Scripts.Runtime.Interfaces
{
    public interface IButtonView
    {
        event Action OnButtonClicked;
        void TurnDown(float coolDown);
    }
}
