﻿using System;
using System.Linq;
using System.Text;
using vCardLib.Constants;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.Utilities;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class TelephoneNumberFieldSerializer : IV2FieldSerializer<TelephoneNumber>,
    IV3FieldSerializer<TelephoneNumber>, IV4FieldSerializer<TelephoneNumber>
{
    public string FieldKey => "TEL";

    public string Write(TelephoneNumber data)
    {
        var builder = new StringBuilder(FieldKey);

        if (data.Type != TelephoneNumberType.None)
        {
            var telephoneNumberTypes = Enum.GetValues(data.GetType())
                .Cast<TelephoneNumberType>()
                .Where(x => data.Type.HasFlag(x))
                .ToArray();

            if (telephoneNumberTypes.Any())
            {
                builder.Append(FieldKeyConstants.MetadataDelimiter);

                foreach (var telephoneNumberType in telephoneNumberTypes)
                    builder.AppendFormat("{0}={1}", FieldKeyConstants.TypeKey,
                        telephoneNumberType.DecomposeTelephoneNumberType());
            }
        }

        if (data.Preference.HasValue)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.Append(FieldKeyConstants.PreferenceKey);
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Number);

        return builder.ToString();
    }

    string IV4FieldSerializer<TelephoneNumber>.Write(TelephoneNumber data)
    {
        var builder = new StringBuilder(FieldKey);

        if (data.Type != TelephoneNumberType.None)
        {
            var telephoneNumberTypes = Enum.GetValues(data.GetType())
                .Cast<TelephoneNumberType>()
                .Where(x => data.Type.HasFlag(x))
                .ToArray();

            if (telephoneNumberTypes.Any())
            {
                builder.Append(FieldKeyConstants.MetadataDelimiter);

                foreach (var telephoneNumberType in telephoneNumberTypes)
                    builder.AppendFormat("{0}={1}", FieldKeyConstants.TypeKey,
                        telephoneNumberType.DecomposeTelephoneNumberType());
            }
        }

        if (!string.IsNullOrWhiteSpace(data.Value))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.ValueKey, data.Value);
        }

        if (data.Preference.HasValue)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.PreferenceKey, data.Preference);
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Number);

        return builder.ToString();
    }
}
