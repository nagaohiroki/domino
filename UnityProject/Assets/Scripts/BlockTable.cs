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
	List<Block> mBlocks = null;
	// カーソル
	[SerializeField]
	GameObject mCursor = null;
	// パラメータ
	BlockParam[,] mBlock;
	// ------------------------------------------------------------------------
	/// @brief セット
	///
	/// @param inIndex
	// ------------------------------------------------------------------------
	public void SetBlock(Vector2Int inIndex, int inType)
	{
		var block = GetBlock(inIndex);
		block.Set(GenerateBlock(inIndex, inType), inType);
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
		index.x = Mathf.FloorToInt(inPos.x / mScale) + mSize.x / 2;
		index.y = Mathf.FloorToInt(inPos.z / mScale) + mSize.y / 2;
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
		pos.x = (inIndex.x - mSize.x / 2) * mScale;
		pos.z = (inIndex.y - mSize.y / 2) * mScale;
		return pos;
	}
	// ------------------------------------------------------------------------
	/// @brief ブロック取得
	///
	/// @param inPos
	///
	/// @return
	// ------------------------------------------------------------------------
	public BlockParam GetBlock(Vector2Int inPos)
	{
		if(inPos.x < 0 || inPos.x >= mSize.x || inPos.y < 0 || inPos.y >= mSize.y)
		{
			return null;
		}
		return mBlock[inPos.y, inPos.x];
	}
	// ------------------------------------------------------------------------
	/// @brief ブロック生成
	///
	/// @param inPos
	/// @param inPrefab
	///
	/// @return
	// ------------------------------------------------------------------------
	Block GenerateBlock(Vector2Int inIndex, int inType)
	{
		if(mBlocks == null || inType < 0 || inType >= mBlocks.Count)
		{
			return null;
		}
		var obj = Instantiate(mBlocks[inType], IndexToPos(inIndex), Quaternion.identity);
		obj.transform.localScale = Vector3.one * mScale;
		return obj;
	}
	// ------------------------------------------------------------------------
	/// @brief 開始
	// ------------------------------------------------------------------------
	void Start()
	{
		mBlock = new BlockParam[mSize.y, mSize.x];
		for(int y = 0; y < mSize.y; ++y)
		{
			for(int x = 0; x < mSize.x; ++x)
			{
				mBlock[y, x] = new BlockParam();
				if(Random.Range(0, 2) == 0)
				{
					SetBlock(new Vector2Int(x, y), Random.Range(0, mBlocks.Count));
				}
			}
		}
	}
}
