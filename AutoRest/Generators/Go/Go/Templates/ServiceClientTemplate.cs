// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Rest.Generator.Go.Templates
{
#line 1 "ServiceClientTemplate.cshtml"
using System;

#line default
#line hidden
#line 2 "ServiceClientTemplate.cshtml"
using System.Collections.Generic;

#line default
#line hidden
#line 3 "ServiceClientTemplate.cshtml"
using System.Linq;

#line default
#line hidden
#line 5 "ServiceClientTemplate.cshtml"
using Microsoft.Rest.Generator.Go

#line default
#line hidden
    ;
#line 6 "ServiceClientTemplate.cshtml"
using Microsoft.Rest.Generator.Go.Templates

#line default
#line hidden
    ;
#line 7 "ServiceClientTemplate.cshtml"
using Microsoft.Rest.Generator.Utilities

#line default
#line hidden
    ;
    using System.Threading.Tasks;

    public class ServiceClientTemplate : Microsoft.Rest.Generator.Go.Template<Microsoft.Rest.Generator.Go.ServiceClientTemplateModel>
    {
        #line hidden
        public ServiceClientTemplate()
        {
        }

        #pragma warning disable 1998
        public override async Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#line 10 "ServiceClientTemplate.cshtml"
  
    var imports = Model.Imports;
    var modelName = GoCodeNamer.CamelCase(Model.Name);

#line default
#line hidden

            WriteLiteral("\r\npackage ");
#line 14 "ServiceClientTemplate.cshtml"
   Write(Model.PackageName);

#line default
#line hidden
            WriteLiteral("\r\n");
#line 15 "ServiceClientTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n");
#line 16 "ServiceClientTemplate.cshtml"
Write(Header("// "));

#line default
#line hidden
            WriteLiteral("\r\n\r\n");
#line 18 "ServiceClientTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\nimport (\r\n");
#line 20 "ServiceClientTemplate.cshtml"
 foreach (var import in imports)
{

#line default
#line hidden

            WriteLiteral("    \"");
#line 22 "ServiceClientTemplate.cshtml"
   Write(import);

#line default
#line hidden
            WriteLiteral("\"\r\n");
#line 23 "ServiceClientTemplate.cshtml"
}

#line default
#line hidden

            WriteLiteral(")\r\n");
#line 25 "ServiceClientTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n\r\nconst (\r\n    ApiVersion = \"");
#line 28 "ServiceClientTemplate.cshtml"
             Write(Model.ApiVersion);

#line default
#line hidden
            WriteLiteral("\"\r\n    DefaultBaseUri = \"");
#line 29 "ServiceClientTemplate.cshtml"
                 Write(Model.BaseUrl);

#line default
#line hidden
            WriteLiteral("\"\r\n)\r\n");
#line 31 "ServiceClientTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n\r\n");
#line 33 "ServiceClientTemplate.cshtml"
Write(WrapComment("// ", Model.Documentation));

#line default
#line hidden
            WriteLiteral("\r\ntype ");
#line 34 "ServiceClientTemplate.cshtml"
 Write(Model.ClientName);

#line default
#line hidden
            WriteLiteral(" struct {\r\n    autorest.Client\r\n    BaseUri string\r\n    SubscriptionId string\r\n}\r" +
"\n");
#line 39 "ServiceClientTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n\r\nfunc New(subscriptionId string) ");
#line 41 "ServiceClientTemplate.cshtml"
                            Write(Model.ClientName);

#line default
#line hidden
            WriteLiteral(" {\r\n    return NewWithBaseUri(DefaultBaseUri, subscriptionId)\r\n}\r\n");
#line 44 "ServiceClientTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n\r\nfunc NewWithBaseUri(baseUri string, subscriptionId string) ");
#line 46 "ServiceClientTemplate.cshtml"
                                                       Write(Model.ClientName);

#line default
#line hidden
            WriteLiteral(" {\r\n    return ");
#line 47 "ServiceClientTemplate.cshtml"
       Write(Model.ClientName);

#line default
#line hidden
            WriteLiteral("{\r\n        Client: autorest.DefaultClient,\r\n        BaseUri: baseUri,\r\n        Su" +
"bscriptionId: subscriptionId,\r\n    }\r\n}\r\n");
#line 53 "ServiceClientTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n\r\n");
#line 55 "ServiceClientTemplate.cshtml"
 foreach (var method in Model.UngroupedMethods)
{

#line default
#line hidden

#line 57 "ServiceClientTemplate.cshtml"
Write(Include<MethodTemplate, MethodTemplateModel>(new MethodTemplate(), method));

#line default
#line hidden
            WriteLiteral("\r\n");
#line 58 "ServiceClientTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n");
#line 59 "ServiceClientTemplate.cshtml"
}

#line default
#line hidden

        }
        #pragma warning restore 1998
    }
}
