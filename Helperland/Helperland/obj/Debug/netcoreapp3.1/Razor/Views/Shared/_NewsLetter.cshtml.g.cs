#pragma checksum "C:\Users\keval\Downloads\New folder (5)\helperland-master\helperland\Helperland\Views\Shared\_NewsLetter.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "80fb02f3f5bbd75dd7e16f76b452b8b7e35d2b70"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__NewsLetter), @"mvc.1.0.view", @"/Views/Shared/_NewsLetter.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\keval\Downloads\New folder (5)\helperland-master\helperland\Helperland\Views\_ViewImports.cshtml"
using Helperland;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\keval\Downloads\New folder (5)\helperland-master\helperland\Helperland\Views\_ViewImports.cshtml"
using Helperland.Core;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\keval\Downloads\New folder (5)\helperland-master\helperland\Helperland\Views\_ViewImports.cshtml"
using Helperland.Enums;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\keval\Downloads\New folder (5)\helperland-master\helperland\Helperland\Views\_ViewImports.cshtml"
using Helperland.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\keval\Downloads\New folder (5)\helperland-master\helperland\Helperland\Views\_ViewImports.cshtml"
using Helperland.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\keval\Downloads\New folder (5)\helperland-master\helperland\Helperland\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\keval\Downloads\New folder (5)\helperland-master\helperland\Helperland\Views\_ViewImports.cshtml"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"80fb02f3f5bbd75dd7e16f76b452b8b7e35d2b70", @"/Views/Shared/_NewsLetter.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7898f82f3c4ec8ef7012c9ce577661f7fb4dcfaf", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__NewsLetter : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<section");
            BeginWriteAttribute("class", " class=\"", 8, "\"", 78, 2);
            WriteAttributeValue("", 16, "pt-0", 16, 4, true);
#nullable restore
#line 1 "C:\Users\keval\Downloads\New folder (5)\helperland-master\helperland\Helperland\Views\Shared\_NewsLetter.cshtml"
WriteAttributeValue(" ", 20,  ViewBag.page == "HomePage" ? "bg-color-pal-grey" : "", 21, 57, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">
    <div class=""newsletter"">
        <div class=""text-center mb-4"">
            <h4>GET OUR NEWSLETTER</h4>
        </div>
        <div class=""text-center"">
            <input type=""text"" name=""email"" placeholder=""YOUR EMAIL"" class=""mb-3"">
            <button type=""submit"" class=""ms-2"">Submit</button>
        </div>
    </div>
</section>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
