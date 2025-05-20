using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void PlayerTestSimplePasses()
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
