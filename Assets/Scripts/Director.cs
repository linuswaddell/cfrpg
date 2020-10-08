﻿using ActorComponents;
using UnityEngine;

// Responsible for triggering 'random' events like traders arriving
public class Director : MonoBehaviour
{
    TimeKeeper.DateTime lastTraderArrival;

    // Update is called once per frame
    void Update()
    {
        if (!GameInitializer.InitializationFinished)
        {
            return;
        }

        if (TimeKeeper.daysBetween(lastTraderArrival, TimeKeeper.CurrentDateTime) > 1)
        {
            lastTraderArrival = TimeKeeper.CurrentDateTime;

            // Trigger a trader i guess?

            // Generate a new trader
            ActorData newTrader = ActorGenerator.Generate(ContentLibrary.Instance.CharacterGenTemplates.Get("trader"));
            newTrader.ActorComponents.Add(new Trader(newTrader.actorId));
            // Register the actor
            ActorRegistry.RegisterActor(newTrader);
            // Give him some money
            newTrader.Wallet.SetBalance(Random.Range(100, 1000));
            // Give him seeds to sell
            newTrader.Inventory.AttemptAddItem(new ItemStack("seed_bag", Random.Range(1, 30)));
            // Spawn the actor
            Vector2Int spawn = WorldMapManager.FindWalkableEdgeTile(Direction.Left);
            ActorSpawner.Spawn(newTrader.actorId, spawn, SceneObjectManager.WorldSceneId);

            // Do a notification
            NotificationManager.Notify(newTrader.ActorName + ", a trader, is stopping by!");
        }
    }
}
