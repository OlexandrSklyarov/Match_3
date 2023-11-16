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

        public void SetView(CellView newView, float duration)
        {
            _view = newView;
            _view.transform.SetParent(transform);
            _view.transform.DOLocalMove(Vector2.zero, duration);
        }

        public void ViewMoveAndReturn(Vector2 tempPosition, float duration)
        {
            _view.transform
                .DOMove(tempPosition, duration)
                .OnComplete(() => _view.transform.DOLocalMove(Vector2.zero, duration).SetEase(Ease.InOutSine));
        }

        public void SetImageWithShake(Sprite sprite, float shakeDuration)
        {
            _view.transform
                .DOShakePosition(shakeDuration, 5f)
                .OnComplete(() => _view.SetImage(sprite));
            
        }
    }
}