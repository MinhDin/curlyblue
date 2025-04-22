using Fusion;
using UnityEngine;

namespace CurlyBlue
{
    /// <summary> Store input data </summary>
    public class CharacterInputData : NetworkBehaviour
    {
        [field: SerializeField] [Networked] public Vector2 MoveRaw   { get; set; }
        [field: SerializeField] [Networked] public Vector3 MoveWorld { get; set; } // Converted from raw to world space
        [field: SerializeField] [Networked] public Vector2 Look      { get; set; }
        [field: SerializeField] [Networked] public bool    Jump      { get; set; }
        [field: SerializeField] [Networked] public bool    Sprint    { get; set; }
    }
}