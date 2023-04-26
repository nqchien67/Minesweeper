using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class CustomNewGameButton : MonoBehaviour
    {
        public TMP_InputField widthInput;
        public TMP_InputField heightInput;
        public TMP_InputField minesInput;

        private void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() => NewGame());
        }

        private void NewGame()
        {
            int width = int.Parse(widthInput.text);
            int height = int.Parse(heightInput.text);
            int mines = int.Parse(minesInput.text);
            GameObject.FindWithTag("GameController").GetComponent<Game>().NewGame(width, height, mines);
        }
    }
}