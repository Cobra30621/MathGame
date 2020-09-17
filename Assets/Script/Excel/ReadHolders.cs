using UnityEngine;

/// <summary>
/// 讀取booknames的scriptObject檔案
/// 使用Resources直接讀取
/// </summary>
public class ReadHolders : MonoBehaviour {
    readonly string assetName = "booknames";

	void Start ()
    {
        BookHolder asset = Resources.Load<BookHolder>(assetName);
        foreach (StageInfo gd in asset.menus)
        {
            Debug.Log(gd._primes);
            Debug.Log(gd._composites);
            Debug.Log(gd._probability);
        }
    }
}