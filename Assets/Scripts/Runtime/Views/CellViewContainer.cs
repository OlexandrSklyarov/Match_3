using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

namespace AS.Runtime.Views
{
    public class CellViewContainer : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
    {
        public CellView MyView => _view;

        [SerializeField] private Image _imageBackground;
        [SerializeField] private CellView _view;
        [SerializeField, Min(0.01f)] private float _moveSpeed = 0.15f;

        private RectTransform _rect;

        public event Action PointerDownEvent;
        public event Action SelectEvent;

        private void Awake() 
        { 
            _rect = GetComponent<RectTransform>();  
        }

        public void SetImageBG(Sprite sprite) => _imageBackground.sprite = sprite;

        public void SetImage(Sprite sprite) => _view.SetImage(sprite);

        public void SetPosition(Vector2 pos) => _rect.anchoredPosition = pos;

        public float GetSize() => _rect.sizeDelta.x;
        

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            PointerDownEvent?.Invoke();
        }
        
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {            
            SelectEvent?.Invoke();            
        }                 

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