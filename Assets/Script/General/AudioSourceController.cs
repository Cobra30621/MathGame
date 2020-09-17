using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController
{

    //此指令碼不需要繼承MonoBehaviour       
    public static Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>();


    public static void PlaySound(string soundName){
        PlaySnd( "Sound", soundName);
    }

    /// <summary>
    /// 需要播放某個音效的時候需要呼叫此方法就可以了
    /// </summary>
    /// <param name="dir">這是你音效的路徑, 必須在Resources目錄下</param>
    /// <param name="name">音效的名稱</param>
    public static void PlaySnd(string dir, string name)
    {

        AudioClip clip = LoadClip(dir, name.ToLower());
        if (clip != null){
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);   // 播放音樂的位置為攝影機位置
            Debug.Log("播放音效:" + name);
        }
        else                                                   // 如果主攝像機離這個位置遠的話會出現聲音小或者聽不見的情況
            Debug.LogError("Clip is Missing" + name);
    }

    
    public static AudioClip LoadClip(string dir, string name)
    {
        if (!audioDic.ContainsKey(name))
        {
            string dirMusic = dir + "/" + name;
            AudioClip clip = Resources.Load(dirMusic) as AudioClip;
            if (clip != null)
                audioDic.Add(name, clip);
            else
                Debug.LogError($"找不到{dir}/{name}:{clip}");
        }
        return audioDic[name];
    }

    //呼叫測試
    private void AudioSourceShow()
    {

        //在其他類裡面呼叫的時候只需要類名點這個靜態方法
        //如我目前的音樂檔案放在(Resources/Muisc)目錄下，檔名為OnClick，
        AudioSourceController.PlaySnd("Music", "OnClick");  //(此音效播放完會自動刪除)
    }

}