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

    public override bool AllTurretsAreDead()
    // pour savoir si toutes les tourelles ennemies sont mortes
    {
        return GetAllEnemiesOfType<Turret>(false).Count == 0;
    }

    public override GameObject GetTurretTarget()
    // donne la meme cible à toutes les tourelles et tire 10 coup dessus
    {
        initialEnemyTurrets =
            (initialEnemyTurrets == null)
                ? GetAllEnemiesOfType<Turret>(false)
                : initialEnemyTurrets;

        var enemies = initialEnemyTurrets;
        int targetIndex = 0;

        if (!allTurretsShouldBeDead)
        {
            targetIndex = (int)Math.Floor((double)cpt / 10); // on récupère un indice qui prend 1 tout les 10
            if (cpt > initialEnemyTurrets.Count * 10) // si on a tiré 10 fois sur cahque tourelles, on estime qu'on a fini de tirer sur les tourelles
            {
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
        return null;
    }

    public override GameObject GetDroneTarget()
    // donne une cible randomde type turret aux drones
    {
        return GetRandomEnemyOfType<Turret>()?.gameObject;
    }

    public override GameObject GetEnemyInRadius(Vector3 centerPos, float radius)
    // retourne un ennemi dans le raius donné s'il existe, null sinon
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
