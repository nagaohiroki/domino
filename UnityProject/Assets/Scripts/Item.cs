using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class Item
{
	public int mKey = 0;
	public int mCount = 0;
	public Item(int inKey, int inCount)
	{
		mKey = inKey;
		mCount = inCount;
	}
}
[System.Serializable]
public class ItemList
{
	[SerializeField]
	List<Item> mItemList = null;
	public void Add(int inKey, int inCount)
	{
		var item = GetItem(inKey);
		if(item == null)
		{
			mItemList.Add(new Item(inKey, inCount));
			return;
		}
		item.mCount += inCount;
	}
	public void Sub(int inKey, int inCount)
	{
		var item = GetItem(inKey);
		if(item == null)
		{
			return;
		}
		item.mCount = Mathf.Max(0, item.mCount - inCount);
	}

	public bool HasItem(int inKey)
	{
		var item = GetItem(inKey);
		if(item == null)
		{
			return false;
		}
		return item.mCount > 0;
	}
	Item GetItem(int inKey)
	{
		foreach(var item in mItemList)
		{
			if(item.mKey == inKey)
			{
				return item;
			}
		}
		return null;
	}
}
