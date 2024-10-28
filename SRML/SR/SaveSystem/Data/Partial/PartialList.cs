﻿using SRML.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SRML.SR.SaveSystem.Data.Partial
{
    internal class PartialList<T> : PartialData<IList<T>>, IDictionaryProvider
    {
        private readonly Predicate<T> hoistCondition;
        private readonly Func<T> emptyValueCreator;
        private readonly SerializerPair<T> serializer;

        readonly Dictionary<int, T> hoistedValues = new Dictionary<int, T>();

        public PartialList(Predicate<T> hoistCondition, SerializerPair<T> serializer, Func<T> emptyValueCreator = null)
        {
            this.hoistCondition = hoistCondition;
            if (emptyValueCreator == null) emptyValueCreator = () => default(T);
            this.emptyValueCreator = emptyValueCreator;
            this.serializer = serializer;
        }

        public IDictionary InternalDictionary => hoistedValues;

        public override void Pull(IList<T> data)
        {
            hoistedValues.Clear();
            for (int i = 0; i < data.Count; i++)
            {
                if (hoistCondition(data[i]))
                {
                    hoistedValues.Add(i, data[i]);
                    data[i] = emptyValueCreator();
                }
            }
        }

        public override void Push(IList<T> data)
        {
            foreach (var pair in hoistedValues)
            {
                data[pair.Key] = pair.Value;
            }
        }

        public override void Read(BinaryReader reader)
        {
            BinaryUtils.ReadDictionary(reader, hoistedValues, (x) => x.ReadInt32(), (x) => (T)serializer.Deserialize(x));
        }

        public override void Write(BinaryWriter writer)
        {
            BinaryUtils.WriteDictionary(writer, hoistedValues, (x, y) => x.Write(y), (x, y) => serializer.Serialize(x, y));
        }
    }
}
