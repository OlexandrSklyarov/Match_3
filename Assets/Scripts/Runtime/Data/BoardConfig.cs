using System;
using AS.Runtime.Models;
using AS.Runtime.Views;
using UnityEngine;

namespace AS.Runtime.Data
{
    [CreateAssetMenu(menuName = "SO/BoardConfig", fileName = "BoardConfig")]
    public class BoardConfig : ScriptableObject
    {
        [field: SerializeField] public Vector2Int Size {get; private set;}
        [field: SerializeField] public CellViewData ViewData {get; private set;}
    }

    [Serializable]
    public class CellViewData
    {
        [field: SerializeField] public CellViewContainer CellPrefab {get; private set;}
        [field: SerializeField] public CellViewItem[] Items {get; private set;}
        [field: SerializeField] public Sprite DefaultImage  {get; private set;}        
    }

    [Serializable]
    public class CellViewItem
    {
        [field: SerializeField] public ItemType Type {get; private set;}
        [field: SerializeField] public Sprite Image {get; private set;}
    }
}