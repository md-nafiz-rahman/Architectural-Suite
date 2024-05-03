
// PlayerMovement.cs provides the functionality for player movement, gravity application, and jumping within a 3D environment.
// This script is part of the "Apartment Kit" asset from the Unity Asset Store and has been adapted to include player jump functionality.
// Reference: Unity Asset Store, "Apartment Kit." https://assetstore.unity.com/packages/3d/environments/apartment-kit-124055 (accessed Jan. 2, 2024).

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
        private Vector3 velocity;
        private bool isGrounded;

        // Additional variables to handle jumping 
        public float fallMultiplier = 5f;
        public float jumpHeight = 3f;
        public float jumpTimeout = 0.3f;
        public float fallTimeout = 0.15f;



        void Update()
        {
            // Cast a ray downwards to check if the player is touching the ground
            RaycastHit hit;
            float distanceToGround = 0.1f;
            isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 * transform.localScale.y + distanceToGround);

            // Reset the vertical velocity if the player is on the ground to prevent accumulating negative velocity
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            // Check if the space key is pressed and the player is on the ground to initiate a jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                // Apply a vertical velocity to the player to make them jump, the formula calculates the necessary velocity to reach the desired jump height
                velocity.y = Mathf.Sqrt(2f * -gravity * jumpHeight) * 1.5f;
            }
        }

        void FixedUpdate()
        {
            // If the player is falling (velocity.y < 0), accelerate the fall according to gravity and a multiplier for a more natural fall
            if (velocity.y < 0)
            {
                velocity.y += gravity * (fallMultiplier * 1.5f) * fallTimeout;
            }
            // If the player is moving upwards (jumping), check if the jump key is released to start decreasing the speed
            else if (velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                velocity.y += gravity * (fallMultiplier * 0.5f) * jumpTimeout;
            }
            // If player is neither falling nor rising, apply normal gravity to handle smooth transitions
            else
            {
                velocity.y += gravity * 5f * Time.fixedDeltaTime;
            }
            controller.Move(velocity * Time.fixedDeltaTime);
        }
    }
}
