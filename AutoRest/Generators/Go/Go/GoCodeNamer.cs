// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Rest.Generator.ClientModel;
using Microsoft.Rest.Generator.Go;
using Microsoft.Rest.Generator.Utilities;

namespace Microsoft.Rest.Generator.Go
{
    public class GoCodeNamer : CodeNamer
    {
        private readonly HashSet<IType> _normalizedTypes;

        /// <summary>
        /// Initializes a new instance of GoCodeNamingFramework.
        /// </summary>
        public GoCodeNamer()
        {
            new HashSet<string>
            {
                // Reserved keywords -- list retrieved from http://golang.org/ref/spec#Keywords
                "break",        "default",      "func",         "interface",    "select",
                "case",         "defer",        "go",           "map",          "struct",
                "chan",         "else",         "goto",         "package",      "switch",
                "const",        "fallthrough",  "if",           "range",        "type",
                "continue",     "for",          "import",       "return",       "var",        
                "nil",
                
                // Reserved predeclared identifiers -- list retrieved from http://golang.org/ref/spec#Predeclared_identifiers
                "bool", "byte",
                "complex64", "complex128",
                "error",
                "float32", "float64",
                "int", "int8", "int16", "int32", "int64",
                "rune", "string",
                "uint", "uint8", "uint16", "uint32", "uint64",
                "uintptr",

                "true", "false", "iota",
                
                "nil",

                "append", "cap", "close", "complex", "copy", "delete", "imag", "len", "make", "new", "panic", "print", "println", "real", "recover",


                // Reserved packages -- list retrieved from http://golang.org/pkg/
                // -- Since package names serve as partial identifiers, exclude the standard library
                "archive", "tar", "zip",
                "bufio",
                "builtin",
                "bytes",
                "compress", "bzip2", "flate", "gzip", "lzw", "zlib",
                "container", "heap", "list", "ring",
                "crypto", "aes", "cipher", "des", "dsa", "ecdsa", "elliptic", "hmac", "md5", "rand", "rc4", "rsa", "sha1", "sha256", "sha512", "subtle", "tls", "x509", "pkix",
                "database", "sql", "driver",
                "debug", "dwarf", "elf", "gosym", "macho", "pe", "plan9obj",
                "encoding", "ascii85", "asn1", "base32", "base64", "binary", "csv", "gob", "hex", "json", "pem", "xml",
                "errors",
                "expvar",
                "flag",
                "fmt",
                "go", "ast", "build", "doc", "format", "parser", "printer", "scanner", "token",
                "hash", "adler32", "crc32", "crc64", "fnv",
                "html", "template",
                "image", "color", "palette", "draw", "gif", "jpeg", "png",
                "index", "suffixarray",
                "io", "ioutil",
                "log", "syslog",
                "math", "big", "cmplx", "rand",
                "mime", "multipart",
                "net", "http", "cgi", "cookiejar", "fcgi", "httptest", "httputil", "pprof", "mail", "rpc", "jsonrpc", "smtp", "textproto", "url",
                "os", "exec", "signal", "user",
                "path", "filepath",
                "reflect",
                "regexp", "syntax",
                "runtime", "cgo", "debug", "pprof", "race",
                "sort",
                "strconv",
                "strings",
                "sync", "atomic",
                "syscall",
                "testing", "iotest", "quick",
                "text", "scanner", "tabwriter", "template", "parse",
                "time",
                "unicode", "utf16", "utf8",
                "unsafe",

                // Other reserved names and packages (defined by the base libraries this code uses)
                "autorest", "client", "date", "err", "req", "resp", "result", "sender"

            }.ForEach(s => ReservedWords.Add(s));

            _normalizedTypes = new HashSet<IType>();
        }

        public override void NormalizeClientModel(ServiceClient client)
        {
            base.NormalizeClientModel(client);

            List<SyntheticType> syntheticTypes = new List<SyntheticType>();
            
            foreach (var method in client.Methods)
            {
                var scope = new VariableScopeProvider();
                foreach (var parameter in method.Parameters)
                {
                    parameter.Name = scope.GetVariableName(parameter.Name);
                }

                if (method.ReturnType is SequenceType || method.ReturnType is DictionaryType)
                {
                    SyntheticType st = new SyntheticType(method.ReturnType);
                    if (syntheticTypes.Contains(st))
                    {
                        method.ReturnType = syntheticTypes.Find(i => i.Equals(st));
                    }
                    else
                    {
                        syntheticTypes.Add(st);
                        client.ModelTypes.Add(st);
                        method.ReturnType = st;
                    }
                }
            }
        }

        protected override IType NormalizeType(IType type)
        {
            if (type == null)
            {
                return null;
            }

            // Using Any instead of Contains since object hash is bound to a property which is modified during normalization
            if (_normalizedTypes.Any(item => type.Equals(item)))
            {
                return _normalizedTypes.First(item => type.Equals(item));
            }

            IType normalizedType = null;
            if (type is PrimaryType)
            {
                normalizedType = NormalizePrimaryType(type as PrimaryType);
            }
            if (type is SequenceType)
            {
                normalizedType = NormalizeSequenceType(type as SequenceType);
            }
            if (type is DictionaryType)
            {
                normalizedType = NormalizeDictionaryType(type as DictionaryType);
            }
            if (type is CompositeType)
            {
                normalizedType = NormalizeCompositeType(type as CompositeType);
            }
            if (type is EnumType)
            {
                normalizedType = NormalizeEnumType(type as EnumType);
            }
            if (normalizedType == null)
            {
                throw new NotSupportedException(string.Format("Type {0} is not supported.", type.GetType()));
            }

            _normalizedTypes.Add(normalizedType);
            return normalizedType;
        }

        private IType NormalizeCompositeType(CompositeType compositeType)
        {
            compositeType.SerializedName = compositeType.Name;
            compositeType.Name = GetTypeName(compositeType.Name);

            foreach (var property in compositeType.Properties)
            {
                property.SerializedName = property.Name;
                property.Name = GetPropertyName(property.Name);
                // TODO (gosdk): For now, inherit Enumerated type names from the composite type field name
                if (property.Type is EnumType)
                {
                    var enumType = property.Type as EnumType;
                    if (String.IsNullOrEmpty(enumType.Name))
                    {
                        enumType.Name = property.Name;
                    }
                }
                property.Type = NormalizeType(property.Type);
            }

            return compositeType;
        }

        private IType NormalizeEnumType(EnumType enumType)
        {
            // TODO (gosdk): Default unnamed Enumerated types to "string"
            if (String.IsNullOrEmpty(enumType.Name))
            {
                enumType.Name = "string";
                enumType.SerializedName = "string";
            }
            else
            {
                enumType.SerializedName = enumType.Name;
                enumType.Name = GetTypeName(enumType.Name);
            }

            foreach (var value in enumType.Values)
            {
                value.SerializedName = value.Name;
                value.Name = GetEnumMemberName(value.Name);
            }
            return enumType;
        }

        private IType NormalizePrimaryType(PrimaryType primaryType)
        {
            if (primaryType == PrimaryType.Object)
            {
                DictionaryType dt = new DictionaryType();
                dt.ValueType = PrimaryType.String;

                return NormalizeDictionaryType(dt);
            }
            else
            {
                if (primaryType == PrimaryType.Boolean)
                {
                    primaryType.Name = "bool";
                }
                else if (primaryType == PrimaryType.ByteArray)
                {
                    primaryType.Name = "[]byte";
                }
                else if (primaryType == PrimaryType.Date)
                {
                    return new PackageType { Import = "github.com/azure/go-autorest/autorest/date", Member = "Date" };
                }
                else if (primaryType == PrimaryType.DateTime)
                {
                    return new PackageType { Import = "github.com/azure/go-autorest/autorest/date", Member = "Time" };
                }
                else if (primaryType == PrimaryType.Double)
                {
                    primaryType.Name = "float64";
                }
                else if (primaryType == PrimaryType.Int)
                {
                    primaryType.Name = "int";
                }
                else if (primaryType == PrimaryType.Long)
                {
                    primaryType.Name = "int32";
                }
                else if (primaryType == PrimaryType.Stream)
                {
                    // TODO (gosdk): Add streaming support
                    throw new ArgumentException("Go does not yet support streamed types");
                    //primaryType.Name = "System.IO.Stream";
                }
                else if (primaryType == PrimaryType.String)
                {
                    primaryType.Name = "string";
                }

                return primaryType;
            }
        }

        private IType NormalizeSequenceType(SequenceType sequenceType)
        {
            sequenceType.ElementType = NormalizeType(sequenceType.ElementType);
            sequenceType.NameFormat = "[]{0}";
            return sequenceType;
        }

        private IType NormalizeDictionaryType(DictionaryType dictionaryType)
        {
            dictionaryType.ValueType = NormalizeType(dictionaryType.ValueType);
            dictionaryType.NameFormat = "map[string]{0}";
            return dictionaryType;
        }

        /// <summary>
        /// Formats a string for naming a method using Pascal case by default.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The formatted string.</returns>
        public override string GetMethodName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            return GetEscapedReservedName(PascalCase(RemoveInvalidCharacters(name)), "__");
        }

        /// <summary>
        /// Formats a string for naming method parameters using GetVariableName Camel case by default.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The formatted string.</returns>
        public override string GetParameterName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            return GetEscapedReservedName(CamelCase(RemoveInvalidCharacters(name)), "__");
        }

        /// <summary>
        /// Formats a string for naming properties using Pascal case by default.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The formatted string.</returns>
        public override string GetPropertyName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            return GetEscapedReservedName(PascalCase(RemoveInvalidCharacters(name)), "__");
        }

        /// <summary>
        /// Formats a string for naming a Type or Object using Pascal case by default.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The formatted string.</returns>
        public override string GetTypeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            return GetEscapedReservedName(PascalCase(RemoveInvalidCharacters(name)), "__");
        }

        /// <summary>
        /// Formats a string for naming a local variable using Camel case by default.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The formatted string.</returns>
        public override string GetVariableName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            return GetEscapedReservedName(CamelCase(RemoveInvalidCharacters(name)), "__");
        }

        /// <summary>
        /// Converts names the conflict with Go reserved terms by appending the passed appendValue.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="appendValue">String to append.</param>
        /// <returns>The transformed reserved name</returns>
        protected override string GetEscapedReservedName(string name, string appendValue)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (appendValue == null)
            {
                throw new ArgumentNullException("appendValue");
            }

            // Use case-sensitive comparisons to reduce generated names
            if (ReservedWords.Contains(name, StringComparer.Ordinal))
            {
                name += appendValue;
            }

            return name;
        }

        public void ReserveNamespace(string ns)
        {
            ReservedWords.Add(PackageNameFromNamespace(ns));
        }

        public static string FormatFileName(string fileName)
        {
            return FormatFileName(String.Empty, fileName);
        }

        public static string FormatFileName(string path, string fileName)
        {
            return path + fileName + ".go";
        }

        public static string FormatImportName(string baseImport, string basePackage, params string[] packages)
        {
            List<string> items = new List<string> { baseImport, basePackage };
            packages.ForEach(p => items.Add(FormatPackageName(p)));
            return String.Join("/", items.ToArray<string>());
        }
    
        public static string FormatPackageName(string packageName)
        {
            if (string.IsNullOrWhiteSpace(packageName))
            {
                return packageName;
            }
            return RemoveInvalidCharacters(packageName).ToLowerInvariant();
        }

        public static List<string> NamespaceParts(string ns)
        {
            // -- The namespace is assumed to be the full-path under go/src (e.g., github.com/azure/azure-sdk-for-go/arm/storage)
            // -- Ensure the namespace uses a Go-style (aka Unix) path
            return new List<string>(ns.Replace('\\', '/').Split('/'));
        }

        public static string PackageNameFromNamespace(string ns)
        {
            List<string> namespaceParts = NamespaceParts(ns);
            return namespaceParts[namespaceParts.Count() - 1];
        }
    }
}