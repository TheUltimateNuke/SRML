﻿using HarmonyLib;
using MonomiPark.SlimeRancher.DataModel;

namespace SRML.SR.Patches
{
    [HarmonyPatch(typeof(PlayerState))]
    [HarmonyPatch("InitUpgradeLocks")]
    internal static class InitUpgradeLocksPatch
    {
        public static void Postfix(PlayerState __instance, PlayerModel model)
        {
            foreach (var v in PersonalUpgradeRegistry.moddedLockers)
            {
                if (v.Value == null) model.availUpgrades.Add(v.Key);
                else model.upgradeLocks[v.Key] = v.Value(__instance);

            }
        }
    }
}
