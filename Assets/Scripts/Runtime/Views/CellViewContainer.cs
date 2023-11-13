using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

namespace AS.Runtime.Views
{
    public class CellViewContainer : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, 
        IBeginDragHandler, IDragHandler,IEndDragHandler
    {
        public CellView MyView => _view;

        [SerializeField] private Image _imageBackground;
        [SerializeField] private CellView _view;
        [SerializeField, Min(0.01f)] private float _moveSpeed = 0.15f;

        private RectTransform _rect;
        private bool _isSelectedState;

        public event Action PointerDownEvent;
        public event Action PointerUpEvent;
        public event Action SelectEvent;

        private void Awake() 
        { 
            _rect = GetComponent<RectTransform>();  
            ResetSelected();
        }

        public void SetImage(Sprite sprite) => _view.SetImage(sprite);

        public void SetPosition(Vector2 pos) => _rect.anchoredPosition = pos;

        public float GetSize() => _rect.sizeDelta.x;

        public void ResetState()
        {
            ResetSelected();
            _isSelectedState = false;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            Clicked();
            _isSelectedState = true;
            PointerDownEvent?.Invoke();
        }
        
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!_isSelectedState)
            {
                Selected();
                SelectEvent?.Invoke();
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (!_isSelectedState)
                ResetSelected();
        }   

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {}

        void IDragHandler.OnDrag(PointerEventData eventData) {}

        void IEndDragHandler.OnEndDrag(PointerEventData eventData) => PointerUpEvent?.Invoke();

        private void Clicked() => SetBgColor(Color.red);

        private void ResetSelected() => SetBgColor(Color.grey);

        private void Selected() => SetBgColor(Color.yellow);
        private void SetBgColor(Color color) => _imageBackground.color = color;

        public void SetView(CellView newView)
        {
            _view = newView;
            _view.transform.SetParent(transform);
            _view.transform.DOLocalMove(Vector2.zero, _moveSpeed);
        }

        public void ViewMoveAndReturn(Vector2 tempPosition)
        {
            _view.transform
                .DOMove(tempPosition, _moveSpeed)
                .OnComplete(() => _view.transform.DOLocalMove(Vector2.zero, _moveSpeed).SetEase(Ease.InOutSine));
        }        
    }
}