﻿using HarmonyLib;
using System;
using System.Diagnostics;
using System.Linq;

namespace SRML.SR.Patches
{
    [HarmonyPatch(typeof(UITemplates))]
    [HarmonyPatch("CreatePurchaseUI")]
    internal static class UITemplatesPurchasablePatch
    {
        public static void Prefix(ref PurchaseUI.Purchasable[] purchasables, PurchaseUI.OnClose onClose)
        {
            StackTrace trace = new StackTrace(1);
            Type type = null;
            foreach (var v in trace.GetFrames())
            {
                var candidateType = v.GetMethod().DeclaringType;
                if (typeof(BaseUI).IsAssignableFrom(candidateType))
                {
                    type = candidateType;
                    break;
                }
            }
            if (type == null) return;
            BaseUI ui = onClose.Target as BaseUI;

            purchasables = PurchasableUIRegistry.customPurchasables.Where(x => x.Key(type, ui)).Select(x => x.Value(ui)).ToArray().AddRangeToArray(purchasables);

            foreach (var v in PurchasableUIRegistry.customManipulators.Where(x => x.Key(type, ui))) v.Value(ui, ref purchasables);
        }
    }
}
