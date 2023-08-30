using Cinemachine;
using Oblation.PlayerInputSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Oblation.PlayerSystem
{
    public class CameraZoomToTarget : MonoBehaviour
    {
        Vector3 m_DefaultCamOffset;
        bool m_bIsActive;

        CinemachineFramingTransposer m_Cam;

        [SerializeField] [ValidateInput("@$value > 0", "Distance has to be greater than zero")]
        [Range(0.1f, 20f)]
        float m_LookDistance;

        void OnEnable()
        {
            PlayerInputs.e_OnCameraZoomPressed += OnZoomPressed;
            PlayerInputs.e_OnCameraZoomCancelled += OnZoomCancelled;
        }

        void OnDisable()
        {
            PlayerInputs.e_OnCameraZoomPressed -= OnZoomPressed;
            PlayerInputs.e_OnCameraZoomCancelled -= OnZoomCancelled;
        }

        void Awake()
        {
            m_Cam = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
            m_DefaultCamOffset = m_Cam.m_TrackedObjectOffset;
        }

        void Update() => HandleCinemachineZoom();

        void HandleCinemachineZoom()
        {
            if (Player.s_Instance == null) return;
            if (!m_bIsActive) return;
            if (Camera.main is null) return;
            
            var dirToMouse =
                Player.s_Instance.transform.position.Direction(Camera.main.ScreenToWorldPoint(PlayerInputs.m_MousePosition));

            SetCameraOffset(dirToMouse * m_LookDistance);
        }

        void SetCameraOffset(Vector3 target) => m_Cam.m_TrackedObjectOffset = m_DefaultCamOffset + target;

        void OnZoomPressed() => m_bIsActive = true;
        void OnZoomCancelled()
        {
            m_bIsActive = false;
            SetCameraOffset(m_DefaultCamOffset);
        }
    }
}