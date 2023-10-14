﻿using AlbanianXrm.PowerPlatform.JsonConverters;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.PowerPlatform
{
    public class EntitySerializer
    {
        public const string TypePropertyName = "__type";
        public const string ValuePropertyName = "__value";
        public const string CollectionKeyPropertyName = "key";
        public const string CollectionValuePropertyName = "value";

        public static T Deserialize<T>(string json, EntitySerializerOptions options = default)
        {
            var jsonSerializerOptions = InitializeOptions(options);
            return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
        }

        public static T Deserialize<T>(ref Utf8JsonReader reader, EntitySerializerOptions options = default)
        {
            var jsonSerializerOptions = InitializeOptions(options);
            return JsonSerializer.Deserialize<T>(ref reader, jsonSerializerOptions);
        }

        public static T Deserialize<T>(ReadOnlySpan<byte> utf8Json, EntitySerializerOptions options = default)
        {
            var jsonSerializerOptions = InitializeOptions(options);
            return JsonSerializer.Deserialize<T>(utf8Json, jsonSerializerOptions);
        }

        public static ValueTask<T> DeserializeAsync<T>(Stream utf8Json, EntitySerializerOptions options = default, CancellationToken cancellationToken = default)
        {
            var jsonSerializerOptions = InitializeOptions(options);
            return JsonSerializer.DeserializeAsync<T>(utf8Json, jsonSerializerOptions, cancellationToken);
        }

        public static string Serialize(object value, Type inputType, EntitySerializerOptions options = default)
        {
            var jsonSerializerOptions = InitializeOptions(options);
            return JsonSerializer.Serialize(value, inputType, jsonSerializerOptions);
        }

        internal static JsonSerializerOptions InitializeOptions(EntitySerializerOptions entitySerializerOptions = default)
        {
            if (entitySerializerOptions == null)
            {
                entitySerializerOptions = new EntitySerializerOptions();
            }
            var jsonSerializerOptions = entitySerializerOptions.JsonSerializerOptions;

            foreach (var item in jsonSerializerOptions.Converters)
            {
                if (CanConvert<AttributeCollection>(item, entitySerializerOptions.converters) ||
                    CanConvert<DateTime>(item, entitySerializerOptions.converters) ||
                    CanConvert<EntityCollection>(item, entitySerializerOptions.converters) ||
                    CanConvert<Entity>(item, entitySerializerOptions.converters) ||
                    CanConvert<EntityImageCollection>(item, entitySerializerOptions.converters) ||
                    CanConvert<EntityReference>(item, entitySerializerOptions.converters) ||
                    CanConvert<FormattedValueCollection>(item, entitySerializerOptions.converters) ||
                    CanConvert<Guid>(item, entitySerializerOptions.converters) ||
                    CanConvert<KeyAttributeCollection>(item, entitySerializerOptions.converters) ||
                    CanConvert<IList<object>>(item, entitySerializerOptions.converters) ||
                    CanConvert<IList<Entity>>(item, entitySerializerOptions.converters) ||
                    CanConvert<Money>(item, entitySerializerOptions.converters) ||
                    CanConvert<object>(item, entitySerializerOptions.converters) ||
                    CanConvert<OptionSetValue>(item, entitySerializerOptions.converters) ||
                    CanConvert<ParameterCollection>(item, entitySerializerOptions.converters) ||
                    CanConvert<RelatedEntityCollection>(item, entitySerializerOptions.converters) ||
                    CanConvert<Relationship>(item, entitySerializerOptions.converters) ||
                    CanConvert<RemoteExecutionContext>(item, entitySerializerOptions.converters))
                {
                    continue;
                }
            }
            EnsureHasConverters(entitySerializerOptions);
            return jsonSerializerOptions;
        }

        internal static bool CanConvert<T>(JsonConverter item, EntitySerializerConverters converters)
        {
            var canConvert = item.CanConvert(typeof(T));
            if (canConvert && !converters.CanConvertType<T>())
            {
                converters.Set<T>((JsonConverter<T>)item);
            }
            return canConvert;
        }

        internal static void EnsureHasConverters(EntitySerializerOptions entitySerializerOptions)
        {
            if (!entitySerializerOptions.converters.CanConvertType<AttributeCollection>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new AttributeCollectionConverter(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<DateTime>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new DateTimeConverter(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<EntityCollection>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new EntityCollectionConverter(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<Entity>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new EntityConverter(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<EntityImageCollection>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new EntityImageCollectionConverter(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<EntityReference>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new EntityReferenceConverter(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<FormattedValueCollection>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new FormattedValueCollectionConverter()));
            }
            if (!entitySerializerOptions.converters.CanConvertType<Guid>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new GuidConverter(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<int>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new IntegerConverter(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<KeyAttributeCollection>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new KeyAttributeCollectionConverter(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<IList<object>>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new ListOfObjectsConverter<object>(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<IList<Entity>>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new ListOfObjectsConverter<Entity>(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<Money>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new MoneyConverter(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<OptionSetValue>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new OptionSetValueConverter(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<ParameterCollection>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new ParameterCollectionConverter(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<Relationship>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new RelationshipConverter(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<RelatedEntityCollection>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new RelatedEntityCollectionConverter(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<RemoteExecutionContext>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new RemoteExecutionContextConverter(entitySerializerOptions)));
            }
            if (!entitySerializerOptions.converters.CanConvertType<object>())
            {
                entitySerializerOptions.JsonSerializerOptions.Converters.Add(
                    entitySerializerOptions.converters.Set(new ObjectContractConverter(entitySerializerOptions)));
            }
        }
    }
}
