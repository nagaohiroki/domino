using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
public class BlockTable : MonoBehaviour
{
	delegate void ForechDelegate(Vector2Int inPos, Block inBlock);
	[SerializeField]
	Vector2Int mSize = Vector2Int.zero;
	[SerializeField]
	float mScale = 0.0f;
	[SerializeField]
	List<GameObject> mCubes = null;
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
		block.Set(GenerateBlock(IndexToPos(inIndex), GetCube(inType)));
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
	public void Cursor(Vector2Int inIndex, int inType)
	{
		var cube = GetCube(inType);
		if(cube == null)
		{
			return;
		}
		cube.transform.position = IndexToPos(inIndex);
		cube.transform.localScale = Vector3.one * mScale;
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
	Block GetBlock(Vector2Int inPos)
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
	/// @brief 周囲
	///
	/// @param inDelegate
	///
	/// @return
	// ------------------------------------------------------------------------
	void ForechBlock(ForechDelegate inDelegate)
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
	// ------------------------------------------------------------------------
	/// @brief ブロック生成
	///
	/// @param inPos
	/// @param inPrefab
	///
	/// @return
	// ------------------------------------------------------------------------
	GameObject GenerateBlock(Vector3 inPos, GameObject inPrefab)
	{
		var obj = Instantiate(inPrefab, inPos, Quaternion.identity);
		obj.SetActive(true);
		obj.transform.localScale = Vector3.one * mScale;
		var child = obj.transform.GetChild(0);
		child.GetComponent<NavMeshObstacle>().enabled = true;
		var render = child.GetComponent<Renderer>();
		render.material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
		return obj;
	}
	// ------------------------------------------------------------------------
	/// @brief
	// ------------------------------------------------------------------------
	void Start()
	{
		var size = mSize * 2;
		mBlock = new Block[size.y, size.x];
		for(int y = 0; y < mBlock.GetLength(0); ++y)
		{
			for(int x = 0; x < mBlock.GetLength(1); ++x)
			{
				mBlock[y, x] = new Block();
			}
		}
		// ブロック配置
		ForechBlock((inPos, inBlock) =>
		{
			if(Random.Range(0, 100) == 0)
			{
				SetBlock(inPos, 0);
			}
		});
	}
}
