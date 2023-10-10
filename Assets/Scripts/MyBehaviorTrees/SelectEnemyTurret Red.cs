using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyTasks")]
[TaskDescription("Select non targeted enemy turret")]

public class SelectEnemyTurretRed : Action
{
	IArmyElement m_ArmyElement;
	ArmyManagerRed m_ArmyManagerRed;
	public SharedTransform target;
	public SharedFloat minRadius;
	public SharedFloat maxRadius;

	public override void OnAwake()
	{
		m_ArmyElement =(IArmyElement) GetComponent(typeof(IArmyElement));
		m_ArmyManagerRed = (ArmyManagerRed) GetComponent(typeof(ArmyManagerRed));
	}

	public override TaskStatus OnUpdate()
	{
		if (m_ArmyElement.ArmyManager == null) return TaskStatus.Running; // la r�f�rence � l'arm�e n'a pas encore �t� inject�e

		target.Value = m_ArmyElement.ArmyManager.Test()?.transform;	
		Debug.Log(m_ArmyElement.ArmyManager.Test()?.transform, this.gameObject);

		if (target.Value != null) return TaskStatus.Success;
		else return TaskStatus.Failure;

	}
}