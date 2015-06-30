// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Rest.Generator.Go.Templates
{
#line 1 "MethodTemplate.cshtml"
using System;

#line default
#line hidden
#line 2 "MethodTemplate.cshtml"
using System.Collections;

#line default
#line hidden
#line 3 "MethodTemplate.cshtml"
using System.Collections.Generic;

#line default
#line hidden
#line 4 "MethodTemplate.cshtml"
using System.Linq;

#line default
#line hidden
#line 5 "MethodTemplate.cshtml"
using System.Net;

#line default
#line hidden
#line 6 "MethodTemplate.cshtml"
using System.Text;

#line default
#line hidden
#line 8 "MethodTemplate.cshtml"
using Microsoft.Rest.Generator.ClientModel

#line default
#line hidden
    ;
#line 9 "MethodTemplate.cshtml"
using Microsoft.Rest.Generator.Go

#line default
#line hidden
    ;
    using System.Threading.Tasks;

    public class MethodTemplate : Microsoft.Rest.Generator.Go.Template<Microsoft.Rest.Generator.Go.MethodTemplateModel>
    {
        #line hidden
        public MethodTemplate()
        {
        }

        #pragma warning disable 1998
        public override async Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#line 12 "MethodTemplate.cshtml"
  
    StringBuilder sb = new StringBuilder();
    foreach (var parameter in Model.LocalParameters)
    {
        if (!string.IsNullOrEmpty(parameter.Documentation))
        {
            sb.Append(parameter.Name);
            sb.Append(" is ");
            sb.Append(parameter.Documentation.ToSentence());
            sb.Append(" ");
        }
    }

    List<string> pd = Model.PrepareDecorators;
    pd.Insert(0, "req");
    List<string> rd = Model.RespondDecorators;
    rd.Insert(0, "resp");
    
    // TODO (gosdk): Detect and better handle long running operations

#line default
#line hidden

            WriteLiteral("\r\n\r\n");
#line 33 "MethodTemplate.cshtml"
Write(WrapComment("// ", Model.ScopedName + " " + Model.Documentation.ToSentence()));

#line default
#line hidden
            WriteLiteral("\r\n");
#line 34 "MethodTemplate.cshtml"
 if (Model.LocalParameters.Count() > 0)
{

#line default
#line hidden

            WriteLiteral("//\r\n");
#line 37 "MethodTemplate.cshtml"

#line default
#line hidden

#line 37 "MethodTemplate.cshtml"
Write(WrapComment("// ", sb.ToString()));

#line default
#line hidden
#line 37 "MethodTemplate.cshtml"
                                  
}

#line default
#line hidden

            WriteLiteral("\r\nfunc (client ");
#line 40 "MethodTemplate.cshtml"
         Write(Model.Owner);

#line default
#line hidden
            WriteLiteral(") ");
#line 40 "MethodTemplate.cshtml"
                         Write(Model.MethodSignature);

#line default
#line hidden
            WriteLiteral(" (");
#line 40 "MethodTemplate.cshtml"
                                                  Write(Model.ReturnSignature);

#line default
#line hidden
            WriteLiteral(") {\r\n    req, err := client.");
#line 41 "MethodTemplate.cshtml"
                   Write(Model.RequestMethodName);

#line default
#line hidden
            WriteLiteral("(");
#line 41 "MethodTemplate.cshtml"
                                              Write(Model.HelperInvocationParameters);

#line default
#line hidden
            WriteLiteral(")\r\n    if err != nil {\r\n        return result, ");
#line 43 "MethodTemplate.cshtml"
                   Write(Model.AutorestError("creating"));

#line default
#line hidden
            WriteLiteral("\r\n    }\r\n\r\n    ");
#line 46 "MethodTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n    req, err = autorest.Prepare(\r\n        ");
#line 48 "MethodTemplate.cshtml"
    Write(pd.EmitAsArguments());

#line default
#line hidden
            WriteLiteral("\r\n    if err != nil {\r\n        return result, ");
#line 50 "MethodTemplate.cshtml"
                   Write(Model.AutorestError("preparing"));

#line default
#line hidden
            WriteLiteral("\r\n    }\r\n\r\n    ");
#line 53 "MethodTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n    resp, err := autorest.SendWithSender(client, req)\r\n");
#line 55 "MethodTemplate.cshtml"
 if (Model.Responses.ContainsKey(HttpStatusCode.Accepted))
{

#line default
#line hidden

            WriteLiteral("    if err == nil {\r\n        err = client.ShouldPoll(resp)\r\n        if err == nil" +
" {\r\n            resp, err = client.PollAsNeeded(resp)\r\n        }\r\n    }\r\n");
#line 63 "MethodTemplate.cshtml"
}

#line default
#line hidden

            WriteLiteral("\r\n    ");
#line 65 "MethodTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n    if err == nil {\r\n        err = autorest.Respond(\r\n            ");
#line 68 "MethodTemplate.cshtml"
        Write(rd.EmitAsArguments());

#line default
#line hidden
            WriteLiteral("\r\n        if err != nil {\r\n            ae = ");
#line 70 "MethodTemplate.cshtml"
             Write(Model.AutorestError("responding to"));

#line default
#line hidden
            WriteLiteral("\r\n        }\r\n    } else {\r\n        ae = ");
#line 73 "MethodTemplate.cshtml"
         Write(Model.AutorestError("sending"));

#line default
#line hidden
            WriteLiteral("\r\n    }\r\n\r\n    ");
#line 76 "MethodTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n    autorest.Respond(resp,\r\n        autorest.ByClosing())\r\n");
#line 79 "MethodTemplate.cshtml"
 if (Model.HasReturnValue) {

#line default
#line hidden

            WriteLiteral("    result.Response = autorest.Response{Response: resp}\r\n");
#line 81 "MethodTemplate.cshtml"
} else {

#line default
#line hidden

            WriteLiteral("    result.Response = resp\r\n");
#line 83 "MethodTemplate.cshtml"
}

#line default
#line hidden

            WriteLiteral("\r\n    ");
#line 85 "MethodTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n    return\r\n}\r\n\r\n");
#line 89 "MethodTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n// Create the ");
#line 90 "MethodTemplate.cshtml"
          Write(Model.ScopedName);

#line default
#line hidden
            WriteLiteral(" request.\r\nfunc (client ");
#line 91 "MethodTemplate.cshtml"
         Write(Model.Owner);

#line default
#line hidden
            WriteLiteral(") ");
#line 91 "MethodTemplate.cshtml"
                         Write(Model.RequestMethodName);

#line default
#line hidden
            WriteLiteral("(");
#line 91 "MethodTemplate.cshtml"
                                                    Write(Model.MethodParametersSignature);

#line default
#line hidden
            WriteLiteral(") (*http.Request, error) {\r\n");
#line 92 "MethodTemplate.cshtml"
 if (Model.PathParameters.Count() > 0)
{

#line default
#line hidden

            WriteLiteral("    ");
#line 94 "MethodTemplate.cshtml"
  Write(Model.PathMap);

#line default
#line hidden
            WriteLiteral("\r\n    ");
#line 95 "MethodTemplate.cshtml"
 Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n");
#line 96 "MethodTemplate.cshtml"
}

#line default
#line hidden

#line 97 "MethodTemplate.cshtml"
 if (Model.QueryParameters.Count() > 0)
{

#line default
#line hidden

            WriteLiteral("    ");
#line 99 "MethodTemplate.cshtml"
  Write(Model.QueryMap);

#line default
#line hidden
            WriteLiteral("\r\n    ");
#line 100 "MethodTemplate.cshtml"
 Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n");
#line 101 "MethodTemplate.cshtml"
}

#line default
#line hidden

            WriteLiteral("    return autorest.DecoratePreparer(\r\n        ");
#line 103 "MethodTemplate.cshtml"
    Write(Model.RequestDecorators.EmitAsArguments());

#line default
#line hidden
            WriteLiteral(".Prepare(&http.Request{})\r\n}\r\n\r\n");
#line 106 "MethodTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n// Create a Preparer by which to prepare the ");
#line 107 "MethodTemplate.cshtml"
                                         Write(Model.ScopedName);

#line default
#line hidden
            WriteLiteral(" request.\r\nfunc (client ");
#line 108 "MethodTemplate.cshtml"
         Write(Model.Owner);

#line default
#line hidden
            WriteLiteral(") ");
#line 108 "MethodTemplate.cshtml"
                         Write(Model.PreparerMethodName);

#line default
#line hidden
            WriteLiteral("() autorest.Preparer {\r\n    return autorest.CreatePreparer(\r\n        autorest.AsJ" +
"SON(),\r\n        ");
#line 111 "MethodTemplate.cshtml"
    Write(Model.HTTPMethodDecorator);

#line default
#line hidden
            WriteLiteral(",\r\n        autorest.WithBaseURL(client.BaseUri),\r\n        autorest.WithPath(\"");
#line 113 "MethodTemplate.cshtml"
                       Write(Model.Url);

#line default
#line hidden
            WriteLiteral("\"))\r\n}\r\n");
        }
        #pragma warning restore 1998
    }
}
