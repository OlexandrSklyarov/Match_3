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
        [field: SerializeField] public AnimationData AnimationData {get; private set;}
    }

    [Serializable]
    public class CellViewData
    {
        [field: SerializeField] public CellViewContainer CellPrefab {get; private set;}
        [field: SerializeField] public CellViewItem[] Items {get; private set;}
        [field: SerializeField] public Sprite DefaultImage  {get; private set;}
    }

    [Serializable]
    public class AnimationData
    {
        [field: SerializeField, Min(0.01f)] public float DefaultSwapDuration  {get; private set;} = 0.15f;
        [field: SerializeField, Min(0.01f)] public float FastSwapDuration  {get; private set;} = 0.05f;
        [field: SerializeField, Min(0.01f)] public float ShakeDuration  {get; private set;} = 0.04f;
    }

    [Serializable]
    public class CellViewItem
    {
        [field: SerializeField] public ItemType Type {get; private set;}
        [field: SerializeField] public Sprite Image {get; private set;}
    }
}