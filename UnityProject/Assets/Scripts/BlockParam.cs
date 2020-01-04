using UnityEngine;
public class BlockParam
{
	Block mBlock;
	int mHP = 0;
	int mBlockType = -1;
	public int BlockType{get{return mBlockType;}}
	public bool IsEmpty{get{return mBlockType == -1;}}
	public void Set(Block inBlock, int inType)
	{
		mBlockType = inType;
		mBlock = inBlock;
		mHP = inBlock.mHP;
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
	void Clear()
	{
		mBlockType = -1;
		GameObject.Destroy(mBlock.gameObject);
	}
}
