﻿using HarmonyLib;
using MonomiPark.SlimeRancher.Persist;
using SRML.SR.SaveSystem;
using System.Collections.Generic;
using System.Linq;

namespace SRML.SR
{
    public static class ExchangeOfferRegistry
    {
        internal static IDRegistry<ProgressDirector.ProgressType> moddedProgress = new IDRegistry<ProgressDirector.ProgressType>();
        internal static IDRegistry<RancherChatMetadata.Entry.RancherName> moddedRancherNames = new IDRegistry<RancherChatMetadata.Entry.RancherName>();

        internal static Dictionary<ExchangeDirector.Rancher, SRMod> customRanchers = new Dictionary<ExchangeDirector.Rancher, SRMod>();
        internal static Dictionary<(ExchangeDirector.Category, Identifiable.Id[]), SRMod> customCategories = new Dictionary<(ExchangeDirector.Category, Identifiable.Id[]), SRMod>();
        internal static Dictionary<ProgressDirector.ProgressType, ExchangeDirector.UnlockList> customUnlocks = new Dictionary<ProgressDirector.ProgressType, ExchangeDirector.UnlockList>();
        internal static Dictionary<Identifiable.Id, float> customUnlockValues = new Dictionary<Identifiable.Id, float>();
        internal static Dictionary<string, SRMod> customRancherIDs = new Dictionary<string, SRMod>();
        internal static Dictionary<string, SRMod> customOfferIDs = new Dictionary<string, SRMod>();

        static ExchangeOfferRegistry()
        {
            ModdedIDRegistry.RegisterIDRegistry(moddedProgress);
            ModdedIDRegistry.RegisterIDRegistry(moddedRancherNames);
        }

        /// <summary>
        /// Registers a rancher.
        /// </summary>
        /// <param name="rancher"></param>
        public static void RegisterRancher(ExchangeDirector.Rancher rancher) => customRanchers.Add(rancher, SRMod.GetCurrentMod());

        /// <summary>
        /// Registers a rancher's id
        /// </summary>
        /// <param name="id"></param>
        public static void RegisterRancherID(string id) => customRancherIDs.Add(id, SRMod.GetCurrentMod());

        /// <summary>
        /// Registers an offer id
        /// </summary>
        /// <param name="id"></param>
        public static void RegisterOfferID(string id) => customOfferIDs.Add(id, SRMod.GetCurrentMod());

        /// <summary>
        /// Registers a category for exchange requests/rewards
        /// </summary>
        /// <param name="category">The category to register</param>
        /// <param name="ids">The ids in the category</param>
        public static void RegisterCategory(ExchangeDirector.Category category, Identifiable.Id[] ids) => customCategories.Add((category, ids), SRMod.GetCurrentMod());

        /// <summary>
        /// Registers an item to be unlocked in a category
        /// </summary>
        /// <param name="item">The <see cref="Identifiable.Id"/> to be unlocked.</param>
        /// <param name="type">The progress required to unlock it</param>
        /// <param name="countForValue">The value used in the offer generator for count</param>
        public static void RegisterUnlockableItem(Identifiable.Id item, ProgressDirector.ProgressType type, int countForValue)
        {
            if (!customUnlocks.ContainsKey(type)) customUnlocks[type] = new ExchangeDirector.UnlockList() { unlock = type, ids = new Identifiable.Id[0] };
            customUnlocks[type].ids = customUnlocks[type].ids.AddToArray(item);
            customUnlockValues.Add(item, countForValue);
        }

        /// <summary>
        /// Registers an item that's automatically unlocked in a category
        /// </summary>
        /// <param name="item">The <see cref="Identifiable.Id"/> to be unlocked.</param>
        /// <param name="countForValue">The value used in the offer generator for count</param>
        public static void RegisterInitialItem(Identifiable.Id item, int countForValue) => RegisterUnlockableItem(item, ProgressDirector.ProgressType.NONE, countForValue);

        /// <summary>
        /// Checks if a rancher or offer ID is modded.
        /// </summary>
        /// <param name="id">The ID to check.</param>
        /// <returns>True if the ID belongs to a mod, otherwise false.</returns>
        public static bool IsCustom(string id) => customRancherIDs.ContainsKey(id) || customOfferIDs.ContainsKey(id);

        /// <summary>
        /// Checks if a rancher is modded.
        /// </summary>
        /// <param name="entry">The rancher to check.</param>
        /// <returns>True if the rancher belongs to a mod, otherwise false.</returns>
        public static bool IsCustom(ExchangeDirector.Rancher entry) => customRanchers.ContainsKey(entry);

        /// <summary>
        /// Checks if a category is modded.
        /// </summary>
        /// <param name="cat">The category to check.</param>
        /// <returns>True if the category belongs to a mod, otherwise false.</returns>
        public static bool IsCustom(ExchangeDirector.Category cat) => customCategories.Any(x => x.Key.Item1 == cat);

        internal static bool IsCustom(ExchangeOfferV04 offer) => IsCustom(offer.offerId) || IsCustom(offer.rancherId);

        internal static SRMod GetModForData(ExchangeOfferV04 offer) => customRancherIDs.Get(offer.rancherId) ?? customOfferIDs.Get(offer.offerId);

        internal static SRMod GetModForID(string id) => customRancherIDs.Get(id) ?? customOfferIDs.Get(id);
    }
}
