// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

using Microsoft.Rest.Generator.ClientModel;
using Microsoft.Rest.Generator.Go;
using Microsoft.Rest.Generator.Utilities;

namespace Microsoft.Rest.Generator.Go
{
    public class ModelsTemplateModel : ServiceClient
    {
        public List<EnumTemplateModel> EnumTemplateModels { get; set; }
        public List<ModelTemplateModel> ModelTemplateModels { get; set; }
        public string PackageName { get; set; }
        
        public ModelsTemplateModel(ServiceClient serviceClient, string packageName)
        {
            this.LoadFrom(serviceClient);

            // Collect enumerated types
            EnumTemplateModels = new List<EnumTemplateModel>();
            foreach (var enumType in EnumTypes)
            {
                // Do not create separate models for "unnamed" Enumerated types
                if (enumType.Name == "string")
                {
                    continue;
                }
                EnumTemplateModels.Add(new EnumTemplateModel(enumType));
            }
            EnumTemplateModels.Sort(delegate(EnumTemplateModel x, EnumTemplateModel y)
            {
                return x.Name.CompareTo(y.Name);
            });

            // Ensure all enumerated type values have the simplest possible unique names
            var enumValues = new HashSet<string>();
            var collisionValues = new HashSet<string>();
            EnumTemplateModels
                .ForEach(em =>
                {
                    em.Values
                        .ForEach(v =>
                        {
                            if (!enumValues.Add(v.Name))
                            {
                                collisionValues.Add(v.Name);
                            }
                        });
                });
            EnumTemplateModels
                .ForEach(em =>
                {
                    if (em.Values.Any(v => collisionValues.Contains(v.Name)))
                    {
                        em.HasUniqueNames = false;
                    }
                });

            // Collect defined models
            ModelTemplateModels = new List<ModelTemplateModel>();
            ModelTypes
                .ForEach(mt =>
                {
                    ModelTemplateModels.Add(new ModelTemplateModel(mt));
                });
            ModelTemplateModels.Sort(delegate(ModelTemplateModel x, ModelTemplateModel y)
            {
                return x.Name.CompareTo(y.Name);
            });

            // Mark all models that "stand alone" (that is, must be defined)
            // -- Any type returned, consumed, or relied on by a method is stand-alone
            ModelTemplateModels
                .Where(mtm =>
                {
                    return Methods.Any(m =>
                        (m.ReturnType != null && (m.ReturnType.Equals(mtm) || m.ReturnType.ReliesOn(mtm)))
                        || m.Parameters.Any(p => p.Type.Equals(mtm) || p.Type.ReliesOn(mtm)));
                })
                .ForEach(mtm =>
                {
                    mtm.IsStandAlone = true;
                });

            // Mark all models returned by one or more methods
            ModelTemplateModels
                .Where(mtm =>
                {
                    return Methods.Any(m => m.ReturnType != null && m.ReturnType.Equals(mtm));
                })
                .ForEach(mtm =>
                {
                    mtm.IsResponseType = true;
                });

            PackageName = packageName;
        }

        public virtual IEnumerable<string> Imports
        {
            get
            {
                // Create an ordered union of the imports each model requires
                var imports = new HashSet<string>();
                imports.Add("github.com/Azure/go-autorest/autorest");
                ModelTypes
                    .ForEach(mt =>
                    {
                        imports.UnionWith((mt as CompositeType).Imports());
                    });
                return imports.OrderBy(i => i);
            }
        }
    }
}
