using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.AI;

public class ArmyManagerRed : ArmyManager
{
    public int toto = 0;

    public override GameObject GetFirstDrone()
    {
        var enemies = GetAllEnemiesOfType<Drone>(false);
        return enemies.FirstOrDefault()?.gameObject;
    }

    public override GameObject GetFirstTurret()
    {
        var enemies = GetAllEnemiesOfType<Turret>(false);
        return enemies.FirstOrDefault()?.gameObject;
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
