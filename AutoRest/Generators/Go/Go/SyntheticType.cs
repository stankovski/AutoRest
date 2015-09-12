// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

using Microsoft.Rest.Generator.ClientModel;

namespace Microsoft.Rest.Generator.Go
{
    /// <summary>
    /// Defines a synthetic type used to hold an array or dictionary method response.
    /// </summary>
    public class SyntheticType : CompositeType
    {
        public SyntheticType(IType collectionType)
        {
            if (!(collectionType is SequenceType) && !(collectionType is DictionaryType))
            {
                throw new ArgumentException("{0} is not a valid colleciton type for SyntheticType", collectionType.ToString());
            }

            // TODO (gosdk): Ensure the generated name does not collide with existing type names
            Name = (collectionType is SequenceType)
                    ? (collectionType as SequenceType).ElementType.Name + "List"
                    : (collectionType as DictionaryType).ValueType.Name + "Set";
            
            Property p = new Property();
            p.SerializedName = "value";
            p.Name = "Value";
            p.Type = collectionType;
            
            Properties.Add(p);
        }
    }
}
