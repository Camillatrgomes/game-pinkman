using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button buttonExit;
    [SerializeField] private GameObject menuUI;

    private void OnEnable()
    {
        buttonExit.onClick.AddListener(ExitMenu);
    }

    private void OnDisable()
    {
        buttonExit.onClick.RemoveListener(ExitMenu);
    }

    private void ExitMenu()
    {
        menuUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
