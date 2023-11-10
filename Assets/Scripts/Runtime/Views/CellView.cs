using UnityEngine;
using UnityEngine.UI;

namespace AS.Runtime.Views
{
    [RequireComponent(typeof(Image))]
    public class CellView : MonoBehaviour
    {
        private Image _image;
        private RectTransform _rect;

        private void Awake() 
        {
            _image = GetComponent<Image>();  
            _rect = GetComponent<RectTransform>();  
        }

        public void SetImage(Sprite sprite) => _image.sprite = sprite;

        public void SetPosition(Vector2 pos) => _rect.anchoredPosition = pos;

        public float GetSize() => _rect.sizeDelta.x;
    }
}