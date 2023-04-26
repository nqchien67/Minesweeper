using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class NewGameButton : MonoBehaviour
    {
        public int width;
        public int height;
        public int mines;

        private void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() => NewGame());
        }

        private void NewGame()
        {
            GameObject.FindWithTag("GameController").GetComponent<Game>().NewGame(width, height, mines);
        }
    }
}