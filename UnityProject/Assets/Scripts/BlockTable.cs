using UnityEngine;
using System.Collections.Generic;
public class BlockTable : MonoBehaviour
{
	[SerializeField]
	Vector2Int mSize = Vector2Int.zero;
	[SerializeField]
	float mScale = 0.0f;
	// ブロック
	[SerializeField]
	List<GameObject> mCubes = null;
	// カーソル
	[SerializeField]
	GameObject mCursor = null;
	Block[,] mBlock;
	// ------------------------------------------------------------------------
	/// @brief
	///
	/// @param inIndex
	///
	/// @return
	// ------------------------------------------------------------------------
	GameObject GetCube(int inIndex)
	{
		if(mCubes == null || inIndex < 0 || inIndex >= mCubes.Count)
		{
			return null;
		}
		return mCubes[inIndex];
	}
	// ------------------------------------------------------------------------
	/// @brief セット
	///
	/// @param inIndex
	// ------------------------------------------------------------------------
	public bool SetBlock(Vector2Int inIndex, int inType)
	{
		var block = GetBlock(inIndex);
		if(block == null || block.mUsed)
		{
			return false;
		}
		block.Set(GenerateBlock(IndexToPos(inIndex), inType), inType);
		return true;
	}
	// ------------------------------------------------------------------------
	/// @brief 削除
	///
	/// @param inIndex
	///
	/// @return
	// ------------------------------------------------------------------------
	public bool ClearBlock(Vector2Int inIndex)
	{
		var block = GetBlock(inIndex);
		if(block == null || !block.mUsed)
		{
			return false;
		}
		block.Clear();
		return true;
	}
	// ------------------------------------------------------------------------
	/// @brief カーソル位置
	///
	/// @param inIndex
	///
	/// @return
	// ------------------------------------------------------------------------
	public void Cursor(Vector2Int inIndex)
	{
		mCursor.transform.localScale = Vector3.one * mScale;
		mCursor.transform.position = IndexToPos(inIndex);
	}
	// ------------------------------------------------------------------------
	/// @brief 位置からIndex取得
	///
	/// @param inPos
	///
	/// @return
	// ------------------------------------------------------------------------
	public Vector2Int PosToIndex(Vector3 inPos)
	{
		var index = Vector2Int.zero;
		index.x = Mathf.FloorToInt(inPos.x / mScale);
		index.y = Mathf.FloorToInt(inPos.z / mScale);
		return index;
	}
	// ------------------------------------------------------------------------
	/// @brief Indexから位置
	///
	/// @param inIndex
	///
	/// @return
	// ------------------------------------------------------------------------
	Vector3 IndexToPos(Vector2Int inIndex)
	{
		var pos = Vector3.zero;
		pos.x = inIndex.x * mScale;
		pos.z = inIndex.y * mScale;
		return pos;
	}
	// ------------------------------------------------------------------------
	/// @brief ブロック取得
	///
	/// @param inPos
	///
	/// @return
	// ------------------------------------------------------------------------
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
	// ------------------------------------------------------------------------
	/// @brief ブロック生成
	///
	/// @param inPos
	/// @param inPrefab
	///
	/// @return
	// ------------------------------------------------------------------------
	GameObject GenerateBlock(Vector3 inPos, int inType)
	{
		var obj = Instantiate(GetCube(inType), inPos, Quaternion.identity);
		obj.transform.localScale = Vector3.one * mScale;
		return obj;
	}
	// ------------------------------------------------------------------------
	/// @brief
	// ------------------------------------------------------------------------
	void Start()
	{
		var size = mSize * 2;
		mBlock = new Block[size.y, size.x];
		for(int y = 0; y < size.y; ++y)
		{
			for(int x = 0; x < size.x; ++x)
			{
				mBlock[y, x] = new Block();
			}
		}
		for(int y = 0; y < size.y; ++y)
		{
			for(int x = 0; x < size.x; ++x)
			{
				if(Random.Range(0, 2) == 0)
				{
					SetBlock(new Vector2Int(x - mSize.x, y - mSize.y), Random.Range(0, mCubes.Count));
				}
			}
		}
	}
}
