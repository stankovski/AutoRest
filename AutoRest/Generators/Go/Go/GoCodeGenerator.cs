// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Rest.Generator;
using Microsoft.Rest.Generator.ClientModel;
using Microsoft.Rest.Generator.Go.Templates;

namespace Microsoft.Rest.Generator.Go
{
    public class GoCodeGenerator : CodeGenerator
    {
        private readonly GoCodeNamer _namingFramework;

        public GoCodeGenerator(Settings settings) : base(settings)
        {
            _namingFramework = new GoCodeNamer();
        }

        public override string Name
        {
            get { return "Go"; }
        }

        public override string Description
        {
            // TODO resource string.
            get { return "Go Client Libraries"; }
        }

        public override string UsageInstructions
        {
            get { return ""; }
        }

        public override string ImplementationFileExtension
        {
            get { return ".go"; }
        }

        /// <summary>
        /// Normalizes client model by updating names and types to be language specific.
        /// </summary>
        /// <param name="serviceClientModel"></param>
        public override void NormalizeClientModel(ServiceClient serviceClientModel)
        {
            // Add the current package name as a reserved keyword
            _namingFramework.ReserveNamespace(Settings.Namespace);

            PopulateAdditionalProperties(serviceClientModel);
            
            _namingFramework.NormalizeClientModel(serviceClientModel);
            _namingFramework.ResolveNameCollisions(serviceClientModel, Settings.Namespace,
                Settings.Namespace + ".Models");
        }

        private void PopulateAdditionalProperties(ServiceClient serviceClientModel)
        {
            //if (Settings.AddCredentials)
            //{
            //    serviceClientModel.Properties.Add(new Property
            //    {
            //        Name = "Credentials",
            //        Type = new CompositeType
            //        {
            //            Name = "ServiceClientCredentials"
            //        },
            //        IsRequired = true,
            //        Documentation = "Subscription credentials which uniquely identify client subscription."
            //    });
            //}
        }

        /// <summary>
        /// Generates Go code for service client.
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <returns></returns>
        public override async Task Generate(ServiceClient serviceClient)
        {
            string packageName = GoCodeNamer.PackageNameFromNamespace(Settings.Namespace);
            
            // Service client
            var serviceClientModel = new ServiceClientTemplateModel(serviceClient, packageName);
            var serviceClientTemplate = new ServiceClientTemplate
            {
                Model = serviceClientModel,
            };
            await Write(serviceClientTemplate, GoCodeNamer.FormatFileName("client"));

            foreach (var groupedMethods in serviceClientModel.GroupedMethods)
            {
                var groupedMethodTemplate = new MethodGroupTemplate
                {
                    Model = groupedMethods,
                };
                await Write(groupedMethodTemplate, GoCodeNamer.FormatFileName(groupedMethods.MethodGroupName.ToLowerInvariant()));
            }


            // Models
            var modelsTemplate = new ModelsTemplate
            {
                Model = new ModelsTemplateModel(serviceClient, packageName),
            };
            await Write(modelsTemplate, GoCodeNamer.FormatFileName("models"));
        }
    }
}
