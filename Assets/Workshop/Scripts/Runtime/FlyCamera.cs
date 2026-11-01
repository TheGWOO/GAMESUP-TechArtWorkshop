using UnityEngine;
using UnityEngine.InputSystem;

namespace Workshop
{
    public sealed class FlyCamera : MonoBehaviour
    {
        [SerializeField] private float _speed = 6f;
        [SerializeField] private float _sensitivity = .15f;

        private float _yaw;
        private float _pitch;

        private void OnEnable()
        {
            _yaw = transform.eulerAngles.y;
            _pitch = Mathf.DeltaAngle(0f, transform.eulerAngles.x);
            SetCursor(true);
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            Mouse mouse = Mouse.current;
            if (keyboard == null || mouse == null)
                return;

            if (keyboard.escapeKey.wasPressedThisFrame)
            {
                SetCursor(false);
                return;
            }

            if (Cursor.lockState != CursorLockMode.Locked)
            {
                if (mouse.leftButton.wasPressedThisFrame)
                    SetCursor(true);
                return;
            }

            Vector2 look = mouse.delta.ReadValue() * _sensitivity;
            _yaw += look.x;
            _pitch = Mathf.Clamp(_pitch - look.y, -85f, 85f);
            transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);

            // Physical positions: ZQSD on AZERTY, WASD on QWERTY.
            float x = (keyboard.dKey.isPressed ? 1f : 0f) - (keyboard.aKey.isPressed ? 1f : 0f);
            float z = (keyboard.wKey.isPressed ? 1f : 0f) - (keyboard.sKey.isPressed ? 1f : 0f);
            float y = (keyboard.eKey.isPressed ? 1f : 0f) - (keyboard.qKey.isPressed ? 1f : 0f);
            Vector3 movement = transform.right * x + Vector3.up * y + transform.forward * z;
            transform.position += Vector3.ClampMagnitude(movement, 1f) * (_speed * Time.deltaTime);
        }

        private static void SetCursor(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }

        private void OnDisable() => SetCursor(false);
    }
}
