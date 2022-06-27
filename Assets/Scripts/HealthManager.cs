using UnityEngine;

public class HealthManager : MonoBehaviour
{
	[SerializeField]
	private DinosaurCtrl rapter;
	public float hitPoint = 100f;

	public void ApplyDamage(float damage)
	{
		hitPoint -= damage;
		rapter.rapterAnimation.CrossFade(rapter.DamageAnimClip.name);

		rapter.effectDamageTween();

		if (hitPoint <= 0)
		{
			rapter.rapterState = DinosaurCtrl.RapterState.Die;
		}
	}
}