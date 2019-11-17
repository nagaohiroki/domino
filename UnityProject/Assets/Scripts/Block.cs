using UnityEngine;
public class Block
{
	public enum Status
	{
		Empty,
		Put
	}
	public Status mStatus = Status.Empty;
	public Vector3 mPos;
}
public class BlockTable
{
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
}
