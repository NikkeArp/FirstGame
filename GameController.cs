using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using WizardAdventure.Structures;

public class GameController : MonoBehaviour
{
#region [Properties]

    public static GameController Instance { get; private set; } = null;
    public readonly int PlayerShootDownTime = 100;
    public bool PlayerCanShoot { get; private set; } = true;
    private List<SlimeMergePair> slimeMergeEventPairs;

#endregion

    void Awake()
    {
        slimeMergeEventPairs = new List<SlimeMergePair>();
        if (Instance is null) 
            Instance = this;
        else if (Instance != this) 
            Destroy(gameObject);
    }


    public bool AddSlimePair(SlimeMergePair pair)
    {
        for (int i = 0; i < slimeMergeEventPairs.Count; i++)
        {
            var item = slimeMergeEventPairs[i];
            if (item.slime1 == pair.slime2)
            {
                Destroy(item.slime1.gameObject);
                this.slimeMergeEventPairs.Remove(item);
                return true;
            }
        }
        this.slimeMergeEventPairs.Add(pair);
        return false;
    }


    public async void PlayerShoot() 
    {
        PlayerCanShoot = false;
        await Task.Delay(TimeSpan.FromMilliseconds(300));
        PlayerCanShoot = true;
    }
}
