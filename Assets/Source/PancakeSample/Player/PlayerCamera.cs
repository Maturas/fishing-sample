using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace PancakeSample.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        private readonly List<Vector2> _deltaBuffer = new();
        private Vector2 _lastDelta = Vector3.zero;
        private float _currentCameraOffset;
        private Transform _originalParent;
        private Transform _cameraHandle;

        [SerializeField]
        private Transform cameraTransform;
        
        [SerializeField]
        private bool cameraSmoothing = true;
        
        [SerializeField]
        private float cameraSmoothingSpeed = 10.0f;

        [SerializeField]
        private float cameraOffsetSmoothing = 10.0f;

        [SerializeField]
        private float cameraDefaultYOffset = 1.75f;

        [SerializeField]
        private float cameraSensitivity = 3.0f;
        
        [SerializeField]
        private PlayerBehaviour player;
        
        public Transform CameraTransform => cameraTransform;
        public Vector2 CurrentRotation { get; set; }

        private void Awake()
        {
            _originalParent = cameraTransform.parent;
            Assert.IsNotNull(cameraTransform, "Missing camera transform reference!");
        }

        private void OnEnable()
        {
            if (cameraSmoothing)
            {
                var placeholder = new GameObject("CameraHandle");
                _cameraHandle = placeholder.transform;
                _cameraHandle.SetParent(_originalParent);
                _cameraHandle.position = cameraTransform.position;
                _cameraHandle.rotation = cameraTransform.rotation;
                cameraTransform.SetParent(null);
            }
        }
        
        private void Update()
        {
            CurrentRotation += GetCursorDelta();
            
            var currentRotation = CurrentRotation;
            currentRotation.y = Mathf.Clamp(CurrentRotation.y, -85.0f, 85.0f);
            CurrentRotation = currentRotation;
            
            _currentCameraOffset = Mathf.Lerp(_currentCameraOffset, cameraDefaultYOffset, Time.deltaTime * cameraOffsetSmoothing);
            
            if (cameraSmoothing)
            {
                _cameraHandle.localPosition = new Vector3(0.0f, _currentCameraOffset, 0.0f);
                
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, _cameraHandle.position, cameraSmoothingSpeed * Time.deltaTime);
                _cameraHandle.localEulerAngles = new Vector3(CurrentRotation.y, 0.0f, 0.0f);
                
                cameraTransform.rotation = _cameraHandle.rotation;
            }
            else
            {
                cameraTransform.localPosition = new Vector3(0.0f, _currentCameraOffset, 0.0f);
                cameraTransform.localEulerAngles = new Vector3(CurrentRotation.y, 0.0f, 0.0f);
            }

            transform.rotation = Quaternion.Euler(0.0f, CurrentRotation.x, 0.0f);
        }

        private Vector2 GetCursorDelta()
        {
            var inputX = player.InputController.Look.x;
            var inputY = player.InputController.Look.y;
            return new Vector2(inputX, -inputY) * cameraSensitivity;
        }
    }
}