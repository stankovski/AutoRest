// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Microsoft.Rest.Generator.ClientModel;
using Microsoft.Rest.Generator.Go;
using Microsoft.Rest.Generator.Utilities;

namespace Microsoft.Rest.Generator.Go
{
    public class MethodTemplateModel : Method
    {
        public readonly MethodScopeProvider MethodScope;
        public readonly string Owner;
        public readonly string PackageName;

        public MethodTemplateModel(Method source, string owner, string packageName, MethodScopeProvider methodScope)
        {
            this.LoadFrom(source);
            
            MethodScope = methodScope;
            Owner = owner;
            PackageName = packageName;
        }

        private string _scopedName;
        public string ScopedName
        {
            get
            {
                if (string.IsNullOrEmpty(_scopedName))
                {
                    _scopedName = MethodScope.GetMethodName(Name, Group);
                }
                return _scopedName;
            }
        }
        
        public string MethodSignature
        {
            get
            {
                return ScopedName + "(" + MethodParametersSignature + ")";
            }
        }

        /// <summary>
        /// Generate the method parameter declaration.
        /// </summary>
        public string MethodParametersSignature
        {
            get
            {
                List<string> declarations = new List<string>();
                LocalParameters
                    .ForEach(p => declarations.Add(string.Format("{0} {1}", p.Name, p.Type.Name)));

                return string.Join(", ", declarations);
            }
        }

        /// <summary>
        /// Returns the name of the method to create the HTTP Request object.
        /// </summary>
        public string RequestMethodName
        {
            get
            {
                return "New" + ScopedName + "Request";
            }
        }

        public string PreparerMethodName
        {
            get
            {
                return ScopedName + "RequestPreparer";
            }
        }

        public string HelperInvocationParameters
        {
            get
            {
                List<string> invocationParams = new List<string>();
                LocalParameters
                    .ForEach(p => invocationParams.Add(p.Name));
                return string.Join(", ", invocationParams);
            }
        }
        
        public string ReturnSignature
        {
            get
            {
                return HasReturnValue
                    ? string.Format("result {0}, ae autorest.Error", ReturnType.Name)
                    : "result autorest.Response, ae autorest.Error";
            }
        }

        /// <summary>
        /// Return the parameters as they apopear in the method signature excluding global parameters.
        /// </summary>
        public IEnumerable<Parameter> LocalParameters
        {
            get
            {
                return
                    Parameters.Where(
                        p => p != null && p.IsMethodArgument() && !string.IsNullOrWhiteSpace(p.Name))
                        .OrderBy(item => !item.IsRequired);
            }
        }

        public IEnumerable<Parameter> BodyParameters
        {
            get
            {
                return Parameters.BodyParameters();
            }
        }

        public IEnumerable<Parameter> FormDataParameters
        {
            get
            {
                return Parameters.FormDataParameters();
            }
        }

        public IEnumerable<Parameter> HeaderParameters
        {
            get
            {
                return Parameters.HeaderParameters();
            }
        }

        public IEnumerable<Parameter> PathParameters
        {
            get
            {
                return Parameters.PathParameters();
            }
        }

        public string PathMap
        {
            get
            {
                return PathParameters.BuildParameterMap("pathParameters", true);
            }
        }

        public IEnumerable<Parameter> QueryParameters
        {
            get
            {
                return Parameters.QueryParameters();
            }
        }

        public string QueryMap
        {
            get
            {
                return QueryParameters.BuildParameterMap("queryParameters");
            }
        }

        public List<string> PrepareDecorators
        {
            get
            {
                var decorators = new List<string>();
                decorators.Add("client.WithAuthorization()");
                decorators.Add("client.WithInspection()");
                return decorators;
            }
        }

        public List<string> RequestDecorators
        {
            get
            {
                var decorators = new List<string>();
                decorators.Add(string.Format("client.{0}()", PreparerMethodName));
                if (BodyParameters.Count() > 0)
                {
                    decorators.Add(string.Format("autorest.WithJSON({0})", BodyParameters.First().SerializedName));
                }
                if (PathParameters.Count() > 0)
                {
                    decorators.Add("autorest.WithPathParameters(pathParameters)");
                }
                if (QueryParameters.Count() > 0)
                {
                    decorators.Add("autorest.WithQueryParameters(queryParameters)");
                }
                return decorators;
            }
        }

        public string HTTPMethodDecorator
        {
            get
            {
                switch (HttpMethod) {
                    case HttpMethod.Delete: return "autorest.AsDelete()";
                    case HttpMethod.Get: return "autorest.AsGet()";
                    case HttpMethod.Head: return "autorest.AsHead()";
                    case HttpMethod.Options: return "autorest.AsOptions()";
                    case HttpMethod.Patch: return "autorest.AsPatch()";
                    case HttpMethod.Post: return "autorest.AsPost()";
                    case HttpMethod.Put: return "autorest.AsPut()";
                    default:
                        throw new ArgumentException(string.Format("The HTTP verb {0} is not supported by the Go SDK", HttpMethod));
                }
            }
        }

        public List<string> RespondDecorators
        {
            get
            {
                var decorators = new List<string>();
                decorators.Add("client.ByInspecting()");
                decorators.Add("autorest.WithErrorUnlessOK()");
                if (HasReturnValue)
                {
                    if (ReturnType is SyntheticType)
                    {
                        decorators.Add("autorest.ByUnmarshallingJSON(&result.Value)");
                    }
                    else
                    {
                        decorators.Add("autorest.ByUnmarshallingJSON(&result)");
                    }
                }
                return decorators;
            }
        }

        public string AutorestError(string phase)
        {
            return string.Format("autorest.NewErrorWithError(err, \"{0}.{1}\", \"{2}\", \"Failure {3} request\")", PackageName, Owner, ScopedName, phase);
        }

        public bool HasReturnValue
        {
            get
            {
                return (ReturnType != null);
            }
        }
    }
}
