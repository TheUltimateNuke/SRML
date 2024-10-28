﻿using HarmonyLib;
using System.Linq;

namespace SRML.SR.Patches
{
    [HarmonyPatch(typeof(PlayerState))]
    [HarmonyPatch("Reset")]
    internal static class PlayerStateResetPatch
    {
        public static void Postfix(PlayerState __instance)
        {
            foreach (var v in AmmoRegistry.inventoryPrefabsToPatch.Where((x) => x.Key != PlayerState.AmmoMode.DEFAULT))
            {
                foreach (var id in v.Value)
                {
                    __instance.ammoDict[v.Key].potentialAmmo.Add(id);
                }
            }
        }
    }
}
