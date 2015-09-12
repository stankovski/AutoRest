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
    List<string> sd = Model.SendDecorators;
    sd.Insert(0, "req");
    sd.Insert(0, "client");
    List<string> rd = Model.RespondDecorators;
    rd.Insert(0, "resp");
    
    // TODO (gosdk): Detect and better handle long running operations

#line default
#line hidden

            WriteLiteral("\r\n\r\n");
#line 36 "MethodTemplate.cshtml"
Write(WrapComment("// ", Model.ScopedName + " " + Model.Documentation.ToSentence()));

#line default
#line hidden
            WriteLiteral("\r\n");
#line 37 "MethodTemplate.cshtml"
 if (Model.LocalParameters.Count() > 0)
{

#line default
#line hidden

            WriteLiteral("//\r\n");
#line 40 "MethodTemplate.cshtml"

#line default
#line hidden

#line 40 "MethodTemplate.cshtml"
Write(WrapComment("// ", sb.ToString()));

#line default
#line hidden
#line 40 "MethodTemplate.cshtml"
                                  
}

#line default
#line hidden

            WriteLiteral("\r\nfunc (client ");
#line 43 "MethodTemplate.cshtml"
         Write(Model.Owner);

#line default
#line hidden
            WriteLiteral(") ");
#line 43 "MethodTemplate.cshtml"
                         Write(Model.MethodSignature);

#line default
#line hidden
            WriteLiteral(" (");
#line 43 "MethodTemplate.cshtml"
                                                  Write(Model.ReturnSignature);

#line default
#line hidden
            WriteLiteral(") {\r\n    req, err := client.");
#line 44 "MethodTemplate.cshtml"
                   Write(Model.RequestMethodName);

#line default
#line hidden
            WriteLiteral("(");
#line 44 "MethodTemplate.cshtml"
                                              Write(Model.HelperInvocationParameters);

#line default
#line hidden
            WriteLiteral(")\r\n    if err != nil {\r\n        return result, ");
#line 46 "MethodTemplate.cshtml"
                   Write(Model.AutorestError("creating"));

#line default
#line hidden
            WriteLiteral("\r\n    }\r\n\r\n    ");
#line 49 "MethodTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n    req, err = autorest.Prepare(\r\n        ");
#line 51 "MethodTemplate.cshtml"
    Write(pd.EmitAsArguments());

#line default
#line hidden
            WriteLiteral("\r\n    if err != nil {\r\n        return result, ");
#line 53 "MethodTemplate.cshtml"
                   Write(Model.AutorestError("preparing"));

#line default
#line hidden
            WriteLiteral("\r\n    }\r\n\r\n    ");
#line 56 "MethodTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n    resp, err := autorest.SendWithSender(\r\n        ");
#line 58 "MethodTemplate.cshtml"
    Write(sd.EmitAsArguments());

#line default
#line hidden
            WriteLiteral("\r\n");
#line 59 "MethodTemplate.cshtml"
 if (Model.Responses.ContainsKey(HttpStatusCode.Accepted))
{

#line default
#line hidden

            WriteLiteral("    if err == nil {\r\n        err = client.IsPollingAllowed(resp)\r\n        if err " +
"== nil {\r\n            resp, err = client.PollAsNeeded(resp)\r\n        }\r\n    }\r\n");
#line 67 "MethodTemplate.cshtml"
}

#line default
#line hidden

            WriteLiteral("\r\n    ");
#line 69 "MethodTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n    if err == nil {\r\n        err = autorest.Respond(\r\n            ");
#line 72 "MethodTemplate.cshtml"
        Write(rd.EmitAsArguments());

#line default
#line hidden
            WriteLiteral("\r\n        if err != nil {\r\n            ae = ");
#line 74 "MethodTemplate.cshtml"
             Write(Model.AutorestError("responding to"));

#line default
#line hidden
            WriteLiteral("\r\n        }\r\n    } else {\r\n        ae = ");
#line 77 "MethodTemplate.cshtml"
         Write(Model.AutorestError("sending"));

#line default
#line hidden
            WriteLiteral("\r\n    }\r\n\r\n    ");
#line 80 "MethodTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n    autorest.Respond(resp,\r\n        autorest.ByClosing())\r\n");
#line 83 "MethodTemplate.cshtml"
 if (Model.HasReturnValue) {

#line default
#line hidden

            WriteLiteral("    result.Response = autorest.Response{Response: resp}\r\n");
#line 85 "MethodTemplate.cshtml"
} else {

#line default
#line hidden

            WriteLiteral("    result.Response = resp\r\n");
#line 87 "MethodTemplate.cshtml"
}

#line default
#line hidden

            WriteLiteral("\r\n    ");
#line 89 "MethodTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n    return\r\n}\r\n\r\n");
#line 93 "MethodTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n// Create the ");
#line 94 "MethodTemplate.cshtml"
          Write(Model.ScopedName);

#line default
#line hidden
            WriteLiteral(" request.\r\nfunc (client ");
#line 95 "MethodTemplate.cshtml"
         Write(Model.Owner);

#line default
#line hidden
            WriteLiteral(") ");
#line 95 "MethodTemplate.cshtml"
                         Write(Model.RequestMethodName);

#line default
#line hidden
            WriteLiteral("(");
#line 95 "MethodTemplate.cshtml"
                                                    Write(Model.MethodParametersSignature);

#line default
#line hidden
            WriteLiteral(") (*http.Request, error) {\r\n");
#line 96 "MethodTemplate.cshtml"
 if (Model.PathParameters.Count() > 0)
{

#line default
#line hidden

            WriteLiteral("    ");
#line 98 "MethodTemplate.cshtml"
  Write(Model.PathMap);

#line default
#line hidden
            WriteLiteral("\r\n    ");
#line 99 "MethodTemplate.cshtml"
 Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n");
#line 100 "MethodTemplate.cshtml"
}

#line default
#line hidden

#line 101 "MethodTemplate.cshtml"
 if (Model.QueryParameters.Count() > 0)
{

#line default
#line hidden

            WriteLiteral("    ");
#line 103 "MethodTemplate.cshtml"
  Write(Model.QueryMap);

#line default
#line hidden
            WriteLiteral("\r\n    ");
#line 104 "MethodTemplate.cshtml"
 Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n");
#line 105 "MethodTemplate.cshtml"
}

#line default
#line hidden

            WriteLiteral("    return autorest.DecoratePreparer(\r\n        ");
#line 107 "MethodTemplate.cshtml"
    Write(Model.RequestDecorators.EmitAsArguments());

#line default
#line hidden
            WriteLiteral(".Prepare(&http.Request{})\r\n}\r\n\r\n");
#line 110 "MethodTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n// Create a Preparer by which to prepare the ");
#line 111 "MethodTemplate.cshtml"
                                         Write(Model.ScopedName);

#line default
#line hidden
            WriteLiteral(" request.\r\nfunc (client ");
#line 112 "MethodTemplate.cshtml"
         Write(Model.Owner);

#line default
#line hidden
            WriteLiteral(") ");
#line 112 "MethodTemplate.cshtml"
                         Write(Model.PreparerMethodName);

#line default
#line hidden
            WriteLiteral("() autorest.Preparer {\r\n    return autorest.CreatePreparer(\r\n        autorest.AsJ" +
"SON(),\r\n        ");
#line 115 "MethodTemplate.cshtml"
    Write(Model.HTTPMethodDecorator);

#line default
#line hidden
            WriteLiteral(",\r\n        autorest.WithBaseURL(client.BaseUri),\r\n        autorest.WithPath(\"");
#line 117 "MethodTemplate.cshtml"
                       Write(Model.Url);

#line default
#line hidden
            WriteLiteral("\"))\r\n}\r\n");
        }
        #pragma warning restore 1998
    }
}
