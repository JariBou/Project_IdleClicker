using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectClicker
{
    public class DraggingZone : MonoBehaviour, IPointerMoveHandler, IPointerUpHandler, IPointerDownHandler
    {
        private bool _dragging = false;

        private Vector2 _lastMousePos;

        [SerializeField] private Transform _camera;

        [SerializeField] private float _dragSpeed = 0.5f;

        [SerializeField] private float _maxWidthDrag = 120;

        private float _cameraSize;

        private Vector3 _startCamPos;
        private float xStartPos;
        private float yStartPos;
        private float zStartPos;
        // Start is called before the first frame update
        void Start()
        {
            xStartPos = _camera.transform.position.x;
            yStartPos = _camera.transform.position.y;
            zStartPos = _camera.transform.position.z;
            _startCamPos = _camera.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!_dragging) return;
            
            Vector2 displacement = eventData.delta*-1;
            float xClamped = Mathf.Clamp(_camera.transform.position.x + displacement.x * _dragSpeed,
                _startCamPos.x-_maxWidthDrag, _startCamPos.x+_maxWidthDrag);
            Vector3 newPos = new Vector3(xClamped, _startCamPos.y, _startCamPos.z);
            _camera.transform.position = newPos;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _dragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _dragging = false;
        }

        #if  UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_startCamPos, new Vector3(2*(_cameraSize + _maxWidthDrag), _cameraSize*2, 0));
        }

        private void OnValidate()
        {
            _cameraSize = _camera.GetComponent<Camera>().orthographicSize;
            _startCamPos = _camera.transform.position;
        }
#endif
    }
}
