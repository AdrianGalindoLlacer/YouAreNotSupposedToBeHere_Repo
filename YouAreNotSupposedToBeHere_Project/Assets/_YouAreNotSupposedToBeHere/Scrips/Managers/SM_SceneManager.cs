using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
 
[RequireComponent(typeof(VideoPlayer))]
public class SM_SceneManager: MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string nombreEscena = "MenuPrincipal";
    [SerializeField] private bool permitirSaltar = true;
    [SerializeField] private KeyCode teclaSaltar = KeyCode.Space;
 
    private bool cambiando = false;
 
    void Awake()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();
 
        // Configuración recomendada para evitar problemas de audio/sincronía
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;
        videoPlayer.skipOnDrop = true;
        videoPlayer.waitForFirstFrame = true;
    }
 
    void Start()
    {
        videoPlayer.prepareCompleted += AlPrepararse;
        videoPlayer.loopPointReached += AlTerminar;
        videoPlayer.errorReceived += AlHaberError;
 
        // Prepara el vídeo (llena el búfer) antes de reproducirlo
        videoPlayer.Prepare();
    }
 
    void Update()
    {
        if (permitirSaltar && Input.GetKeyDown(teclaSaltar))
            CambiarEscena();
    }
 
    void AlPrepararse(VideoPlayer vp)
    {
        vp.Play();
    }
 
    void AlTerminar(VideoPlayer vp)
    {
        CambiarEscena();
    }
 
    void AlHaberError(VideoPlayer vp, string mensaje)
    {
        Debug.LogError("Error en el VideoPlayer: " + mensaje);
        CambiarEscena(); // Para que el juego no se quede atascado
    }
 
    void CambiarEscena()
    {
        if (cambiando) return;
        cambiando = true;
        SceneManager.LoadScene(nombreEscena);
    }
 
    void OnDestroy()
    {
        if (videoPlayer == null) return;
        videoPlayer.prepareCompleted -= AlPrepararse;
        videoPlayer.loopPointReached -= AlTerminar;
        videoPlayer.errorReceived -= AlHaberError;
    }
}
