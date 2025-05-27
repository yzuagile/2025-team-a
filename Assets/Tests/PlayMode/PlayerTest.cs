using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTest
{
    public class TestPlayerStats : PlayerStats
    {
        public bool dieCalled = false;

        public override void Die()
        {
            dieCalled = true;
            base.Die(); // 可選：保留原本行為，或拿掉只測試 flag
        }
    }
    private GameObject CreateTestEnemy()
    {
        var data = ScriptableObject.CreateInstance<EnemyData>();
        data.contactDamage = 10f;
        data.maxHealth = 50f;
        data.movementSpeed = 0f;
        data.enemyName = "TestEnemy";
        data.experienceDropped = 5;

        var fakeOrb = new GameObject("FakeOrb");
        fakeOrb.AddComponent<CircleCollider2D>();
        fakeOrb.AddComponent<ExperienceOrb>();

        GameObject enemy = new GameObject("Enemy");
        enemy.AddComponent<CircleCollider2D>().isTrigger = true;
        // 初始化資料先備好
        enemy.AddComponent<Animator>().runtimeAnimatorController =
           Resources.Load<RuntimeAnimatorController>("DummyAnimator");
        var stats = enemy.AddComponent<EnemyStats>();
        stats.enemyData = data;
        stats.experienceOrbPrefab = fakeOrb;

        
        enemy.AddComponent<Rigidbody2D>().gravityScale = 0;
       

        enemy.AddComponent<EnemyMovement>();

        return enemy;
    }



    // A Test behaves as an ordinary method
    [Test]
    public void TakeDamage_ReducesHealthCorrectly()
    {
        GameObject obj = new GameObject();
        obj.AddComponent<Rigidbody2D>();
        obj.AddComponent<PlayerMovements>();
       // obj.AddComponent<UIManager>();
        PlayerStats player = obj.AddComponent<PlayerStats>();

        player.currentHealth = 100f;

        player.TakeDamage(10f);

        Assert.AreEqual(90f, player.currentHealth);
    }
    [Test]
    public void TakeDamage_HealthDoesNotGoBelowZero_TriggersDie()
    {
        GameObject obj = new GameObject();
        obj.AddComponent<Rigidbody2D>();
        obj.AddComponent<PlayerMovements>();
        obj.AddComponent<Animator>();
        TestPlayerStats player = obj.AddComponent<TestPlayerStats>();
        player.maxHealth = 100f;
        player.currentHealth = 10f;

        player.TakeDamage(10f);

        Assert.AreEqual(0f, player.currentHealth);
        Assert.IsTrue(player.dieCalled);
    }

   
    //[UnityTest]
    //public IEnumerator PlayerTakesDamageEveryHalfSecond_IfInRange()
    //{
    //    GameObject playerObj = new GameObject();
    //    playerObj.tag = "Player";
    //    playerObj.AddComponent<Rigidbody2D>();
    //    playerObj.AddComponent<PlayerMovements>();
    //    playerObj.AddComponent<Animator>();

    //    PlayerStats playerStats = playerObj.AddComponent<PlayerStats>();
    //    playerStats.maxHealth = 100f;
    //    playerStats.currentHealth = 100f;

    //    GameObject enemyObj = CreateTestEnemy();

    //    enemyObj.transform.position = Vector2.zero;
    //    playerObj.transform.position = Vector2.zero;

    //    yield return new WaitForSeconds(2.2f);

    //    float damage = playerStats.maxHealth - playerStats.currentHealth;
    //    Debug.Log($"玩家共受到傷害: {damage}");

    //    Assert.GreaterOrEqual(damage, 20f);
    //    Assert.LessOrEqual(damage, 30f);
    //}



    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    //[UnityTest]
    //public IEnumerator PlayerTestWithEnumeratorPasses()
    //{
    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}
}
