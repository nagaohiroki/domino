using UnityEngine;
public class BlockParam
{
	GameObject mGameObject;
	public bool mUsed{get; private set;}
	public int mType{get; private set;}
	public int mHP = 0;
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
	public void Damage(int inDamage)
	{
		mHP += inDamage;
		if(mHP <= 0)
		{
			Clear();
			return;
		}
	}
}
