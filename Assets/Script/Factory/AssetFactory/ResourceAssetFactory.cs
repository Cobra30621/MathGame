using UnityEngine;
using System.Collections;

// 從專案的Resource中,將Unity Asset實體化成GameObject的工廠類別
public class ResourceAssetFactory : IAssetFactory 
{
	public const string SoldierPath = "Characters/Soldier/";
	public const string EnemyPath = "Characters/Enemy/";
	public const string WeaponPath = "Weapons/";
	public const string EffectPath = "Effects/";
	public const string AudioPath  = "Sound/";
	public const string SpritePath = "Sprites/";
	
	public const string BallPath =  "Prefabs/";
	public const string BallImagePath = "BallImage/";
	public const string ImagePath = "Image/";
	public const string StageInfoCardPath = "Prefabs/StageInfoCard";

	// 讀取球物件
	public override GameObject LoadBallPrefab( string AssetName ){
		string BallStyle = GetBallStyle();
		return InstantiateGameObject( BallStyle + BallPath + AssetName);
	}

	// 讀取照片
	public override Sprite LoadBallImage(string SpriteName){
		string BallStyle = GetBallStyle();
		return Resources.Load(  BallStyle + BallImagePath + SpriteName,typeof(Sprite)) as Sprite;
	}

	//讀取Bar的照片
	public override Sprite LoadImageSprite(string SpriteName){
		string BallStyle = GetBallStyle();
		return Resources.Load(  BallStyle + ImagePath + SpriteName,typeof(Sprite)) as Sprite;
	}

	public GameObject LoadStageInfoCard(){
		return Resources.Load(  StageInfoCardPath,typeof(GameObject)) as GameObject;
	}

	// 取的目前的造型
	public string GetBallStyle(){
		string path= "";
		switch (GameMeditor.Instance.GetBallStyle()){
			case BallStyle.tomato:
				path = "Tomato/";
				break;
			case BallStyle.snowBall:
				path = "SnowBall/";
				break;
			default:
				Debug.LogError("傳入錯誤造型");
				break;
		}

		return path;
	}


	// 產生特效
	public override GameObject LoadEffect( string AssetName )
	{
		return InstantiateGameObject( EffectPath + AssetName);
	}

	// 產生AudioClip
	public override AudioClip  LoadAudioClip(string ClipName)
	{
		UnityEngine.Object res = LoadGameObjectFromResourcePath(AudioPath + ClipName );
		if(res==null)
			return null;
		return res as AudioClip;
	}

	// 產生Sprite
	public override Sprite LoadSprite(string SpriteName)
	{
		return Resources.Load(SpritePath + SpriteName,typeof(Sprite)) as Sprite;
	}

	// 產生GameObject
	private GameObject InstantiateGameObject( string AssetName )
	{
		// 從Resrouce中載入
		UnityEngine.Object res = LoadGameObjectFromResourcePath( AssetName );
		if(res==null)
			return null;
		return  UnityEngine.Object.Instantiate(res) as GameObject;
	}

	// 從Resrouce中載入
	public UnityEngine.Object LoadGameObjectFromResourcePath( string AssetPath)
	{
		UnityEngine.Object res = Resources.Load(AssetPath);
		if( res == null)
		{
			Debug.LogWarning("無法載入路徑["+AssetPath+"]上的Asset");
			return null;
		}		
		return res;
	}
}
