﻿using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class PhotoFieldDeserializer : IV2FieldDeserializer<Photo>,
    IV3FieldDeserializer<Photo>, IV4FieldDeserializer<Photo>
{
    public static string FieldKey => "PHOTO";

    Photo IV2FieldDeserializer<Photo>.Read(string input)
    {
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);

        if (metadata.Length == 0)
            return new Photo(value);

        string? type = null, mimeType = null, encoding = null;

        foreach (var datum in metadata)
        {
            var (key, data) = DataSplitHelpers.SplitDatum(datum, '=');

            if (key.EqualsIgnoreCase(FieldKeyConstants.EncodingKey))
                encoding = data;
            // HACK: not sure how else to distinguish the type for v2.1
            else
                type = key;
        }

        return new Photo(value, encoding, type, mimeType);
    }

    Photo IV3FieldDeserializer<Photo>.Read(string input)
    {
        var (metadata, data) = DataSplitHelpers.SplitLine(FieldKey, input);

        if (metadata.Length == 0)
            return new Photo(data);

        string? type = null, mimeType = null, encoding = null, value = null;

        foreach (var datum in metadata)
        {
            var (key, entry) = DataSplitHelpers.SplitDatum(datum, '=');

            if (key.EqualsIgnoreCase(FieldKeyConstants.EncodingKey))
                encoding = entry == "b" ? "BASE64" : data;
            else if (key.EqualsIgnoreCase(FieldKeyConstants.ValueKey))
                value = entry;
            else if (key.EqualsIgnoreCase(FieldKeyConstants.TypeKey))
                type = entry;
        }

        return new Photo(data, encoding, type, mimeType, value);
    }

    Photo IV4FieldDeserializer<Photo>.Read(string input)
    {
        const string dataPrefix = "data:";
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);

        if (metadata.Length == 0)
            return new Photo(value);

        string? type = null, mimeType = null, encoding = null, valueMetadata = null;

        foreach (var datum in metadata)
        {
            var (key, data) = DataSplitHelpers.SplitDatum(datum, '=');

            if (key.EqualsIgnoreCase(FieldKeyConstants.MediaTypeKey) ||
                key.EqualsIgnoreCase(FieldKeyConstants.MediaTypeKey))
                mimeType = data;
            else if (key.EqualsIgnoreCase(FieldKeyConstants.ValueKey))
                valueMetadata = data;
            else if (key.EqualsIgnoreCase(FieldKeyConstants.TypeKey))
                type = data;
        }

        if (value.StartsWithIgnoreCase(dataPrefix))
            value = value.Replace(dataPrefix, string.Empty);

        if (value.Contains(";"))
        {
            var split = value.Split(FieldKeyConstants.MetadataDelimiter);
            mimeType = split[0];
            value = split[1];
        }

        if (value.Contains(","))
        {
            var split = value.Split(',');
            encoding = split[0];
            value = split[1];
        }

        return new Photo(value, encoding, type, mimeType, valueMetadata);
    }
}