using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
	[SerializeField]
	GameManager mGameManager = null;
	[SerializeField]
	NavMeshAgent mAgent = null;
	void Update()
	{
		mAgent.SetDestination(mGameManager.mPawn.transform.position);
	}
	// ------------------------------------------------------------------------
	/// @brief ヒット
	///
	/// @param inColl
	// ------------------------------------------------------------------------
	void OnCollisionEnter(Collision inColl)
	{
		var pawn = inColl.gameObject.GetComponent<Pawn>();
		if(pawn == null)
		{
			return;
		}
		pawn.Damage(1);
		Destroy(gameObject);
	}
}
