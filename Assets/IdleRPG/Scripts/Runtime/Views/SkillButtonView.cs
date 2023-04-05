using System;
using System.Collections;
using IdleRPG.Scripts.Data;
using IdleRPG.Scripts.Runtime.Commands;
using IdleRPG.Scripts.Runtime.Controllers;
using IdleRPG.Scripts.Runtime.Interfaces;
using IdleRPG.Scripts.Runtime.Spawners;
using UnityEngine;
using UnityEngine.UI;

namespace IdleRPG.Scripts.Runtime.Views
{
    public class SkillButtonView : MonoBehaviour, IButtonView
    {
        public event Action OnButtonClicked;

        private IEnumerator _coolDownCoroutine;

        [SerializeField] 
        private Button button;

        [SerializeField] 
        private SkillsDataBundle dataBundle;

        [SerializeField] 
        private PlayerView player;

        [SerializeField] 
        private EnemySpawner spawner;

        [SerializeField] 
        private LightingView lightingView;
        
        private Image _buttonImage;

        private SkillController _controller;
        private ISkillCommand _skillCommand;

        private bool IsCooldownActive => !(_coolDownCoroutine is null);

        private void Awake()
        {
            _buttonImage = button.GetComponent<Image>();
            button.onClick.AddListener(InvokeButtonClick);
            
            ManageDependencies();
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(InvokeButtonClick);
            if (IsCooldownActive)
            {
                StopCoroutine(_coolDownCoroutine);
            }
            
            _controller.Dispose();
        }

        public void TurnDown(float coolDown)
        {
            ToggleButton(false);
            _coolDownCoroutine = CoolDownProcess(coolDown);
            StartCoroutine(_coolDownCoroutine);
        }

        private void InvokeButtonClick()
        {
            OnButtonClicked?.Invoke();
        }

        private IEnumerator CoolDownProcess(float time)
        {
            var timePassed = 0f;
            while (timePassed < time)
            {
                yield return null;
                timePassed += Time.deltaTime;
                UpdateState(timePassed/time);
            }

            ToggleButton(true);
        }

        private void UpdateState(float filledAmount)
        {
            _buttonImage.fillAmount = filledAmount;
        }

        private void ToggleButton(bool isActivated)
        {
            button.interactable = isActivated;
            _buttonImage.fillAmount = isActivated ? 1 : 0;
        }

        private void ManageDependencies()
        {
            _skillCommand = new LightingSkillCommand(spawner, player, dataBundle.LightingData, lightingView);
            _controller = new SkillController(this, _skillCommand);
        }
    }
}
