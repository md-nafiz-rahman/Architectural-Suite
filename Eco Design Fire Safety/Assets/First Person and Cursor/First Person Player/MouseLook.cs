
// MouseLook.cs handles camera control for a first-person perspective, allowing the player to look around with the mouse.
// This script is part of the Modular First Person Controller from the Unity Asset Store.
// Reference: Unity Asset Store, "Modular First Person Controller." https://assetstore.unity.com/packages/3d/characters/modular-first-person-controller-189884 (accessed Jan. 2, 2024).

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
    public class MouseLook : MonoBehaviour
    {

        public float mouseXSensitivity = 100f;

        public Transform playerBody;

        float xRotation = 0f;

        void Start()
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseXSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}