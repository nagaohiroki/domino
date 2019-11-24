using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class Pawn : MonoBehaviour
{
	[SerializeField]
	Text mText = null;
	[SerializeField]
	float mSpeed = 0.0f;
	[SerializeField]
	GameObject mCube = null;
	[SerializeField]
	int mBlockCount = 0;
	const float mScale = 2.0f;
	BlockTable mBlockTable = new BlockTable(new Vector2Int(40, 40));
	// ------------------------------------------------------------------------
	/// @brief 位置からIndex取得
	///
	/// @param inPos
	///
	/// @return
	// ------------------------------------------------------------------------
	static Vector2Int PosToIndex(Vector3 inPos)
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
	static Vector3 IndexToPos(Vector2Int inIndex)
	{
		var pos = Vector3.zero;
		pos.x = inIndex.x * mScale;
		pos.z = inIndex.y * mScale;
		return pos;
	}
	// ------------------------------------------------------------------------
	/// @brief 移動
	// ------------------------------------------------------------------------
	void Move()
	{
		var vel = Vector3.zero;
		vel.x = Input.GetAxis("Horizontal");
		vel.z = Input.GetAxis("Vertical");
		var agent = GetComponent<NavMeshAgent>();
		if(agent != null && agent.enabled)
		{
			agent.Move(vel * mSpeed * Time.deltaTime);
		}
		Camera.main.transform.parent.position = transform.position;
	}
	// ------------------------------------------------------------------------
	/// @brief カーソル位置
	///
	/// @return
	// ------------------------------------------------------------------------
	Vector3 GetCursorPos()
	{
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground"), QueryTriggerInteraction.Ignore))
		{
			return hit.point;
		}
		return Vector3.zero;
	}
	Vector3 BoxPos(Vector2Int inIndex)
	{
		return IndexToPos(inIndex) + (Vector3.one - Vector3.up) * mScale * 0.5f;
	}
	// ------------------------------------------------------------------------
	/// @brief ショット
	// ------------------------------------------------------------------------
	void Shot()
	{
		// カーソル更新
		var index = PosToIndex(GetCursorPos());
		mCube.transform.position = BoxPos(index);
		mCube.transform.localScale = Vector3.one * mScale;
		// ブロック
		var block = mBlockTable.GetBlock(index);
		if(block == null)
		{
			return;
		}
		if(!Input.GetButtonDown("Fire1"))
		{
			return;
		}
		if(block.mStatus == Block.Status.Empty)
		{
			if(mBlockCount <= 0)
			{
				return;
			}
			SetBlock(index);
			--mBlockCount;
		}
		else if(block.mStatus == Block.Status.Put)
		{
			block.Clear();
			++mBlockCount;
		}
	}
	void SetBlock(Vector2Int inIndex)
	{
		var block = mBlockTable.GetBlock(inIndex);
		block.Set(GenerateBlock(BoxPos(inIndex), mCube));
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
		obj.GetComponent<Collider>().isTrigger = false;
		obj.transform.localScale = Vector3.one * mScale;
		var render = obj.GetComponent<Renderer>();
		render.material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
		obj.GetComponent<NavMeshObstacle>().enabled = true;
		return obj;
	}
	// ------------------------------------------------------------------------
	/// @brief ログ
	// ------------------------------------------------------------------------
	string Log()
	{
		var index = PosToIndex(transform.position);
		var cursor = PosToIndex(GetCursorPos());
		return string.Format("index: {0}, cursor: {1}\n{2}", index, cursor, mBlockCount);
	}
	// ------------------------------------------------------------------------
	/// @brief 開始
	// ------------------------------------------------------------------------
	void Start()
	{
		mBlockTable.ForechBlock((inPos, inBlock) =>
		{
			if(Random.Range(0, 100) == 0)
			{
				SetBlock(inPos);
			}
		});
	}
	// ------------------------------------------------------------------------
	/// @brief 更新
	// ------------------------------------------------------------------------
	void Update()
	{
		Move();
		Shot();
		mText.text = Log();
	}
}
