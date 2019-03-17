﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using System.Reflection;

namespace SRML
{
    internal static class SRModLoader
    {
        public const string ModJson = "modinfo.json";

        static readonly Dictionary<String,SRMod> Mods = new Dictionary<string, SRMod>();

        public static void LoadMods()
        {
            FileSystem.CheckDirectory(FileSystem.ModPath);
            HashSet<ProtoMod> foundMods = new HashSet<ProtoMod>(new ProtoMod.Comparer());
            foreach (var v in Directory.GetFiles(FileSystem.ModPath, ModJson, SearchOption.AllDirectories))
            {
                var mod = ProtoMod.ParseFromJson(v);
                if (!foundMods.Add(mod))
                {
                    throw new Exception($"Found mod with duplicate id '{mod.id}' in {v}!");
                }



            }

            DependencyChecker.CheckDependencies(foundMods);

            LoadAssemblies(foundMods);
        }

        static void LoadAssemblies(ICollection<ProtoMod> protomods)
        {
            foreach (var mod in protomods)
            {
                foreach (var file in Directory.GetFiles(mod.path,"*.dll", SearchOption.AllDirectories))
                {
                    var a = Assembly.LoadFrom(Path.GetFullPath(file));
                    Type entryType = a.ManifestModule.GetTypes()
                        .FirstOrDefault((x) => (x.BaseType?.FullName ?? "") == "SRML.ModEntryPoint");
                    if (entryType == default(Type)) continue;
                    AddMod(mod, entryType);
                    goto foundmod;
                }

                throw new EntryPointNotFoundException($"Could not find assembly for mod '{mod.id}'");

                foundmod:
                continue;
            }
        }

        static void AddMod(ProtoMod modInfo, Type entryType)
        {
            ModEntryPoint entryPoint = (ModEntryPoint) Activator.CreateInstance(entryType);
            Mods.Add(modInfo.id,new SRMod(modInfo.ToModInfo(),entryPoint));
        }

        public static void PreLoadMods()
        {
            foreach (var mod in Mods)
            {
                mod.Value.PreLoad();

            }
        }

        public static void PostLoadMods()
        {
            foreach (var mod in Mods)
            {
                mod.Value.PostLoad();
            }
        }

        internal class ProtoMod
        {
            public string id;
            public string name;
            public string author;
            public string version;
            public string path;
            public override bool Equals(object o)
            {
                if (!(o is ProtoMod obj)) return base.Equals(o);
                return id == obj.id;
            }

            public static ProtoMod ParseFromJson(String jsonFile)
            {

                var proto =
                    JsonConvert.DeserializeObject<ProtoMod>(File.ReadAllText(jsonFile));
                proto.path = Path.GetDirectoryName(jsonFile);
                proto.ValidateFields();
                return proto;

            }

            void ValidateFields()
            {
                id = id.ToLower();
                if (id.Contains(" ")) throw new Exception($"Invalid mod id: {id}");
            }

            public SRModInfo ToModInfo()
            {
                return new SRModInfo(id, name, author, SRModInfo.ModVersion.Parse(version));
            }
            public override int GetHashCode()
            {
                return 1877310944 + EqualityComparer<string>.Default.GetHashCode(id);
            }

            public class Comparer : IEqualityComparer<ProtoMod>
            {
                public bool Equals(ProtoMod x, ProtoMod y)
                {
                    return x.Equals(y);
                }

                public int GetHashCode(ProtoMod obj)
                {
                    return obj.GetHashCode();
                }
            }

        }
    }
}