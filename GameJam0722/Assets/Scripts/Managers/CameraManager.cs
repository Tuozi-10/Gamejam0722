using DG.Tweening;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(Camera))]
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager instance;

        private Camera m_camera;

        [SerializeField] private float m_speedZoom = 1.5f;
        [SerializeField] private float m_durationZoom = 0.35f;
        
        [SerializeField] private float m_minZoom = 6.1f;
        [SerializeField] private float m_maxZoom = 4.1f;
        
        private void Awake()
        {
            if (instance is not null)
            {
                Destroy(this);
                return;
            }

            m_camera = GetComponent<Camera>();
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        private void Update()
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                m_camera.DOOrthoSize(Mathf.Max(m_maxZoom, m_camera.orthographicSize-m_speedZoom), m_durationZoom);
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                m_camera.DOOrthoSize(Mathf.Min(m_minZoom, m_camera.orthographicSize+m_speedZoom), m_durationZoom);
            
            }
        }
    }
}
