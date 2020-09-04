using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MainFactory
{
    private static BallFactory m_BallFactory = null;
	private static ResourceAssetFactory m_resourceAssetFactory = null;

   	// 取得將Unity Asset實作化的工廠
	public static BallFactory GetBallFactory()
	{
		if( m_BallFactory == null)
		{
			m_BallFactory = new BallFactory();
		}
		return m_BallFactory;
	}

	// 取得將Unity Asset實作化的工廠
	public static ResourceAssetFactory GetResourceAssetFactory()
	{
		if( m_resourceAssetFactory == null)
		{
			m_resourceAssetFactory = new ResourceAssetFactory();
		}
		return m_resourceAssetFactory;
	}
}