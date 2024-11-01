﻿using MonomiPark.SlimeRancher.DataModel;
using SRML.SR.UI;
using UnityEngine;

namespace SRML.SR
{
    public static class SRCallbacks
    {

        static SRCallbacks()
        {
            OnMainMenuLoaded += ModMenuUIHandler.OnMainMenuLoaded;
        }

        public delegate void OnSaveGameLoadedDelegate(SceneContext t);
        public delegate void OnMainMenuLoadedDelegate(MainMenuUI mainMenu);
        public delegate void OnActorSpawnDelegate(Identifiable.Id id, GameObject obj, ActorModel model);
        public delegate void OnActorDestroyedDelegate(Identifiable.Id id, GameObject obj, ActorModel model);
        public delegate void OnPediaEntrySelectedDelegate(PediaDirector.Id id);
        public delegate void OnPediaEntryUnlockedDelegate(PediaDirector.Id id);
        public delegate void OnProgressChangedDelegate(PediaDirector.Id id, int progress);
        public delegate void OnGadgetBuiltDelegate(Gadget.Id id, GameObject obj, GadgetModel model);
        public delegate void OnLandPlotBuiltDelegate(LandPlot.Id id, GameObject obj, LandPlotModel model);
        public delegate void OnGadgetDemolishedDelegate(Gadget.Id id, GameObject obj, GadgetModel model);
        public delegate void OnLandPlotDemolishedDelegate(LandPlot.Id id, GameObject obj, LandPlotModel model);
        public delegate void OnBlueprintUnlockedDelegate(Gadget.Id blueprint);
        public delegate void OnBlueprintPurchasedDelegate(Gadget.Id blueprint);
        public delegate void OnZoneEnteredDelegate(ZoneDirector.Zone zone, PlayerState playerState);
        public delegate void OnCorporateLevelBoughtDelegate(int level);
        internal delegate void OnGameContextReadyDelegate();

        public static event OnSaveGameLoadedDelegate OnSaveGameLoaded;
        public static event OnSaveGameLoadedDelegate PreSaveGameLoaded;
        public static event OnSaveGameLoadedDelegate PreSaveGameLoad;
        public static event OnMainMenuLoadedDelegate OnMainMenuLoaded;
        public static event OnActorSpawnDelegate OnActorSpawn;
        public static event OnZoneEnteredDelegate OnZoneEntered;
        public static event OnCorporateLevelBoughtDelegate OnCorporateLevelBought;
        internal static event OnGameContextReadyDelegate OnGameContextReady;

        internal static void OnLoad()
        {
            OnGameContextReady?.Invoke();
        }

        internal static void OnMainMenuLoad(MainMenuUI mainmenu) => OnMainMenuLoaded?.Invoke(mainmenu);

        internal static void OnZoneEnterCallback(ZoneDirector.Zone zone, PlayerState playerState) => OnZoneEntered?.Invoke(zone, playerState);

        internal static void OnCorporateLevelBoughtCallback(ProgressDirector dir) => OnCorporateLevelBought?.Invoke(dir.GetProgress(ProgressDirector.ProgressType.CORPORATE_PARTNER));

        internal static void OnSceneLoaded(SceneContext t)
        {
            if (Levels.isMainMenu()) return;
            OnSaveGameLoaded?.Invoke(t);
        }

        internal static void OnActorSpawnCallback(Identifiable.Id id, GameObject obj, ActorModel model) => OnActorSpawn?.Invoke(id, obj, model);

        internal static void PreSceneLoad(SceneContext t)
        {
            if (Levels.isMainMenu()) return;
            PreSaveGameLoad?.Invoke(t);
            PreSaveGameLoaded?.Invoke(t);
        }
    }
}
