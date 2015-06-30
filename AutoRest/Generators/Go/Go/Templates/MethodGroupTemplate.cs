// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Rest.Generator.Go.Templates
{
#line 1 "MethodGroupTemplate.cshtml"
using System;

#line default
#line hidden
#line 2 "MethodGroupTemplate.cshtml"
using System.Collections.Generic;

#line default
#line hidden
#line 3 "MethodGroupTemplate.cshtml"
using System.Linq;

#line default
#line hidden
#line 5 "MethodGroupTemplate.cshtml"
using Microsoft.Rest.Generator.Go

#line default
#line hidden
    ;
#line 6 "MethodGroupTemplate.cshtml"
using Microsoft.Rest.Generator.Go.Templates

#line default
#line hidden
    ;
#line 7 "MethodGroupTemplate.cshtml"
using Microsoft.Rest.Generator.Utilities

#line default
#line hidden
    ;
    using System.Threading.Tasks;

    public class MethodGroupTemplate : Microsoft.Rest.Generator.Go.Template<Microsoft.Rest.Generator.Go.MethodGroupTemplateModel>
    {
        #line hidden
        public MethodGroupTemplate()
        {
        }

        #pragma warning disable 1998
        public override async Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#line 10 "MethodGroupTemplate.cshtml"
  
    var imports = Model.Imports;
    var modelName = GoCodeNamer.CamelCase(Model.Name);

#line default
#line hidden

            WriteLiteral("\r\n\r\npackage ");
#line 15 "MethodGroupTemplate.cshtml"
   Write(Model.PackageName);

#line default
#line hidden
            WriteLiteral("\r\n");
#line 16 "MethodGroupTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n");
#line 17 "MethodGroupTemplate.cshtml"
Write(Header("// "));

#line default
#line hidden
            WriteLiteral("\r\n\r\n");
#line 19 "MethodGroupTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n\r\nimport (\r\n");
#line 22 "MethodGroupTemplate.cshtml"
 foreach (var import in imports)
{

#line default
#line hidden

            WriteLiteral("    \"");
#line 24 "MethodGroupTemplate.cshtml"
   Write(import);

#line default
#line hidden
            WriteLiteral("\"\r\n");
#line 25 "MethodGroupTemplate.cshtml"
}

#line default
#line hidden

            WriteLiteral(")\r\n");
#line 27 "MethodGroupTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n\r\n");
#line 29 "MethodGroupTemplate.cshtml"
Write(WrapComment("// ", Model.Documentation));

#line default
#line hidden
            WriteLiteral("\r\ntype ");
#line 30 "MethodGroupTemplate.cshtml"
 Write(Model.ClientName);

#line default
#line hidden
            WriteLiteral(" struct {\r\n    ");
#line 31 "MethodGroupTemplate.cshtml"
Write(Model.BaseClient);

#line default
#line hidden
            WriteLiteral("\r\n}\r\n\r\nfunc New");
#line 34 "MethodGroupTemplate.cshtml"
    Write(Model.ClientName);

#line default
#line hidden
            WriteLiteral("(subscriptionId string) ");
#line 34 "MethodGroupTemplate.cshtml"
                                               Write(Model.ClientName);

#line default
#line hidden
            WriteLiteral(" {\r\n    return New");
#line 35 "MethodGroupTemplate.cshtml"
          Write(Model.ClientName);

#line default
#line hidden
            WriteLiteral("WithBaseUri(DefaultBaseUri, subscriptionId)\r\n}\r\n");
#line 37 "MethodGroupTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n\r\nfunc New");
#line 39 "MethodGroupTemplate.cshtml"
    Write(Model.ClientName);

#line default
#line hidden
            WriteLiteral("WithBaseUri(baseUri string, subscriptionId string) ");
#line 39 "MethodGroupTemplate.cshtml"
                                                                          Write(Model.ClientName);

#line default
#line hidden
            WriteLiteral(" {\r\n    return ");
#line 40 "MethodGroupTemplate.cshtml"
       Write(Model.ClientName);

#line default
#line hidden
            WriteLiteral("{NewWithBaseUri(baseUri, subscriptionId)}\r\n}\r\n");
#line 42 "MethodGroupTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n\r\n");
#line 44 "MethodGroupTemplate.cshtml"
 foreach (var method in Model.MethodTemplateModels)
{

#line default
#line hidden

#line 46 "MethodGroupTemplate.cshtml"
Write(Include<MethodTemplate, MethodTemplateModel>(new MethodTemplate(), method));

#line default
#line hidden
            WriteLiteral("\r\n");
#line 47 "MethodGroupTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n");
#line 48 "MethodGroupTemplate.cshtml"
}

#line default
#line hidden

        }
        #pragma warning restore 1998
    }
}
