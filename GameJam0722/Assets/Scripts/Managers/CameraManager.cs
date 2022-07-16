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

        private void FixedUpdate()
        {
         ManageZoom();
         ManageMove();
        }

        private Vector2 MousePos;
        
        private void ManageMove()
        {
            if (Input.GetMouseButton(2))
            {
                Debug.Log("in");
             
                if (MousePos.x > -6666 && MousePos.y > -6666)
                {
                    Move();
                }
                
                MousePos.x = Input.mousePosition.x;
                MousePos.y = Input.mousePosition.y;
              
            }
            else
            {
                MousePos.x = MousePos.y = -6666;
            }
        }

        private void Move()
        {
            transform.DOKill();

            Vector3 NextPos = transform.localPosition + (m_camera.orthographicSize * 0.005f * new Vector3(MousePos.x - Input.mousePosition.x,
                                                        MousePos.y - Input.mousePosition.y, 0));

            NextPos.x = Mathf.Clamp(NextPos.x, -5, 5);
            NextPos.y = Mathf.Clamp(NextPos.y, -5, 5);
            NextPos.z = -30;
            transform.DOLocalMove(NextPos , 0.15f);
        }
        
        private void ManageZoom()
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
