using UnityEngine;
using UnityEngine.UI;

namespace AS.Runtime.Views
{
    public class CellView : MonoBehaviour
    {
        public Vector2 MyPosition => transform.position;
        
        [SerializeField] private Image _image;

        public void SetImage(Sprite sprite) => _image.sprite = sprite;
    }
}