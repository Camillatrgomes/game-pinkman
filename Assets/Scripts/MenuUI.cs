using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("Menu UI properties")]
    [SerializeField] private Button buttonStart;
    [SerializeField] private Button buttonOptions;
    [SerializeField] private Button buttonExit;
    [SerializeField] private GameObject optionsPanel;

    [Header("Áudio")]
    [SerializeField] private TrocaDeMusica trocaDeMusica;

    private void OnEnable()
    {
        buttonStart.onClick.AddListener(IniciarJogo);
        buttonOptions.onClick.AddListener(OpenOptionsPanel);
        buttonExit.onClick.AddListener(ExitGame);

        optionsPanel.SetActive(false);
    }

    private void OnDisable()
    {
        buttonStart.onClick.RemoveListener(IniciarJogo);
        buttonOptions.onClick.RemoveListener(OpenOptionsPanel);
        buttonExit.onClick.RemoveListener(ExitGame);
    }

    private void IniciarJogo()
    {
        // 1️⃣ Troca a música
        trocaDeMusica.TrocarParaMusicaSecundaria();

        // 2️⃣ Carrega a cena
        SceneManager.LoadScene("Demo");
    }

    private void OpenOptionsPanel()
    {
        gameObject.SetActive(false);
        optionsPanel.SetActive(true);
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
