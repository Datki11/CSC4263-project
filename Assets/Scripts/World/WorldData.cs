using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to keep world data persistent when switching between the world and battle.
public class WorldData : MonoBehaviour
{
    public Vector3 playerPos;
    public class EnemyData {
        public Vector2 startPos;
        public Vector3 pos;
    }
    public List<EnemyData> enemyDatas;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player World");
        playerPos = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, playerObj.GetComponent<PlayerWorldController>().initialZ);
        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy World"));
        enemyDatas = new List<EnemyData>();
        foreach(GameObject enemy in enemies) {
            EnemyData enemyData = new EnemyData();
            enemyData.startPos = new Vector2(enemy.GetComponent<EnemyWorld>().startPos.x, enemy.GetComponent<EnemyWorld>().startPos.y);
            enemyData.pos = new Vector3(enemy.gameObject.transform.position.x, enemy.gameObject.transform.position.y, enemy.GetComponent<WorldPerspective>().initialZ);
            enemyDatas.Add(enemyData);
        }
    }

    public void Reload() {

        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy World"));
        foreach(GameObject enemy in enemies) {
            Vector2 enemyStartPos = enemy.GetComponent<EnemyWorld>().startPos;
            EnemyData enemyData = enemyDatas.Find(e => e.startPos.x == enemyStartPos.x && e.startPos.y == enemyStartPos.y);
            if (enemyData == null)
                Destroy(enemy);
            else
                enemy.transform.position = enemyData.pos;
        }

        GameObject.FindGameObjectWithTag("Player World").transform.position = playerPos;

        Destroy(gameObject);

    }
}
