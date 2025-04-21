using UnityEngine;

namespace CurlyBlue
{
    public class CharacterInput : MonoBehaviour
    {
        [field: SerializeField] public Vector2 Move   { get; set; }
        [field: SerializeField] public Vector2 Look   { get; set; }
        [field: SerializeField] public bool    Jump   { get; set; }
        [field: SerializeField] public bool    Sprint { get; set; }
    }
}