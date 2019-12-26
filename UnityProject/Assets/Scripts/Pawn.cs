using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Pawn : MonoBehaviour
{
	[SerializeField]
	float mSpeed = 0.0f;
	[SerializeField]
	int mHP = 0;
	[SerializeField]
	GameManager mGameManager = null;
	[SerializeField]
	Text mText = null;
	[SerializeField]
	ItemList mItemList = null;
	[SerializeField]
	int mCurrentType = 0;
	// ------------------------------------------------------------------------
	/// @brief ダメージ
	///
	/// @param inDamage
	///
	/// @return
	// ------------------------------------------------------------------------
	public void Damage(int inDamage)
	{
		mHP -= inDamage;
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
	// ------------------------------------------------------------------------
	/// @brief スロットの変更
	// ------------------------------------------------------------------------
	void ChangeSlot()
	{
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			mCurrentType = Mathf.Clamp(--mCurrentType, 0, mItemList.Count);
		}
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			mCurrentType = Mathf.Clamp(++mCurrentType, 0, mItemList.Count);
		}
	}
	// ------------------------------------------------------------------------
	/// @brief ショット
	// ------------------------------------------------------------------------
	void Shot(int inType)
	{
		var blockTable = mGameManager.mBlockTable;
		var index = blockTable.PosToIndex(GetCursorPos());
		blockTable.Cursor(index);
		// 追加
		if(Input.GetButton("Fire1"))
		{
			if(blockTable.ClearBlock(index))
			{
				mItemList.Add(inType, 1);
			}
		}
		// 取り除く
		if(Input.GetButton("Fire2"))
		{
			if(mItemList.HasItem(inType))
			{
				if(blockTable.SetBlock(index, inType))
				{
					mItemList.Sub(inType, 1);
				}
			}
		}
	}
	// ------------------------------------------------------------------------
	/// @brief テキスト
	// ------------------------------------------------------------------------
	void Text()
	{
		mText.text = string.Empty;
		mText.text += string.Format("HP : {0}\n", mHP);
		mText.text += mItemList.ToString(mCurrentType);
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
