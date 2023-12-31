using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyTasks")]
[TaskDescription("Select a turret target, cycling through all enemy turrets and shooting 10 times on each")]

public class SelectTurretTarget : Action
{
	IArmyElement m_ArmyElement;
	public SharedTransform target;

	public override void OnAwake()
	{
		m_ArmyElement =(IArmyElement) GetComponent(typeof(IArmyElement));
	}

	public override TaskStatus OnUpdate()
	{
		if (m_ArmyElement.ArmyManager == null) return TaskStatus.Running; // la r�f�rence � l'arm�e n'a pas encore �t� inject�e

		target.Value = m_ArmyElement.ArmyManager.GetTurretTarget()?.transform;
		// Debug.Log($"This = {this.transform}");

		if (target.Value != null) return TaskStatus.Success;
		else return TaskStatus.Failure;

	}
}