using System;
using System.Collections;
using UnityEngine;

namespace IdleRPG.Scripts.Runtime.Views
{
    [RequireComponent(typeof(LineRenderer))]
    public class LightingView : MonoBehaviour
    {
        [SerializeField] 
        private float lifetime = 0.5f;

        private WaitForSeconds _wait;
        private LineRenderer _lineRenderer;
        private IEnumerator _coroutine;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _wait = new WaitForSeconds(lifetime);
            gameObject.SetActive(false);
        }

        public void Show(Vector3[] positions)
        {
            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.SetPositions(positions);
            gameObject.SetActive(true);
            _coroutine = LifetimeProcess();
            StartCoroutine(_coroutine);
        }

        private IEnumerator LifetimeProcess()
        {
            yield return _wait;
            Hide();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
