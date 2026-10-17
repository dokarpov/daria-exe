using NUnit.Framework;
using UnityEngine;
using UnityEditor;
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

    [Test]
    public void Research_Flow_Is_Sequential_And_Completable()
    {
        var gameGo = new GameObject("game");
        var game = gameGo.AddComponent<ResearchLabGame>();

        Assert.AreEqual(ResearchLabGame.ResearchStage.Observe, game.Stage);
        Assert.IsTrue(game.CanInteract("observe"));
        Assert.IsFalse(game.CanInteract("verify"));

        game.ResolveInteraction(CreateInteractable("observe"), "Observed");
        Assert.AreEqual(ResearchLabGame.ResearchStage.Talk, game.Stage);
        Assert.IsTrue(game.CanInteract("researcher"));
        Assert.IsFalse(game.CanInteract("verify"));

        game.ResolveInteraction(CreateInteractable("researcher"), "Followed up");
        Assert.AreEqual(ResearchLabGame.ResearchStage.Verify, game.Stage);
        Assert.IsTrue(game.CanInteract("verify"));

        game.ResolveInteraction(CreateInteractable("verify"), "Verified");
        Assert.AreEqual(ResearchLabGame.ResearchStage.Complete, game.Stage);
        Assert.IsFalse(game.CanInteract("observe"));
        Assert.IsFalse(game.CanInteract("researcher"));
        Assert.IsFalse(game.CanInteract("verify"));

        Object.DestroyImmediate(gameGo);
    }

    static Interactable CreateInteractable(string id)
    {
        var go = new GameObject(id);
        var item = go.AddComponent<Interactable>();
        var so = new SerializedObject(item);
        so.FindProperty("id").stringValue = id;
        so.ApplyModifiedPropertiesWithoutUndo();
        return item;
    }
}
