using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BGM{
    Boss, Normal
}
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource musicPlayer; 

    // 音樂
    [SerializeField] AudioClip BossBGM;
    [SerializeField] AudioClip NormalBGM;

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

    public static void PlayMusic (){
        instance.musicPlayer.Play();
        Debug.Log("Playmusic");
    }

    public static void SwitchMusic(BGM bgm){
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

        }
    }


    public static void StopMusic(){
        instance.musicPlayer.Stop();
        Debug.Log("Stopmusic");
    }
}
