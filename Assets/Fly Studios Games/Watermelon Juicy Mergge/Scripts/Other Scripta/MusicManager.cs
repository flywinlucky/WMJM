using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicSource;
    [ReadOnly] public float musicDuration;

    public float pauseInterval;
    public float fadeClipDuration;
    static bool isCreated = false;
    float clipCurentVolume;

    // Start is called before the first frame update
    void Start()
    {
        clipCurentVolume = musicSource.volume;

        // Verificăm dacă avem un clip audio asociat
        if (musicSource.clip != null)
        {
            // Obținem durata clipului audio și o salvăm în musicDuration
            musicDuration = musicSource.clip.length;
        }

        // Dacă obiectul nu a fost încă creat
        if (!isCreated)
        {
            // Îl facem nemuritor
            DontDestroyOnLoad(gameObject);
            isCreated = true; // Setăm flagul ca obiectul a fost creat
        }
        else
        {
            // Dacă obiectul a fost deja creat în altă scenă, distrugem duplicatul
            Destroy(gameObject);
        }

        // Începem să redăm muzica imediat la startul jocului
        PlayFadeMusic();
    }

    [Button]
    public void PlayFadeMusic()
    {
        //Debug.LogError("PlayFadeMusic");

        // Setăm volumul la 0 și începem redarea
        musicSource.volume = 0f;
        musicSource.Play();

        // Aplicăm efectul de fade-in folosind DOTween
        musicSource.DOFade(clipCurentVolume, fadeClipDuration)
            .OnComplete(() =>
            {
                // Aici verificăm dacă muzica s-a terminat și apelăm StopFadeMusic
                StartCoroutine(WaitForMusicEnd());

                //Debug.LogError("WaitForMusicEnd");
            });
    }

    IEnumerator WaitForMusicEnd()
    {
        // Așteptăm până când muzica se termină de redat
        yield return new WaitForSeconds(musicDuration);

        // Oprim muzica cu un efect de fade-out și apoi o reluăm după o pauză
        StopFadeMusic();

        yield return new WaitForSeconds(pauseInterval);
        PlayFadeMusic();
    }

    [Button]
    public void StopFadeMusic()
    {
        //Debug.LogError("StopFadeMusic");
       
        // Aplicăm efectul de fade-out
        musicSource.DOFade(0f, fadeClipDuration)
            .OnComplete(() => musicSource.Stop());
    }
}