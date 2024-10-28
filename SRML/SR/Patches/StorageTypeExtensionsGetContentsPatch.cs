﻿using HarmonyLib;
using System.Collections.Generic;

namespace SRML.SR.Patches
{
    [HarmonyPatch(typeof(StorageTypeExtensions))]
    [HarmonyPatch("GetContents")]
    internal static class StorageTypeExtensionsGetContentsPatch
    {
        public static void Postfix(SiloStorage.StorageType type, HashSet<Identifiable.Id> __result)
        {
            if (!AmmoRegistry.siloPrefabs.ContainsKey(type)) return;
            foreach (var v in AmmoRegistry.siloPrefabs[type])
            {
                __result.Add(v);
            }
        }
    }
}
