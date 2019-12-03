using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Pawn : MonoBehaviour
{
	[SerializeField]
	float mSpeed = 0.0f;
	[SerializeField]
	int mBlockCount = 0;
	[SerializeField]
	int mHP = 0;
	[SerializeField]
	BlockTable mBlockTable = null;
	[SerializeField]
	Text mText = null;
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
	void OnCollisionEnter(Collision inColl)
	{
		var enemy = inColl.gameObject.GetComponent<Enemy>();
		if (enemy == null)
		{
			return;
		}
		--mHP;
		Debug.Log(inColl.gameObject.name);
	}
	// ------------------------------------------------------------------------
	/// @brief テキスト
	// ------------------------------------------------------------------------
	void Text()
	{
		var text = string.Format("HP : {0}\nItem : {1}\n", mHP, mBlockCount);
		mText.text = text;
	}
	// ------------------------------------------------------------------------
	/// @brief 更新
	// ------------------------------------------------------------------------
	void Update()
	{
		Move();
		Shot();
		Text();
	}
}
