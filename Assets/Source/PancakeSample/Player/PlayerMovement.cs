using UnityEngine;

namespace PancakeSample.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Vector3 _velocity;
        
        [SerializeField]
        private CharacterController controller;

        [SerializeField]
        private float speed = 5.0f;

        [SerializeField]
        private float acceleration = 10.0f;

        [SerializeField]
        private float deceleration = 20.0f;
        
        [SerializeField]
        private PlayerBehaviour player;

        private void Update()
        {
            var moveVector = player.CanMove ? player.InputController.Move : Vector2.zero;
            var move = new Vector3(moveVector.x, 0.0f, moveVector.y);
            move = transform.TransformDirection(move);
            move = Vector3.ClampMagnitude(move, 1.0f);
            move *= speed;
            
            _velocity = move.magnitude > 0.0f ? 
                Vector3.Lerp(_velocity, move, acceleration * Time.deltaTime) : 
                Vector3.Lerp(_velocity, Vector3.zero, deceleration * Time.deltaTime);

            if (controller.isGrounded)
            {
                _velocity.y = Physics.gravity.y;
            }
            else
            {
                _velocity.y += Physics.gravity.y * Time.deltaTime;
            }

            controller.Move(_velocity * (speed * Time.deltaTime));
        }
    }
}