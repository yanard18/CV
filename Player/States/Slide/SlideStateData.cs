using Sirenix.OdinInspector;
using UnityEngine;

///<summary>
/// SlideStateData
///</summary>

[CreateAssetMenu(menuName = "Oblation/Player/Configs/Create SlideStateData", fileName = "SlideStateData", order = 0)]
public class SlideStateData : ScriptableObject
{
	[Min(0)]
	public float m_FrictionAcceleration = 10f;

	[Min(0)]
	public float m_CooldownDuration = 1f;

	public float m_YPercentageToEffect;
	public float m_XPercentageToEffect;

	public float m_MaxSpeed = 30f;
	public float m_ExtraSpeed;


}
