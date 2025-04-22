using UnityEngine;

namespace CurlyBlue
{
    /// <summary> Character Input Data </summary>
    public class CharacterInputData : MonoBehaviour
    {
        [field: SerializeField] public Vector2 MoveRaw   { get; set; } 
        [field: SerializeField] public Vector3 MoveWorld { get; set; } // Converted raw to world space
        [field: SerializeField] public Vector2 Look      { get; set; }
        [field: SerializeField] public bool    Jump      { get; set; }
        [field: SerializeField] public bool    Sprint    { get; set; }
    }
}