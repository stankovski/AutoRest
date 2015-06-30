// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Rest.Generator.ClientModel;
using Microsoft.Rest.Generator.Utilities;

namespace Microsoft.Rest.Generator.Go
{
    public static class ClientModelExtensions
    {
        public const string ApiVersionName = "ApiVersion";
        public const string ApiVersionSerializedName = "api-version";

        public const string SkipUrlEncoding = "x-ms-skip-url-encoding";

        /////////////////////////////////////////////////////////////////////////////////////////
        //
        // General Extensions
        //
        /////////////////////////////////////////////////////////////////////////////////////////

        public static string ToSentence(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            else
            {
                value = value.Trim();
                return value.First().ToString().ToLowerInvariant() + (value.Length > 1 ? value.Substring(1) : "");
            }
        }

        public static string EmitAsArguments(this IList<string> arguments)
        {
            var sb = new StringBuilder();

            if (arguments.Count() > 0)
            {
                sb.Append(arguments[0]);

                for (var i = 1; i < arguments.Count(); i++)
                {
                    sb.AppendLine(",");
                    sb.Append(arguments[i]);
                }
            }

            sb.Append(")");

            return sb.ToString();
        }

        /////////////////////////////////////////////////////////////////////////////////////////
        //
        // Parameter Extensions
        //
        /////////////////////////////////////////////////////////////////////////////////////////

        public static string BuildParameterMap(this IEnumerable<Parameter> parameters, string mapVariable, bool urlEncodeIfNeeded = false)
        {
            var builder = new StringBuilder();

            if (parameters.Count() > 0)
            {
                builder.Append(mapVariable);
                builder.Append(" := map[string]interface{} {");
                if (parameters.Count() > 0)
                {
                    builder.AppendLine();
                    var indented = new IndentedStringBuilder("  ");
                    foreach (var parameter in parameters.OrderBy(p => p.SerializedName))
                    {
                        string serializedName = string.Empty;
                        string name = string.Empty;

                        // TODO (gosdk): Could we catch and handl APIVersion without explicit logic?
                        if (parameter.IsApiVersion())
                        {
                            serializedName = ApiVersionSerializedName;
                            name = ApiVersionName;
                        }
                        else
                        {
                            serializedName = parameter.SerializedName;
                            name = parameter.IsClientProperty()
                                ? "client." + GoCodeNamer.PascalCase(parameter.Name)
                                : parameter.Name;
                            
                            if (urlEncodeIfNeeded && parameter.RequiresUrlEncoding())
                            {
                                name = string.Format("url.QueryEscape({0})", name);
                            }
                        }

                        indented.AppendLine("\"{0}\": {1},", serializedName, name);
                    }
                    builder.Append(indented);
                }
                builder.AppendLine("}");
            }
            return builder.ToString();
        }

        public static IEnumerable<Parameter> ByLocation(this IEnumerable<Parameter> parameters, ParameterLocation location)
        {
            return parameters
                .Where(p => p.Location == location)
                .AsEnumerable();
        }

        public static IEnumerable<Parameter> BodyParameters(this IEnumerable<Parameter> parameters)
        {
            return parameters.ByLocation(ParameterLocation.Body);
        }

        public static IEnumerable<Parameter> FormDataParameters(this IEnumerable<Parameter> parameters)
        {
            return parameters.ByLocation(ParameterLocation.FormData);
        }

        public static IEnumerable<Parameter> HeaderParameters(this IEnumerable<Parameter> parameters)
        {
            return parameters.ByLocation(ParameterLocation.Header);
        }

        public static IEnumerable<Parameter> PathParameters(this IEnumerable<Parameter> parameters)
        {
            return parameters.ByLocation(ParameterLocation.Path);
        }

        public static IEnumerable<Parameter> QueryParameters(this IEnumerable<Parameter> parameters)
        {
            return parameters.ByLocation(ParameterLocation.Query);
        }

        /// <summary>
        /// Return the separator associated with a given collectionFormat
        /// </summary>
        /// <param name="format">The collection format</param>
        /// <returns>The separator</returns>
        private static string GetSeparator(this CollectionFormat format)
        {
            switch (format)
            {
                case CollectionFormat.Csv:
                    return ",";
                case CollectionFormat.Pipes:
                    return "|";
                case CollectionFormat.Ssv:
                    return " ";
                case CollectionFormat.Tsv:
                    return "\t";
                default:
                    throw new NotSupportedException(string.Format("Collection format {0} is not supported.", format));
            }
        }

        public static bool IsClientProperty(this Parameter parameter)
        {
            return parameter.ClientProperty != null;
        }

        public static bool IsMethodArgument(this Parameter parameter)
        {
            return !parameter.IsClientProperty();
        }

        public static bool IsApiVersion(this Parameter parameter)
        {
            return parameter.Name.Equals(ApiVersionName, StringComparison.OrdinalIgnoreCase);
        }

        public static bool RequiresUrlEncoding(this Parameter parameter)
        {
            return !parameter.Extensions.ContainsKey(SkipUrlEncoding);
        }

        public static string JsonTag(this Property property, bool omitEmpty = true)
        {
            return string.Format("`json:\"{0}{1}\"`", property.SerializedName, omitEmpty ? ",omitempty" : "");
        }

        /////////////////////////////////////////////////////////////////////////////////////////
        //
        // Type Extensions
        //
        /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Determines if a type can be assigned the value null
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>true if null can be assigned, otherwise false</returns>
        public static bool CanBeNull(this IType type)
        {
            var composite = type as CompositeType;
            var sequence = type as SequenceType;
            var dictionary = type as DictionaryType;
            var known = type as PrimaryType;
            return composite != null
                || sequence != null
                || dictionary != null
                || (known != null
                    && (known == PrimaryType.Object
                        || known == PrimaryType.ByteArray
                        || known == PrimaryType.Stream));
        }

        /// <summary>
        /// Determines if a type can be empty
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>true if null can be assigned, otherwise false</returns>
        public static bool CanBeEmpty(this IType type)
        {
            var sequence = type as SequenceType;
            var dictionary = type as DictionaryType;
            var known = type as PrimaryType;
            return sequence != null
                || dictionary != null
                || (known != null
                    && (known == PrimaryType.ByteArray
                        || known == PrimaryType.String));
        }
        public static string GetEmptyCheck(string valueReference)
        {
            return string.Format("len({0}) == 0", valueReference);
        }

        public static string GetNotEmptyCheck(string valueReference)
        {
            return string.Format("len({0}) > 0", valueReference);
        }

        public static string GetNullCheck(string valueReference)
        {
            return string.Format("{0} == nil", valueReference);
        }

        public static string Fields(this CompositeType compositeType)
        {
            var indented = new IndentedStringBuilder("    ");

            if (compositeType.BaseModelType != null)
            {
                indented.Append(compositeType.BaseModelType.Fields());
            }

            foreach (var property in compositeType.Properties)
            {
                if (property.Type is CompositeType)
                {
                    indented.Indent();
                    indented.AppendFormat("{0} struct {{\n", property.Name);
                    indented.Append((property.Type as CompositeType).Fields());
                    indented.AppendFormat("}} {0}\n", property.JsonTag());
                    indented.Outdent();
                }
                else
                {
                    indented.AppendFormat("{0} {1} {2}\n", property.Name, property.Type.Name, property.JsonTag());
                }
            }
            
            return indented.ToString();
        }

        public static bool ReliesOn(this IType rootType, IType type)
        {
            if (rootType is CompositeType)
            {
                return (rootType as CompositeType).Properties.Any(p => p.Type.ReliesOn(type));
            }
            else if (rootType is DictionaryType)
            {
                var dt = rootType as DictionaryType;
                return dt.ValueType.Equals(type) || (dt.ValueType is CompositeType && dt.ValueType.ReliesOn(type));
            }
            else if (rootType is SequenceType)
            {
                var st = rootType as SequenceType;
                return st.ElementType.Equals(type) || (st.ElementType is CompositeType && st.ElementType.ReliesOn(type));
            }
            return false;
        }

        public static IEnumerable<string> Imports(this CompositeType compositeType)
        {
            var imports = new HashSet<string>();
            compositeType
                .Properties
                .ForEach(p =>
                {
                    if (p.Type is DictionaryType)
                    {
                        imports.UnionWith((p.Type as DictionaryType).Imports());
                    }
                    else if (p.Type is PackageType)
                    {
                        imports.UnionWith((p.Type as PackageType).Imports());
                    }
                    else if (p.Type is SequenceType)
                    {
                        imports.UnionWith((p.Type as SequenceType).Imports());
                    }
                });
            return imports;
        }

        public static IEnumerable<string> Imports(this DictionaryType dictionaryType)
        {
            var imports = new HashSet<string>();
            return imports;
        }

        public static IEnumerable<string> Imports(this SequenceType sequenceType)
        {
            var imports = new HashSet<string>();
            return imports;
        }

        /// <summary>
        /// Generate code to perform required validation on a type
        /// </summary>
        /// <param name="type">The type to validate</param>
        /// <param name="scope">A scope provider for generating variable names as necessary</param>
        /// <param name="valueReference">A reference to the value being validated</param>
        /// <returns>The code to validate the reference of the given type</returns>
        public static string ValidateType(this IType type, IScopeProvider scope, string valueReference)
        {
            CompositeType model = type as CompositeType;
            SequenceType sequence = type as SequenceType;
            DictionaryType dictionary = type as DictionaryType;
            /*
            if (model != null && model.Properties.Any())
            {
                return CheckNotNull(valueReference, string.Format("{0}.Validate();", valueReference));
            }
            if (sequence != null)
            {
                var elementVar = scope.GetVariableName("element");
                var innerValidation = sequence.ElementType.ValidateType(scope, elementVar);
                if (!string.IsNullOrEmpty(innerValidation))
                {
                    var sequenceBuilder = string.Format("foreach ( var {0} in {1})\r\n{{\r\n", elementVar,
                        valueReference);
                    sequenceBuilder += string.Format("    {0}\r\n}}", innerValidation);
                    return CheckNotNull(valueReference, sequenceBuilder);
                }
            }
            else if (dictionary != null)
            {
                var valueVar = scope.GetVariableName("valueElement");
                var innerValidation = dictionary.ValueType.ValidateType(scope, valueVar);
                if (!string.IsNullOrEmpty(innerValidation))
                {
                    var sequenceBuilder = string.Format("if ( {0} != null)\r\n{{\r\n", valueReference);
                    sequenceBuilder += string.Format("    foreach ( var {0} in {1}.Values)\r\n    {{\r\n", valueVar,
                        valueReference);
                    sequenceBuilder += string.Format("        {0}\r\n    }}\r\n}}", innerValidation);
                    return CheckNotNull(valueReference, sequenceBuilder);
                }
            }
             */

            return null;
        }

        public static bool HasModelTypes(this Method method)
        {
            return method.Parameters.Any(p => p.Type is CompositeType) ||
                   method.Responses.Any(r => r.Value is CompositeType) || method.ReturnType is CompositeType ||
                   method.DefaultResponse is CompositeType;
        }
    }
}
