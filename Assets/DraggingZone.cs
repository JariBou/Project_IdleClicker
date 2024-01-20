using ProjectClicker.Enemies;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using ProjectClicker.Core;

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
        [FormerlySerializedAs("moveSpeed")] [SerializeField] private float _moveSpeed = 0.5f;


        [Header("Clicking on Enemies")]
        private TeamStats _teamStats;
        private Camera _cameraCamera;
        [SerializeField] private LayerMask _layerMask;
        private Collider2D _collider;
        [SerializeField] private GameObject _display;
        // Start is called before the first frame update
        private void Start()
        {
/*            xStartPos = _camera.transform.position.x;
            yStartPos = _camera.transform.position.y;
            zStartPos = _camera.transform.position.z;*/
            _startCamPos = _camera.transform.position;
            _teamStats = GameObject.FindWithTag("Team").GetComponent<TeamStats>();
            _cameraCamera = GameObject.FindWithTag("CameraSlider").GetComponent<Camera>();
        }

        // Update is called once per frame
        private void Update()
        {
            _teamFollower.x = (_leaderTeam1.transform.position.x + _leaderTeam2.transform.position.x) / 2 + 1f;
            if (!_dragging && Mathf.Abs(_camera.position.x - _teamFollower.x) > 0.02f)
            {
                float xClamped = Mathf.Clamp(_teamFollower.x, _startCamPos.x - _maxWidthDrag, _startCamPos.x + _maxWidthDrag);
                _camera.position = Vector3.Lerp(_camera.position, new Vector3(xClamped, _startCamPos.y, _camera.position.z), 2.5f * Time.deltaTime);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector2 newMousePosition = Utils.ConvertPosCamToCam(Input.mousePosition, _display.transform.position, Camera.main, _cameraCamera);

                /*Debug.DrawLine(Vector3.zero, newMousePosition, Color.red, 3f);*/

                Collider2D collider = Physics2D.OverlapPoint(newMousePosition, _layerMask);
                if (collider != null)
                {
                    collider.GetComponent<EnemiesBehavior>().TakeDamage(_teamStats.Damage * 0.5f);// Sinon c'est trop facile

                }
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
