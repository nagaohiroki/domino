using UnityEngine;
public class Block
{
	public enum Status
	{
		Empty,
		Put
	}
	GameObject mGameObject;
	public Status mStatus{get; private set;} = Status.Empty;
	public void Clear()
	{
		mStatus = Status.Empty;
		GameObject.Destroy(mGameObject);
	}
	public void Set(GameObject inGameObject)
	{
		mStatus = Status.Put;
		mGameObject = inGameObject;
	}
}
public class BlockTable
{
	public delegate void ForechDelegate(Vector2Int inPos, Block inBlock);
	Block[,] mBlock;
	public BlockTable(Vector2Int inSize)
	{
		var size = inSize * 2;
		mBlock = new Block[size.y, size.x];
		for(int y = 0; y < mBlock.GetLength(0); ++y)
		{
			for(int x = 0; x < mBlock.GetLength(1); ++x)
			{
				mBlock[y, x] = new Block();
			}
		}
	}
	public Block GetBlock(Vector2Int inPos)
	{
		int numX = mBlock.GetLength(1);
		int numY = mBlock.GetLength(0);
		int x =  numX / 2 + inPos.x;
		int y =  numY / 2 + inPos.y;
		if(x < 0 || x >= numX || y < 0 || y >= numY)
		{
			return null;
		}
		return mBlock[y, x];
	}
	public void ForechBlock(ForechDelegate inDelegate)
	{
		var half = new Vector2Int(mBlock.GetLength(1) / 2, mBlock.GetLength(0) / 2);
		for(int y = 0; y < mBlock.GetLength(0); ++y)
		{
			for(int x = 0; x < mBlock.GetLength(1); ++x)
			{
				inDelegate(new Vector2Int(x, y) - half, mBlock[y, x]);
			}
		}
	}
}
