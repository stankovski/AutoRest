// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Rest.Generator.Go.Templates
{
#line 1 "EnumTemplate.cshtml"
using System;

#line default
#line hidden
#line 2 "EnumTemplate.cshtml"
using System.Collections.Generic;

#line default
#line hidden
#line 3 "EnumTemplate.cshtml"
using System.Linq;

#line default
#line hidden
#line 5 "EnumTemplate.cshtml"
using Microsoft.Rest.Generator.Go

#line default
#line hidden
    ;
#line 6 "EnumTemplate.cshtml"
using Microsoft.Rest.Generator.Go.Templates

#line default
#line hidden
    ;
#line 7 "EnumTemplate.cshtml"
using Microsoft.Rest.Generator.Utilities

#line default
#line hidden
    ;
    using System.Threading.Tasks;

    public class EnumTemplate : Microsoft.Rest.Generator.Go.Template<Microsoft.Rest.Generator.Go.EnumTemplateModel>
    {
        #line hidden
        public EnumTemplate()
        {
        }

        #pragma warning disable 1998
        public override async Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#line 10 "EnumTemplate.cshtml"
  
    var constants = Model.Constants.Keys.OrderBy(v => v);
    var modelName = GoCodeNamer.CamelCase(Model.Name);

#line default
#line hidden

            WriteLiteral("\r\n\r\n");
#line 15 "EnumTemplate.cshtml"
Write(WrapComment("// ", Model.Documentation));

#line default
#line hidden
            WriteLiteral("\r\ntype ");
#line 16 "EnumTemplate.cshtml"
Write(Model.Name);

#line default
#line hidden
            WriteLiteral(" string\r\n");
#line 17 "EnumTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n\r\nconst (\r\n");
#line 20 "EnumTemplate.cshtml"
 foreach (var c in constants)
{

#line default
#line hidden

            WriteLiteral("    ");
#line 22 "EnumTemplate.cshtml"
  Write(c);

#line default
#line hidden
            WriteLiteral(" ");
#line 22 "EnumTemplate.cshtml"
       Write(Model.Name);

#line default
#line hidden
            WriteLiteral(" = \"");
#line 22 "EnumTemplate.cshtml"
                        Write(Model.Constants[c]);

#line default
#line hidden
            WriteLiteral("\"\r\n");
#line 23 "EnumTemplate.cshtml"
}

#line default
#line hidden

            WriteLiteral(")\r\n");
#line 25 "EnumTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n");
        }
        #pragma warning restore 1998
    }
}
