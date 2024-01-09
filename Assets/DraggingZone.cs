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
/*        private float xStartPos;
        private float yStartPos;
        private float zStartPos;*/

        [Header("Team")]
        private Transform _teamTransform;
        [SerializeField] private Vector2 _teamFollower;
        [SerializeField] private GameObject _leaderTeam1;
        [SerializeField] private GameObject _leaderTeam2;
        [SerializeField] private float moveSpeed = 0.5f;
        // Start is called before the first frame update
        void Start()
        {
/*            xStartPos = _camera.transform.position.x;
            yStartPos = _camera.transform.position.y;
            zStartPos = _camera.transform.position.z;*/
            _startCamPos = _camera.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            _teamFollower.x = ((_leaderTeam1.transform.position.x + _leaderTeam2.transform.position.x) / 2) + 1f;
            float width = 2 * (_cameraSize - _maxWidthDrag);
            if (!_dragging && Mathf.Abs(_camera.position.x - _teamFollower.x) > 0.02f)
            {
                float xClamped = Mathf.Clamp(_teamFollower.x, _startCamPos.x - _maxWidthDrag, _startCamPos.x + _maxWidthDrag);
                _camera.position = Vector3.Lerp(_camera.position, new Vector3(xClamped, _startCamPos.y, _camera.position.z), 2.5f * Time.deltaTime);
            }
        }

        public void OnPointerMove(PointerEventData eventData)
        {

            if (!_dragging) return;
            Vector2 displacement = eventData.delta*-1;
            float xClamped = Mathf.Clamp(_camera.position.x + displacement.x * _dragSpeed,
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_camera.position, new Vector3(2*(_cameraSize + _maxWidthDrag), _cameraSize*2,0));
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector2(_teamFollower.x, _teamFollower.y), new Vector3(2*(_cameraSize - _maxWidthDrag), _cameraSize*2, 0));
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(new Vector2(_teamFollower.x, _teamFollower.y), 0.5f);
        }

        private void OnValidate()
        {
            _cameraSize = _camera.GetComponent<Camera>().orthographicSize;
            _startCamPos = _camera.transform.position;
        }
    }
}
