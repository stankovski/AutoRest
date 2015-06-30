// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Rest.Generator.Go.Templates
{
#line 1 "ModelsTemplate.cshtml"
using System;

#line default
#line hidden
#line 2 "ModelsTemplate.cshtml"
using System.Collections.Generic;

#line default
#line hidden
#line 3 "ModelsTemplate.cshtml"
using System.Linq;

#line default
#line hidden
#line 5 "ModelsTemplate.cshtml"
using Microsoft.Rest.Generator.ClientModel

#line default
#line hidden
    ;
#line 6 "ModelsTemplate.cshtml"
using Microsoft.Rest.Generator.Go

#line default
#line hidden
    ;
#line 7 "ModelsTemplate.cshtml"
using Microsoft.Rest.Generator.Go.Templates

#line default
#line hidden
    ;
#line 8 "ModelsTemplate.cshtml"
using Microsoft.Rest.Generator.Utilities

#line default
#line hidden
    ;
    using System.Threading.Tasks;

    public class ModelsTemplate : Microsoft.Rest.Generator.Go.Template<Microsoft.Rest.Generator.Go.ModelsTemplateModel>
    {
        #line hidden
        public ModelsTemplate()
        {
        }

        #pragma warning disable 1998
        public override async Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#line 11 "ModelsTemplate.cshtml"
  
    var imports = Model.Imports;

#line default
#line hidden

            WriteLiteral("\r\npackage ");
#line 14 "ModelsTemplate.cshtml"
   Write(Model.PackageName);

#line default
#line hidden
            WriteLiteral("\r\n");
#line 15 "ModelsTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n");
#line 16 "ModelsTemplate.cshtml"
Write(Header("// "));

#line default
#line hidden
            WriteLiteral("\r\n\r\n");
#line 18 "ModelsTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
            WriteLiteral("\r\n\r\n");
#line 20 "ModelsTemplate.cshtml"
 if (!imports.IsNullOrEmpty())
{

#line default
#line hidden

            WriteLiteral("import (\r\n");
#line 23 "ModelsTemplate.cshtml"
foreach (var import in imports)
{

#line default
#line hidden

            WriteLiteral("    \"");
#line 25 "ModelsTemplate.cshtml"
   Write(import);

#line default
#line hidden
            WriteLiteral("\"\r\n");
#line 26 "ModelsTemplate.cshtml"
}

#line default
#line hidden

            WriteLiteral(")    \r\n");
#line 28 "ModelsTemplate.cshtml"

#line default
#line hidden

#line 28 "ModelsTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
#line 28 "ModelsTemplate.cshtml"
          
}

#line default
#line hidden

            WriteLiteral("\r\n");
#line 31 "ModelsTemplate.cshtml"
 foreach (var enumTemplateModel in Model.EnumTemplateModels) {

#line default
#line hidden

#line 32 "ModelsTemplate.cshtml"
Write(Include<EnumTemplate, EnumTemplateModel>(new EnumTemplate(), enumTemplateModel));

#line default
#line hidden
            WriteLiteral("\r\n");
#line 33 "ModelsTemplate.cshtml"

#line default
#line hidden

#line 33 "ModelsTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
#line 33 "ModelsTemplate.cshtml"
          

#line default
#line hidden

            WriteLiteral("\r\n");
#line 35 "ModelsTemplate.cshtml"
}

#line default
#line hidden

            WriteLiteral("\r\n");
#line 37 "ModelsTemplate.cshtml"
 foreach (var modelTemplateModel in Model.ModelTemplateModels.Where(mtm => mtm.IsStandAlone)) {

#line default
#line hidden

#line 38 "ModelsTemplate.cshtml"
Write(Include<ModelTemplate, ModelTemplateModel>(new ModelTemplate(), modelTemplateModel));

#line default
#line hidden
            WriteLiteral("\r\n");
#line 39 "ModelsTemplate.cshtml"

#line default
#line hidden

#line 39 "ModelsTemplate.cshtml"
Write(EmptyLine);

#line default
#line hidden
#line 39 "ModelsTemplate.cshtml"
          

#line default
#line hidden

            WriteLiteral("\r\n");
#line 41 "ModelsTemplate.cshtml"
}

#line default
#line hidden

        }
        #pragma warning restore 1998
    }
}
