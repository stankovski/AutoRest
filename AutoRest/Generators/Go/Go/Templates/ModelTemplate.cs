// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Rest.Generator.Go.Templates
{
#line 1 "ModelTemplate.cshtml"
using System;

#line default
#line hidden
#line 2 "ModelTemplate.cshtml"
using System.Collections.Generic;

#line default
#line hidden
#line 3 "ModelTemplate.cshtml"
using System.Linq;

#line default
#line hidden
#line 5 "ModelTemplate.cshtml"
using Microsoft.Rest.Generator.Go

#line default
#line hidden
    ;
#line 6 "ModelTemplate.cshtml"
using Microsoft.Rest.Generator.Go.Templates

#line default
#line hidden
    ;
#line 7 "ModelTemplate.cshtml"
using Microsoft.Rest.Generator.Utilities

#line default
#line hidden
    ;
    using System.Threading.Tasks;

    public class ModelTemplate : Microsoft.Rest.Generator.Go.Template<Microsoft.Rest.Generator.Go.ModelTemplateModel>
    {
        #line hidden
        public ModelTemplate()
        {
        }

        #pragma warning disable 1998
        public override async Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#line 10 "ModelTemplate.cshtml"
  
    var modelName = GoCodeNamer.CamelCase(Model.Name);

#line default
#line hidden

            WriteLiteral("\r\n\r\n");
#line 14 "ModelTemplate.cshtml"
Write(WrapComment("// ", Model.Documentation));

#line default
#line hidden
            WriteLiteral("\r\ntype ");
#line 15 "ModelTemplate.cshtml"
Write(Model.Name);

#line default
#line hidden
            WriteLiteral(" struct {\r\n");
#line 16 "ModelTemplate.cshtml"
 if (Model.IsResponseType)
{

#line default
#line hidden

            WriteLiteral("    autorest.Response `json:\"-\"`\r\n");
#line 19 "ModelTemplate.cshtml"
}

#line default
#line hidden

            WriteLiteral("    ");
#line 20 "ModelTemplate.cshtml"
Write(Model.Fields());

#line default
#line hidden
            WriteLiteral("\r\n}\r\n");
        }
        #pragma warning restore 1998
    }
}
