﻿using HarmonyLib;

namespace SRML.SR.Patches
{
    [HarmonyPatch(typeof(DirectedActorSpawner))]
    [HarmonyPatch("Start")]

    internal static class DirectedActorSpawnerStartPatch
    {
        public static void Postfix(DirectedActorSpawner __instance)
        {
            foreach (var v in DirectedActorSpawnerRegistry.spawnerFixers)
            {
                if (v.Key(__instance)) v.Value(__instance);
            }
        }
    }
}
