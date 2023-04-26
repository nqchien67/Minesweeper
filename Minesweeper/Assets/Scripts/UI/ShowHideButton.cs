using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ShowHideButton : MonoBehaviour
    {
        public GameObject target;

        private void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() => Action());
        }

        private void Action()
        {
            if (target.activeSelf)
            {
                target.SetActive(false);
            }
            else
            {
                target.SetActive(true);
            }
        }
    }
}