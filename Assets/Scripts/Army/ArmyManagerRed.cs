using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.AI;

public class ArmyManagerRed : ArmyManager
{

    public override GameObject Test()
    {
        var enemies = GetAllEnemies(false);
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
        int nDrones = 0, nTurrets = 0, health = 0;
        ComputeStatistics(ref nDrones, ref nTurrets, ref health);
        GUIUtility.systemCopyBuffer = "1\t" + ((int)Timer.Value).ToString() + "\t" + nDrones.ToString() + "\t" + nTurrets.ToString() + "\t" + health.ToString();

        RefreshHudDisplay(); // pour une dernière mise à jour en cas de victoire
    }

    // Surcharge de la méthode GetAllEnemies pour cibler en premier lieu les tourelles vertes
    public List<ArmyElement> GetAllEnemies(bool sortRandom)
    {
        var greenTurrets = GetAllEnemiesOfType<Turret>(false); // get green turret first

        // Ensuite, obtenez toutes les autres unités ennemies
        var enemies = GameObject.FindObjectsOfType<ArmyElement>().Where(element =>
            !element.gameObject.CompareTag("2") && element != null && element.isActiveAndEnabled).ToList();

        // Combinez les deux listes en mettant d'abord les tourelles vertes
        var combinedList = greenTurrets.Concat(enemies).ToList();

        if (sortRandom) combinedList.Sort((a, b) => Random.value.CompareTo(.5f));
        return combinedList;
    }

    // Méthode pour disperser les drones rouges
    // public void DisperseRedDrones()
    // {
    //     var redDrones = GetAllAlliesOfType<Drone>(false);

    //     foreach (var drone in redDrones)
    //     {
    //         // Changez ici le comportement pour garder le drone en mouvement ou rétablissez son comportement initial
    //     }
    // }

    // Vous pouvez ajouter d'autres méthodes personnalisées pour votre stratégie ici
}
