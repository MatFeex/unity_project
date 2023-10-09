using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Linq;
using UnityEngine.AI;

public class ArmyManagerRed : ArmyManager
{
    private int cpt = 0;
    private bool allTurretsShouldBeDead = false;
    private int initialTurretNb = 0;

    private List<ArmyElement> initialEnemyTurrets = null;

    public override GameObject GetTurretTarget()
    {
        initialEnemyTurrets =
            (initialEnemyTurrets == null)
                ? GetAllEnemiesOfType<Turret>(false)
                : initialEnemyTurrets;

        var enemies = initialEnemyTurrets;
        initialTurretNb = (initialTurretNb == 0) ? enemies.Count : initialTurretNb;
        int targetIndex = 0;

        if (!allTurretsShouldBeDead) // on tire d'abord sur les tourelles
        {
            targetIndex = (int)Math.Floor((double)cpt / 10);
            if (cpt > initialTurretNb * 10)
            {
                Debug.Log("All turrets should be dead");
                allTurretsShouldBeDead = true;
            }
            cpt++;
            try
            {
                return enemies[targetIndex].gameObject;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        else // une fois qu'elles sont censées etre toutes dead, on tire sur les drones
        {
            return GetRandomEnemyOfType<Drone>().gameObject;
        }
    }

    public override GameObject GetClosestEnemyInRadius(Vector3 centerPos, float radius)
    {
        List<ArmyElement> enemies = GetAllEnemies(false)
            .Where(item => Vector3.Distance(centerPos, item.transform.position) < radius)
            .ToList();

        if (enemies.Count == 0)
        {
            return null;
        }

        return enemies.FirstOrDefault().gameObject;

        // enemies.Sort(
        //     (a, b) =>
        //         Vector3
        //             .Distance(centerPos, a.transform.position)
        //             .CompareTo(Vector3.Distance(centerPos, b.transform.position))
        // );

        // double minDistance = 10000;
        // ArmyElement target = enemies.FirstOrDefault();
        // foreach (ArmyElement e in enemies)
        // {
        // 	float currentDist = Vector3.Distance(centerPos, e.transform.position);
        //     if (currentDist < minDistance) {
        // 		minDistance = currentDist;
        // 		target = e;
        // 	}
        // }

        // return target.gameObject;
    }

    public override GameObject GetDroneTarget(Vector3 centerPos)
    {
        var enemies = GetAllEnemiesOfType<Drone>(false);
        float minDistance = 1000;
        ArmyElement target = enemies.FirstOrDefault();
        foreach (ArmyElement e in enemies)
        {
            float currentDist = Vector3.Distance(centerPos, e.transform.position);
            if (currentDist < minDistance)
            {
                minDistance = currentDist;
                target = e;
            }
        }

        return target.gameObject;
    }

    public override GameObject GetTargetOfType<T>() where T: ArmyElement {
        var enemies = GetAllEnemiesOfType<T>();
        return enemies.FirstOrDefault().gameObject;
    }

    public override GameObject GetTurretToProtect(ArmyElement caller)
    {
        var AlliesTurret = GetAllAllies(true, caller)
            .Where(element => (element is Turret))
            .ToList();

        if (AlliesTurret.Count > 0)
        {
			var enemies = GetAllEnemiesOfTypeByDistance<Drone>(false, AlliesTurret.FirstOrDefault().gameObject.transform.position, 0, 10);
			return enemies.Count > 0 ? enemies.FirstOrDefault()?.gameObject : null;
        }
        return null;
    }

    public override void ArmyElementHasBeenKilled(GameObject go)
    {
        base.ArmyElementHasBeenKilled(go);
        if (m_ArmyElements.Count == 0)
        {
            GUIUtility.systemCopyBuffer = "0\t" + ((int)Timer.Value).ToString() + "\t0\t0\t0";
        }
    }

    public void GreenArmyIsDead(string deadArmyTag)
    {
        int nDrones = 0,
            nTurrets = 0,
            health = 0;
        ComputeStatistics(ref nDrones, ref nTurrets, ref health);
        GUIUtility.systemCopyBuffer =
            "1\t"
            + ((int)Timer.Value).ToString()
            + "\t"
            + nDrones.ToString()
            + "\t"
            + nTurrets.ToString()
            + "\t"
            + health.ToString();

        RefreshHudDisplay(); //pour une derni�re mise � jour en cas de victoire
    }
}
