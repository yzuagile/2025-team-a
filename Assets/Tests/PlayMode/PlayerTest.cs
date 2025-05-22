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

        public void Die()
        {
            dieCalled = true;
            base.Die(); // �i��G�O�d�쥻�欰�A�ή����u���� flag
        }
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
        var obj = new GameObject();
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
