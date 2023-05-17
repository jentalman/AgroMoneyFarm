using Infrastructure;
using Services.Input;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerMove : MonoBehaviour
    {
        private CharacterController _characterController;
        private float _movementSpeed = 4f;

        private IInputService _inputService;
        private Vector3 _movementVector;
 
        private void Awake()
        {
            _inputService = Game.InputService;

            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            _movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > 0)
            {
                _movementVector = Camera.main.transform.TransformDirection(_inputService.Axis);
                _movementVector.y = 0;
                _movementVector.Normalize();

                transform.forward = _movementVector;
            }

            _movementVector += Physics.gravity;
            
            _characterController.Move(_movementVector * (_movementSpeed * Time.deltaTime));
        }
        
    }
}