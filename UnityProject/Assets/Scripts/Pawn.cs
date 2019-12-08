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
	[SerializeField]
	ItemList mItemList = null;
	int mCurrentType = 0;
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
	void Shot(int inType)
	{
		// カーソル更新
		var index = mBlockTable.PosToIndex(GetCursorPos());
		mBlockTable.Cursor(index, inType);
		if(Input.GetButton("Fire1"))
		{
			if(mBlockTable.ClearBlock(index))
			{
				mItemList.Add(inType, 1);
			}
		}
		if(Input.GetButton("Fire2"))
		{
			if(mItemList.HasItem(inType))
			{
				if(mBlockTable.SetBlock(index, inType))
				{
					mItemList.Sub(inType, 1);
				}
			}
		}
	}
	void OnCollisionEnter(Collision inColl)
	{
		var enemy = inColl.gameObject.GetComponent<Enemy>();
		if(enemy == null)
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
		Shot(mCurrentType);
		Text();
	}
}
