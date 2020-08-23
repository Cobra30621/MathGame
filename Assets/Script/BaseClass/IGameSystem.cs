using UnityEngine;
using System.Collections;

// 遊戲子系統共用界面
public abstract class IGameSystem
{
	protected GameMeditor meditor = null;
	public IGameSystem( GameMeditor meditor )
	{
		this.meditor = meditor;
	}

	public virtual void Initialize(){}
	public virtual void Release(){}
	public virtual void Update(){}

}
