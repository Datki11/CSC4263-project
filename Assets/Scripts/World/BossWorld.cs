using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossWorld : EnemyWorld
{
    
    public override void SetupBattle(Scene scene, LoadSceneMode mode)
    {
        base.SetupBattle(scene, mode);
        BattleManager.Instance.isFinalBoss = true;
    }
}
