using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossWorld : EnemyWorld
{
    
    public override void SetupBattle(Scene scene, LoadSceneMode mode)
    {
        BattleManager.Instance.isFinalBoss = true;
        base.SetupBattle(scene, mode);
    }
}
