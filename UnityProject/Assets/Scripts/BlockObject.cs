using UnityEngine;
public class Block
{
	GameObject mGameObject;
	public bool mUsed{get; private set;}
	public int mType{get;private set;}
	public void Clear()
	{
		mUsed = false;
		mType = -1;
		GameObject.Destroy(mGameObject);
	}
	public void Set(GameObject inGameObject, int inType)
	{
		mUsed = true;
		mType = inType;
		mGameObject = inGameObject;
	}
}
