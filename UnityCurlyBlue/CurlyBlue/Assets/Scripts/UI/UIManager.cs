using Fusion;
using UnityEngine;

namespace CurlyBlue
{
    public class UIManager : MonoBehaviour
    {
        public UIData         UIData;
        public UIMainMenu     MainMenu;
        public UIInGameOption InGameOption;
        public CurlyBlueGame  Game;

        public void ShowMainMenu()
        {
            InGameOption.Hide();
            MainMenu.Show(this);
        }

        public void ShowInGameOption()
        {
            MainMenu.Hide();
            InGameOption.Show(this);
        }
    }

    public abstract class UIView : MonoBehaviour
    {
        protected UIManager _uiManager;

        public virtual void Show(UIManager uiManager)
        {
            _uiManager = uiManager;
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}