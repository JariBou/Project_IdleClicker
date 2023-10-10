using System.Collections;
using System.Collections.Generic;
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
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!_dragging) return;
            
            Vector2 displacement = eventData.delta*-1;
            _camera.transform.Translate(displacement.x * _dragSpeed, 0, 0);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _dragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _dragging = false;
        }
    }
}
