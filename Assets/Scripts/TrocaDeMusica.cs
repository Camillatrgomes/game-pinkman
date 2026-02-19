using System.Collections;
using UnityEngine;

public class TrocaDeMusica : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioSourcePrincipal;
    [SerializeField] private AudioSource audioSourceSecundario;

    [Header("Configurações de Fade")]
    [SerializeField] private float tempoFade = 0.5f;

    public static TrocaDeMusica Instancia;

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Garante estados iniciais
        audioSourcePrincipal.volume = 1f;
        audioSourceSecundario.volume = 0f;

        if (!audioSourcePrincipal.isPlaying)
            audioSourcePrincipal.Play();
    }

    // CHAMAR AO CLICAR EM START
    public void TrocarParaMusicaSecundaria()
    {
        StopAllCoroutines();
        StartCoroutine(FadeParaSecundaria());
    }

    // OPCIONAL: voltar para música do menu
    public void TrocarParaMusicaPrincipal()
    {
        StopAllCoroutines();
        StartCoroutine(FadeParaPrincipal());
    }

    IEnumerator FadeParaSecundaria()
    {
        // Sincroniza tempo
        audioSourceSecundario.time = audioSourcePrincipal.time;

        if (!audioSourceSecundario.isPlaying)
            audioSourceSecundario.Play();

        float tempo = 0f;

        while (tempo < tempoFade)
        {
            tempo += Time.deltaTime;

            audioSourcePrincipal.volume = Mathf.Lerp(1f, 0f, tempo / tempoFade);
            audioSourceSecundario.volume = Mathf.Lerp(0f, 1f, tempo / tempoFade);

            yield return null;
        }

        audioSourcePrincipal.Stop();
        audioSourcePrincipal.volume = 1f;
        audioSourceSecundario.volume = 1f;
    }

    IEnumerator FadeParaPrincipal()
    {
        audioSourcePrincipal.time = audioSourceSecundario.time;

        if (!audioSourcePrincipal.isPlaying)
            audioSourcePrincipal.Play();

        float tempo = 0f;

        while (tempo < tempoFade)
        {
            tempo += Time.deltaTime;

            audioSourceSecundario.volume = Mathf.Lerp(1f, 0f, tempo / tempoFade);
            audioSourcePrincipal.volume = Mathf.Lerp(0f, 1f, tempo / tempoFade);

            yield return null;
        }

        audioSourceSecundario.Stop();
        audioSourceSecundario.volume = 1f;
        audioSourcePrincipal.volume = 1f;
    }
}
