﻿using System.Collections.Generic;

namespace SRML.Editor
{
    internal static class ReplacerCache
    {
        static readonly Dictionary<IFieldReplacer, ResolvedReplacer> replacers = new Dictionary<IFieldReplacer, ResolvedReplacer>();

        public static ResolvedReplacer GetReplacer(IFieldReplacer replacer)
        {
            if (replacers.TryGetValue(replacer, out var resolved)) return resolved;
            var newreplacer = ResolvedReplacer.Resolve(replacer);
            replacers.Add(replacer, newreplacer);
            return newreplacer;
        }

        public static void ClearCache()
        {
            replacers.Clear();
        }

    }
}
