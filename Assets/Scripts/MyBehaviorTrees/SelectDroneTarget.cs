using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyTasks")]
[TaskDescription("Select a random turret to attack")]

public class SelectDroneTarget : Action
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

		if (target.Value != null) return TaskStatus.Success;

		target.Value = m_ArmyElement.ArmyManager.GetDroneTarget()?.transform;

		if (target.Value != null) return TaskStatus.Success;
		else return TaskStatus.Failure;

	}
}