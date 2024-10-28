﻿using HarmonyLib;
using SRML.SR.SaveSystem.Data.Ammo;
using System.Diagnostics;
using UnityEngine;

namespace SRML.SR.SaveSystem.Patches
{
    [HarmonyPatch(typeof(Ammo))]
    [HarmonyPatch("GetSelectedStored")]
    internal static class AmmoGetSelectedStoredPatch
    {
        public static void Postfix(Ammo __instance, GameObject __result)
        {
            if (!__result) return;

            if ((new StackFrame(2)).GetMethod().DeclaringType == typeof(VacColorAnimator)) return;

            if (AmmoIdentifier.TryGetIdentifier(__instance, out var identifier))
            {
                if (PersistentAmmoManager.HasPersistentAmmo(identifier))
                {
                    PersistentAmmoManager.PersistentAmmoData[identifier].OnSelected(Identifiable.GetId(__result), __instance.selectedAmmoIdx);
                }
            }
        }
    }
}
