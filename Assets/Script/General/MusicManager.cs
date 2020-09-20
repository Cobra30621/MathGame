using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BGM{
    Boss, Normal, Pass, Dead
}
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource musicPlayer; 

    // 音樂
    [SerializeField] AudioClip BossBGM;
    [SerializeField] AudioClip NormalBGM;
    [SerializeField] AudioClip Pass;
    [SerializeField] AudioClip Dead;

    void Awake () {
        if (instance==null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            musicPlayer = GetComponent<AudioSource>();
        }
        else if (this!=instance)
        {
            Destroy(gameObject);
        }      

        
    }

    public static void PlayMusicAtOnce (BGM bgm){
        instance.Switch(bgm);
        instance.musicPlayer.loop = false;
        instance.musicPlayer.Play();
        Debug.Log($"Playmusic{bgm}");
    }


    public static void PlayMusic (BGM bgm){
        instance.Switch(bgm);
        instance.musicPlayer.loop = true;
        instance.musicPlayer.Play();
        Debug.Log($"Playmusic{bgm}");
    }

    public static void PlayMusic (){
        instance.musicPlayer.loop = true;
        instance.musicPlayer.Play();
        Debug.Log("Playmusic");
    }

    public static void SwitchMusic(BGM bgm){
        Debug.Log($"SwitchMusic{bgm}");
        instance.Switch(bgm);
    }

    public void Switch(BGM bgm){
        switch (bgm){
            case BGM.Normal:
                musicPlayer.clip = NormalBGM;
                break;
            case BGM.Boss:
                musicPlayer.clip = BossBGM;
                break;
            case BGM.Pass:
                musicPlayer.clip = Pass;
                break;
            case BGM.Dead:
                musicPlayer.clip = Dead;
                break;

        }
    }


    public static void StopMusic(){
        instance.musicPlayer.Stop();
        Debug.Log("Stopmusic");
    }
}
