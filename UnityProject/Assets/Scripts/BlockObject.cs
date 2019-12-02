using UnityEngine;
public class Block
{
	GameObject mGameObject;
	public bool mUsed{get; private set;}
	public void Clear()
	{
		mUsed = false;
		GameObject.Destroy(mGameObject);
	}
	public void Set(GameObject inGameObject)
	{
		mUsed = true;
		mGameObject = inGameObject;
	}
}
