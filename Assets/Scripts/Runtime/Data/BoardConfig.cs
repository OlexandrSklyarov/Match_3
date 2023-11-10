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
        [field: SerializeField] public CellView CellPrefab {get; private set;}
        [field: SerializeField] public CellViewData[] Views {get; private set;}
    }

    [Serializable]
    public class CellViewData
    {
        [field: SerializeField] public ItemType Type {get; private set;}
        [field: SerializeField] public Sprite Image {get; private set;}
    }
}