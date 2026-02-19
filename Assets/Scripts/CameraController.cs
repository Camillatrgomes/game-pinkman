using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Header("Câmeras")]
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;

    [Header("Player")]
    public Transform player;

    [Header("Intro")]
    public float tempoIntro = 1f;

    [Header("Camera 2")]
    public float smoothMove = 1.5f;

    [Header("Zoom Camera 3")]
    public float zoomInicialCam3 = 6f;
    public float zoomFinalCam3 = 4f;
    public float duracaoZoomCam3 = 1.5f;

    private Vector3 moveVelocity;
    private bool cam2Ativa = false;
    private bool cam3Ativa = false;

    void Start()
    {
        cam1.gameObject.SetActive(true);
        cam2.gameObject.SetActive(false);
        cam3.gameObject.SetActive(false);

        StartCoroutine(IniciarCamera2());
    }

    void LateUpdate()
    {
        if (cam2Ativa)
        {
            SeguirPlayer(cam2);
        }

        if (cam3Ativa)
        {
            SeguirPlayer(cam3);
        }
    }

    IEnumerator IniciarCamera2()
    {
        yield return new WaitForSeconds(tempoIntro);

        cam1.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);
        cam2Ativa = true;
    }

    void SeguirPlayer(Camera cam)
    {
        Vector3 alvo = new Vector3(
            player.position.x,
            player.position.y,
            cam.transform.position.z
        );

        cam.transform.position = Vector3.SmoothDamp(
            cam.transform.position,
            alvo,
            ref moveVelocity,
            smoothMove
        );
    }

    // CHAMADO PELO TRIGGER
    public void AtivarCamera3()
    {
        if (cam3Ativa) return;

        cam2Ativa = false;
        cam2.gameObject.SetActive(false);

        cam3.gameObject.SetActive(true);
        cam3Ativa = true;

        cam3.orthographicSize = zoomInicialCam3;
        StartCoroutine(ZoomCamera3());
    }

    IEnumerator ZoomCamera3()
    {
        float tempo = 0f;

        while (tempo < duracaoZoomCam3)
        {
            tempo += Time.deltaTime;

            cam3.orthographicSize = Mathf.Lerp(
                zoomInicialCam3,
                zoomFinalCam3,
                tempo / duracaoZoomCam3
            );

            yield return null;
        }

        cam3.orthographicSize = zoomFinalCam3;
        ExitGame();
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
