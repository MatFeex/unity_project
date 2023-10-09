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
    private List<ArmyElement> initialEnemyTurrets = null;

    public override bool AllTurretsAreDead() {
        return GetAllEnemiesOfType<Turret>(false).Count == 0;
    } 

    public override GameObject GetTurretTarget()
    {
        initialEnemyTurrets =
            (initialEnemyTurrets == null)
                ? GetAllEnemiesOfType<Turret>(false)
                : initialEnemyTurrets;

        var enemies = initialEnemyTurrets;
        int targetIndex = 0;


        if (!allTurretsShouldBeDead) // on tire d'abord sur les tourelles
        {
            targetIndex = (int)Math.Floor((double)cpt / 10);
            if (cpt > initialEnemyTurrets.Count * 10)
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
            // return GetRandomEnemyOfType<Drone>().gameObject;
            return null;
        }
    }

    public override GameObject GetDroneTarget()
    {
        return GetRandomEnemyOfType<Turret>()?.gameObject;
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