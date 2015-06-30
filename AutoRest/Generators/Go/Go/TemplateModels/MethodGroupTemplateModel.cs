// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Rest.Generator.ClientModel;
using Microsoft.Rest.Generator.Go;
using Microsoft.Rest.Generator.Utilities;

namespace Microsoft.Rest.Generator.Go
{
    public class MethodGroupTemplateModel : ServiceClient
    {
        public readonly string BaseClient;
        public readonly string ClientName;
        public readonly string PackageName;
        public readonly string MethodGroupName;
        public readonly MethodScopeProvider MethodScope;
        public readonly List<MethodTemplateModel> MethodTemplateModels;

        public MethodGroupTemplateModel(string baseClient, string packageName, string methodGroupName, MethodScopeProvider methodScope, ServiceClient serviceClient)
        {
            this.LoadFrom(serviceClient);

            Documentation = methodGroupName + " Client";

            BaseClient = baseClient;
            ClientName = methodGroupName + "Client";
            PackageName = packageName;
            MethodGroupName = methodGroupName;
            MethodScope = methodScope;

            MethodTemplateModels = new List<MethodTemplateModel>();
            Methods.Where(m => m.Group == MethodGroupName)
                .ForEach(m => MethodTemplateModels.Add(new MethodTemplateModel(m, ClientName, packageName, methodScope)));
        }

        public virtual IEnumerable<string> Imports
        {
            get
            {
                // Import referenced package types and required standard types
                var imports = new HashSet<string>();
                imports.UnionWith(ServiceClientTemplateModel.AutorestImports);
                if (Methods.Count() > 0)
                {
                    imports.UnionWith(ServiceClientTemplateModel.StandardImports);
                    Methods
                        .ForEach(m =>
                        {
                            m.Parameters
                                .ForEach(p =>
                                {
                                    if (p.Type is PackageType)
                                    {
                                        imports.Add((p.Type as PackageType).Import);
                                    }
                                    if (p.RequiresUrlEncoding())
                                    {
                                        imports.Add("net/url");
                                    }
                                });
                            var bodyParameters = m.Parameters.Where(p => p.Location == ParameterLocation.Body);
                            if (m.ReturnType is PackageType)
                            {
                                imports.Add((m.ReturnType as PackageType).Import);
                            }
                        });
                }
                return imports.OrderBy(i => i);
            }
        }
    }
}