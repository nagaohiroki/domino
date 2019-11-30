using UnityEngine;
using UnityEngine.AI;
public class Pawn : MonoBehaviour
{
	[SerializeField]
	float mSpeed = 0.0f;
	[SerializeField]
	int mBlockCount = 0;
	[SerializeField]
	BlockTable mBlockTable = null;
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
	// ------------------------------------------------------------------------
	/// @brief ショット
	// ------------------------------------------------------------------------
	void Shot()
	{
		// カーソル更新
		var index = mBlockTable.PosToIndex(GetCursorPos());
		mBlockTable.Cursor(index);
		if(Input.GetButton("Fire1"))
		{
			if(mBlockTable.ClearBlock(index))
			{
				++mBlockCount;
			}
		}
		if(Input.GetButton("Fire2"))
		{
			if(mBlockCount > 0)
			{
				if(mBlockTable.SetBlock(index))
				{
					--mBlockCount;
				}
			}
		}
	}
	// ------------------------------------------------------------------------
	/// @brief 更新
	// ------------------------------------------------------------------------
	void Update()
	{
		Move();
		Shot();
	}
}
