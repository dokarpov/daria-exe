using NUnit.Framework;
using UnityEngine;
using DariaExe;

public sealed class ResearchLabSmokeTests
{
    [Test]
    public void PlayerController_Requires_Rigidbody2D()
    {
        var attr = (RequireComponent)System.Attribute.GetCustomAttribute(
            typeof(PlayerController), typeof(RequireComponent));
        Assert.IsNotNull(attr);
    }

    [Test]
    public void Interaction_Component_Can_Be_Created()
    {
        var go = new GameObject("test");
        Assert.IsNotNull(go.AddComponent<Interactable>());
        Object.DestroyImmediate(go);
    }
}
