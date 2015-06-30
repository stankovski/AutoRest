// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

using Microsoft.Rest.Generator.ClientModel;
using Microsoft.Rest.Generator.Go;
using Microsoft.Rest.Generator.Utilities;

namespace Microsoft.Rest.Generator.Go
{
    public class ServiceClientTemplateModel : ServiceClient
    {
        public static readonly List<string> AutorestImports = new List<string> { "github.com/azure/go-autorest/autorest" };
        public static readonly List<string> StandardImports = new List<string> { "net/http" };

        public readonly string ClientName;
        public readonly List<MethodGroupTemplateModel> GroupedMethods;
        public readonly List<MethodTemplateModel> UngroupedMethods;
        public readonly string PackageName;

        public ServiceClientTemplateModel(ServiceClient serviceClient, string packageName)
        {
            this.LoadFrom(serviceClient);

            PackageName = packageName;

            ClientName = Name;
            UngroupedMethods = new List<MethodTemplateModel>();
            Methods.Where(m => m.Group == null)
                .ForEach(m => UngroupedMethods.Add(new MethodTemplateModel(m, ClientName, packageName, new MethodScopeProvider())));

            GroupedMethods = new List<MethodGroupTemplateModel>();
            MethodGroups.ForEach(g => {
                GroupedMethods.Add(new MethodGroupTemplateModel(ClientName, packageName, g, new MethodScopeProvider(), serviceClient));
            });
        }

        public virtual IEnumerable<string> Imports
        {
            get
            {
                var imports = new HashSet<string>();
                imports.UnionWith(AutorestImports);
                var ungroupedMethods = Methods.Where(m => m.Group == null);
                if (ungroupedMethods.Count() > 0)
                {
                    imports.UnionWith(StandardImports);
                    ungroupedMethods
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