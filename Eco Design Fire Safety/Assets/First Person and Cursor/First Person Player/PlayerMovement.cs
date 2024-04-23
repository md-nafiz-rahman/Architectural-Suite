using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{
    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController controller;
        public float speed = 5f;
        public float gravity = -100f;
        public float fallMultiplier = 5f;
        public float jumpHeight = 3f;
        public float jumpTimeout = 0.3f;
        public float fallTimeout = 0.15f;

        private Vector3 velocity;
        private bool isGrounded;

        void Update()
        {
            RaycastHit hit;
            float distanceToGround = 0.1f;
            isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 * transform.localScale.y + distanceToGround);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                velocity.y = Mathf.Sqrt(2f * -gravity * jumpHeight) * 1.5f;
            }
            Debug.Log($"Velocity Y: {velocity.y}, Is Grounded: {isGrounded}");
        }

        void FixedUpdate()
        {
            if (velocity.y < 0)
            {
                velocity.y += gravity * (fallMultiplier * 1.5f) * fallTimeout;
            }
            else if (velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                velocity.y += gravity * (fallMultiplier * 0.5f) * jumpTimeout;
            }
            else
            {
                velocity.y += gravity * 5f * Time.fixedDeltaTime;
            }
            controller.Move(velocity * Time.fixedDeltaTime);
        }
    }
}
