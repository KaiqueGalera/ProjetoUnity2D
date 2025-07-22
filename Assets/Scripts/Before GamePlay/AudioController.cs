using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{   
    [Header("Config. Musica e Sons")]
    public      AudioSource         music;
    public      AudioSource         fx;
    private     AudioClip           newMusic;

    [Header("Musicas")]
    public      AudioClip           titleMusic;
    public      AudioClip           gamePlayMusic;
    public      AudioClip           gameOverMusic;
    public      AudioClip           gamePlayMusic2;
    public      AudioClip           gameOverMusic3;
    public      AudioClip           gameOverMusic4;

    [Header("Fx Hero")]
    public      AudioSource         sDmgHeroFx;
    public      AudioSource         sRevive;
    public      AudioSource         sJump;
    public      AudioClip           dmgHeroFx;
    public      AudioClip           revive;
    public      AudioClip           jump;

    [Header("Fx Armadilhas")]
    public      AudioSource         sExplosion;
    public      AudioSource         sMouthTrapSound;
    public      AudioSource         sLaserShoot;
    public      AudioClip           explosion;
    public      AudioClip           mouthTrapSound;
    public      AudioClip           LaserShoot;

    [Header("Controle das fases")]
    public  bool                    isFaseEqDone = false;
    public  bool                    isFaseDocDone = false;
    [Header("Troca de cena")]
    
    private     IsDone              _IsDone;
    private     string              sceneName;
    public      Animator            fadeAn;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _IsDone =    FindFirstObjectByType(typeof(IsDone)) as IsDone;

        music.clip = titleMusic;
        music.loop = true;
        music.Play();
    }
    
    // Start is called before the first frame update
    void Start()
    {

        SceneManager.LoadScene("Title");;
        

    }

    public void ChangeScene(string scene, bool isChangeMusic, AudioClip music)
    {
        switch (isChangeMusic)
        {
            case true:
                newMusic = music;
                sceneName = scene;
                
                StartCoroutine("changeMusicScene");
            break;

            case false:
                SceneManager.LoadScene(scene);
            break;
        }
    }

    IEnumerator changeMusicScene()
    {
        FadeIn(); // Chama a função para escurecer a tela

        float volumeMax = music.volume;

        for (float volume = music.volume; volume > 0; volume -= 0.01f) // Abaixa gradualmente o volume da música a cada frame
        {
            music.volume = volume;

            yield return new WaitForEndOfFrame(); // Diminui o volume a cada frame
        }

        music.clip = newMusic; // Recebe nova música
        music.Play(); // Inicia a nova música
        
        yield return    new WaitUntil(() => _IsDone.isFadeDone == true); // Aguarda até que o fade tenha terminado
        
        SceneManager.LoadScene(sceneName);

        for (float volume = 0; volume < volumeMax; volume += 0.01f) // Aumenta o volume da música (agora trocada) gradualmente a cada frame
        {
            music.volume = volume;

            yield return new WaitForEndOfFrame(); // Diminui o volume a cada frame
        }
    }

    public  void FadeIn()
    {
        _IsDone.isFadeDone = false;

        fadeAn.SetTrigger("FadeIn");
    }
    public  void FadeOut()
    {
        _IsDone.isFadeDone = false;
        
        fadeAn.SetTrigger("FadeOut");
    }

    // public void LoadNewScene(string sceneName)
    // {
    //     SceneManager.LoadScene(sceneName);
    // }

}
