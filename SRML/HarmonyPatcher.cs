﻿using HarmonyLib;
using System.Reflection;

namespace SRML
{
    /// <summary>
    /// Utility class to help manage the main SRML harmony instance
    /// </summary>
    public static class HarmonyPatcher
    {
        private static Harmony _instance;

        internal static Harmony Instance
        {
            get
            {
                if (_instance == null)
                {
                    InitializeInstance();
                }

                return _instance;
            }
        }

        static void InitializeInstance()
        {
            _instance = new Harmony("net.veesus.srml");
        }

        internal static void PatchAll()
        {
            Instance.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static Harmony SetInstance(string name)
        {
            var currentMod = SRMod.GetCurrentMod();
            currentMod.CreateHarmonyInstance(name);
            return currentMod.HarmonyInstance;
        }

        public static Harmony GetInstance()
        {
            return SRMod.GetCurrentMod().HarmonyInstance;
        }
    }
}
