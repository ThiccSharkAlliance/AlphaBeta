using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MenuCamera : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _circleSpeed;
        [SerializeField] private float _circleSize;
        [SerializeField] private GameObject _parent;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Color _backgroundColor;
        [SerializeField] private Color _backgroundFade;
        [SerializeField] private float _fadeSpeed;

        private float _t;

        private void Awake()
        {
            StartCoroutine(FadeBackground());
        }

        // Update is called once per frame
        void Update()
        {
            var direction = Vector3.zero;

            var xPos = Mathf.Sin(Time.time * _circleSpeed) * _circleSize;
            var zPos = Mathf.Cos(Time.time * _circleSpeed) * _circleSize;

            direction.x += xPos;
            direction.z += zPos;
            
            Debug.DrawRay(transform.position, direction);
            
            transform.position += direction * (Time.deltaTime * _movementSpeed);
            _parent.transform.forward = direction;
        }

        private IEnumerator FadeBackground()
        {
            yield return new WaitForSeconds(1);
            
            while (_t < 1)
            {
                _t += Time.deltaTime * _fadeSpeed;
                _backgroundImage.color = Color.Lerp(_backgroundColor, _backgroundFade, _t);
                yield return null;
            }
        }
    }
}
