// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

using Microsoft.Rest.Generator.ClientModel;
using Microsoft.Rest.Generator.Utilities;

namespace Microsoft.Rest.Generator.Go
{
    public class EnumTemplateModel : EnumType
    {
        public bool HasUniqueNames { get; set; }
        
        public EnumTemplateModel(EnumType source)
        {
            this.LoadFrom(source);

            // Assume members have unique names
            HasUniqueNames = true;
        }

        public IDictionary<string, string> Constants
        {
            get
            {
                var constants = new Dictionary<string, string>();
                Values
                    .ForEach(v =>
                    {
                        constants.Add(HasUniqueNames ? v.Name : Name + v.Name, v.SerializedName);
                    });

                return constants;
            }
        }

        public string Documentation { get; set; }
    }
}
