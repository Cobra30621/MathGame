using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectDebug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterStage(){
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("MainGame");
    }

        // 等讀取完場景，再執行
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameMeditor.Instance.SetGameProcess(GameProcess.Start); // 遊戲流程變成開始
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void AddMoney(){
        GameMeditor.Instance.AddMoney(10000);
        StageSelectUI.RefreshInfo(); // 更新金錢介面
    }

}
