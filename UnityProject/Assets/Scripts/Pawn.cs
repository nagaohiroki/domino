using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class Pawn : MonoBehaviour
{
	[SerializeField]
	Text mText = null;
	[SerializeField]
	NavMeshAgent mAgent = null;
	[SerializeField]
	float mSpeed = 0.0f;
	[SerializeField]
	GameObject mCube = null;
	const float mScale = 1.0f;
	BlockTable mBlockTable = new BlockTable(new Vector2Int(10, 10));
	static Vector2Int GetIndex(Vector3 inPos)
	{
		var index = Vector2Int.zero;
		index.x = Mathf.FloorToInt(inPos.x / mScale);
		index.y = Mathf.FloorToInt(inPos.z / mScale);
		return index;
	}
	static Vector3 GetPos(Vector2Int inIndex)
	{
		var pos = Vector3.zero;
		pos.x = inIndex.x * mScale;
		pos.z = inIndex.y * mScale;
		return pos;
	}
	void Move()
	{
		var offset = Vector3.zero;
		offset.x = Input.GetAxis("Horizontal");
		offset.z = Input.GetAxis("Vertical");
		mAgent.Move(offset * mSpeed * Time.deltaTime);
		Camera.main.transform.parent.position = transform.position;
	}
	Vector3 GetCursorPos()
	{
		// カーソル位置
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
		{
			return hit.point;
		}
		return Vector3.zero;
	}
	void Shot()
	{
		var index = GetIndex(GetCursorPos());
		var block = mBlockTable.GetBlock(index);
		if(block == null)
		{
			return;
		}
		if(block.mStatus != Block.Status.Empty)
		{
			return;
		}
		var offset = new Vector3(mScale, 0.0f, mScale) * 0.5f;
		var pos = GetPos(index) + offset;
		mCube.transform.position = pos;
		if(Input.GetButtonDown("Fire1"))
		{
			block.mStatus = Block.Status.Put;
			var obj = Instantiate(mCube, pos, Quaternion.identity);
			obj.SetActive(true);
			obj.GetComponent<Collider>().isTrigger = false;
			obj.transform.localScale = Vector3.one * mScale;
			var render = obj.GetComponent<Renderer>();
			render.material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
			obj.GetComponent<NavMeshObstacle>().enabled = true;
		}
	}
	void Update()
	{
		Move();
		Shot();
		DebugLog();
	}
	void DebugLog()
	{
		var index = GetIndex(transform.position);
		var cursor = GetIndex(GetCursorPos());
		mText.text = string.Format("index: {0}, cursor: {1}", index, cursor);
	}
}
