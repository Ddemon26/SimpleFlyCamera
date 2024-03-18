using UnityEngine;

namespace Damon.SimpleFlyCamera
{
    public class SimpleCameraController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public MoveAxis Horizontal = new MoveAxis(KeyCode.D, KeyCode.A);
        public MoveAxis Vertical = new MoveAxis(KeyCode.W, KeyCode.S);
        public MoveAxis Up = new MoveAxis(KeyCode.E, KeyCode.Q);

        [Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
        public float Boost = 3.5f;

        [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
        public float PositionLerpTime = 0.2f;

        [Header("Rotation Settings")]
        [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
        public AnimationCurve MouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

        [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
        public float RotationLerpTime = 0.01f;

        [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
        public bool InvertY = false;

        private CameraState targetCameraState = new CameraState();
        private CameraState interpolatingCameraState = new CameraState();

        private void OnEnable()
        {
            targetCameraState.SetFromTransform(transform);
            interpolatingCameraState.SetFromTransform(transform);
        }

        private void Update()
        {
            HandleInput();
            UpdateCameraPosition();
            UpdateCameraRotation();
        }

        private void HandleInput()
        {
            // Right mouse button: lock/unlock cursor
            if (Input.GetMouseButtonDown(1)) Cursor.lockState = CursorLockMode.Locked;
            if (Input.GetMouseButtonUp(1))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            // Scroll wheel: Adjust boost
            Boost += Input.mouseScrollDelta.y * 0.2f;
        }

        private Vector3 GetInputTranslationDirection()
        {
            // Simplified to directly use the axis values
            return new Vector3(Horizontal, Up, Vertical);
        }

        private void UpdateCameraPosition()
        {
            var translation = GetInputTranslationDirection() * Time.deltaTime;

            // Speed up movement when shift key held
            if (Input.GetKey(KeyCode.LeftShift)) translation *= 10.0f;

            // Apply boost
            translation *= Mathf.Pow(2.0f, Boost);

            targetCameraState.Translate(translation);

            // Interpolate towards the target state
            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / PositionLerpTime) * Time.deltaTime);
            var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / RotationLerpTime) * Time.deltaTime);
            interpolatingCameraState.LerpTowards(targetCameraState, positionLerpPct, rotationLerpPct);
            interpolatingCameraState.UpdateTransform(transform);
        }

        private void UpdateCameraRotation()
        {
            if (Input.GetMouseButton(1)) // Right mouse button held
            {
                var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (InvertY ? 1 : -1));
                var mouseSensitivityFactor = MouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

                targetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor;
                targetCameraState.pitch += mouseMovement.y * mouseSensitivityFactor;
            }
        }
    }
}