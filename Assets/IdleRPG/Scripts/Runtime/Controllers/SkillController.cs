using System;
using IdleRPG.Scripts.Runtime.Interfaces;

namespace IdleRPG.Scripts.Runtime.Controllers
{
    public class SkillController: IDisposable
    {
        private readonly IButtonView _buttonView;
        private readonly ISkillCommand _skillCommand;

        public SkillController(IButtonView buttonView, ISkillCommand skillCommand)
        {
            _buttonView = buttonView;
            _skillCommand = skillCommand;
            
            SubscribeEvents();
        }
        
        public void Dispose()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _skillCommand.OnSuccess += SkillUseHandle;
            _buttonView.OnButtonClicked += _skillCommand.Execute;
        }
        
        private void UnsubscribeEvents()
        {
            _skillCommand.OnSuccess -= SkillUseHandle;
            _buttonView.OnButtonClicked -= _skillCommand.Execute;
        }

        private void SkillUseHandle()
        {
            _buttonView.TurnDown(_skillCommand.CooldownTime);
        }
    }
}
