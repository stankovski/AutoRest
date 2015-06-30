// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Rest.Generator.ClientModel;
using Microsoft.Rest.Generator.Utilities;

namespace Microsoft.Rest.Generator.Go
{
    public class ModelTemplateModel : CompositeType
    {
        private readonly IScopeProvider _scope = new VariableScopeProvider();

        // A "stand alone" type is one that must be publicly defined
        public bool IsStandAlone;

        // True if the type is returned by a method
        public bool IsResponseType;

        public ModelTemplateModel(CompositeType source)
        {
            this.LoadFrom(source);

            PropertyTemplateModels = new List<PropertyTemplateModel>();
            source.Properties.ForEach(p => PropertyTemplateModels.Add(new PropertyTemplateModel(p)));
        }

        public IScopeProvider Scope
        {
            get { return _scope; }
        }

        public virtual IEnumerable<string> Imports
        {
            get
            {
                return (this as CompositeType).Imports();
            }
        }

        public List<PropertyTemplateModel> PropertyTemplateModels { get; private set; }
    }
}