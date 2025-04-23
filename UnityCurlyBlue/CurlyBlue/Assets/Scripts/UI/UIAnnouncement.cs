using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CurlyBlue
{
    public class UIAnnouncement : MonoBehaviour
    {
        const string Format = "{0} has completed the level!!!";

        public float           TimeShow = 5;
        public TextMeshProUGUI Text;

        private Sequence _sequence;
        
        public void Show(string playerName)
        {
            _sequence?.Kill();

            Text.text = string.Format(Format, playerName);
            
            var startColor = Text.color;
            startColor.a = 0f;
            Text.color   = startColor;
            
            _sequence = DOTween.Sequence();
            _sequence.Append(Text.DOFade(1, 1));
            _sequence.AppendInterval(TimeShow);
            _sequence.Append(Text.DOFade(0, 1));
        }
    }
}