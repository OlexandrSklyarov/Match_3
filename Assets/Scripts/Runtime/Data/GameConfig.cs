using UnityEngine;

namespace AS.Runtime.Data
{
    [CreateAssetMenu(menuName = "SO/GameConfig", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        
        [field: Header("Board"),SerializeField] public BoardConfig Board {get; private set;}

    }
}