﻿using SRML;
using SRML.SR;
using System.Reflection;
using UnityEngine;

namespace SampleMod
{
    public class Main : ModEntryPoint
    {
        // Called before GameContext.Awake
        // this is where you want to register stuff (like custom enum values or identifiable id's)
        // and patch anything you want to patch with harmony
        public void PreLoad()
        {
            Debug.Log("We did it!");
            HarmonyPatcher.GetInstance().PatchAll(Assembly.GetExecutingAssembly());


            // this code registers a callback that's run every time a saved game is loaded
            // in this case it spawns a mosaic boom largo
            SRCallbacks.OnSaveGameLoaded += (scenecontext) =>
            {

                var playerModel = SceneContext.Instance.GameModel.GetPlayerModel();
                SRBehaviour.InstantiateActor(
                    GameContext.Instance.LookupDirector.GetPrefab(Identifiable.Id.MOSAIC_BOOM_LARGO), MonomiPark.SlimeRancher.Regions.RegionRegistry.RegionSetId.UNSET, playerModel.position,
                    playerModel.rotation);
            };
        }


        // Called right before PostLoad
        // Used to register stuff that needs lookupdirector access
        public override void Load()
        {

        }


        // Called after GameContext.Start
        // stuff like gamecontext.lookupdirector are available in this step, generally for when you want to access
        // ingame prefabs and the such
        public override void PostLoad()
        {
            Debug.Log("We did it! Again! ");
        }
    }

}
