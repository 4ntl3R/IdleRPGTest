using System;
using System.Collections;
using IdleRPG.Scripts.Runtime.Interfaces;
using UnityEngine;

namespace IdleRPG.Scripts.Runtime.Managers
{
    public class SingleCoroutineManager: IIterator
    {
        private readonly MonoBehaviour _target;
        private bool _isIterating = false;
        private IEnumerator _currentRoutine;

        public SingleCoroutineManager(MonoBehaviour target)
        {
            _target = target;
        }
        
        public event Action<float> OnIterate;

        public void StartIterating(float? interval = null)
        {
            if (_isIterating)
            {
                throw new Exception("Cant launch another coroutine");
            }
            
            _isIterating = true;
            _currentRoutine = interval.HasValue ? IterationProcess(interval.Value) : IterationProcess();
            _target.StartCoroutine(_currentRoutine);
        }
        
        public void StopIterating()
        {
            _isIterating = false;
            _target.StopCoroutine(_currentRoutine);
        }

        private IEnumerator IterationProcess(float interval)
        {
            var wait = new WaitForSeconds(interval);
            while (_isIterating)
            {
                yield return wait;
                OnIterate?.Invoke(interval);
            }
        }
        
        private IEnumerator IterationProcess()
        {
            while (_isIterating)
            {
                yield return null;
                OnIterate?.Invoke(Time.deltaTime);
            }
        }
    }
}
