#pragma checksum "C:\Users\keval\Downloads\Task\Helperland\Helperland\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "886cf9fc6c01136769bbc397958786ecedfdfd93"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
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
#line 1 "C:\Users\keval\Downloads\Task\Helperland\Helperland\Views\_ViewImports.cshtml"
using Helperland;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\keval\Downloads\Task\Helperland\Helperland\Views\_ViewImports.cshtml"
using Helperland.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"886cf9fc6c01136769bbc397958786ecedfdfd93", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b5f94cf04a7ec23f27ac33992ef127038e0b3154", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Home", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("nav-link"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Price", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Contact_Us", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Service_Provider", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\keval\Downloads\Task\Helperland\Helperland\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "886cf9fc6c01136769bbc397958786ecedfdfd935260", async() => {
                WriteLiteral("\r\n    <section>\r\n        <div class=\"back_img\">\r\n            <nav class=\"navbar navbar-expand-xl navbar-dark nav_bar\">\r\n                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "886cf9fc6c01136769bbc397958786ecedfdfd935662", async() => {
                    WriteLiteral("<img src=\"./img/logo-small.png\" class=\"main_logo\">");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
                <button class=""navbar-toggler"" type=""button"" data-toggle=""collapse"" data-target=""#collapsibleNavbar"">
                    <span class=""navbar-toggler-icon""></span>
                </button>
                <div class=""collapse navbar-collapse"" id=""collapsibleNavbar"">
                    <ul class=""navbar-nav ml-auto"" style=""align-items: center;"">
                        <li class=""nav-item book_now"">
                            <a class=""nav-link book_now_text"" href=""#""><span class=""book_now_text"">Book a Clear</span></a>
                        </li>
                        <li class=""nav-item prices"">
                            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "886cf9fc6c01136769bbc397958786ecedfdfd937833", async() => {
                    WriteLiteral("<span class=\"book_now_text\">Prices</span>");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_3.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
                        </li>
                        <li class=""nav-item"">
                            <a class=""nav-link"" href=""#""><span class=""book_now_text other_btn"">Our Guarantee</span></a>
                        </li>
                        <li class=""nav-item"">
                            <a class=""nav-link"" href=""#""><span class=""book_now_text other_btn"">Blog</span></a>
                        </li>
                        <li class=""nav-item"">
                            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "886cf9fc6c01136769bbc397958786ecedfdfd939902", async() => {
                    WriteLiteral("<span class=\"book_now_text other_btn\">Contact Us</span>");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_4.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
                        </li>
                        <li class=""nav-item"">
                            <a class=""nav-link login"" href=""#""><span class=""login_text"">Login</span></a>
                        </li>
                        <li class=""nav-item book_now"">
                            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "886cf9fc6c01136769bbc397958786ecedfdfd9311781", async() => {
                    WriteLiteral("<span class=\"login_text\">Become a Helper</span>");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_5.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                        </li>\r\n                        <li class=\"nav-item\">\r\n                            <a");
                BeginWriteAttribute("href", " href=\"", 2214, "\"", 2221, 0);
                EndWriteAttribute();
                WriteLiteral(@" class=""nav-link text-white""><img src=""./img/ic-flag.png"" style=""margin-right:7px ;""><i class=""fa fa-chevron-down"" style=""color: white;""></i></a>
                        </li>
                    </ul>
                </div>
            </nav>

            <div class=""perent_div"">
                <div class=""child1_home"">
                    <h1 class=""child1_heading_text"">Lorem ipsum  elit.</h1>
                    <div style=""display: flex;"">
                        <div>
                            <i class=""fa fa-check"" style=""color: #ffff;""></i>
                        </div>
                        <div>
                            <p class=""child1_home_text""> ipsum, dolor sit amet consectetur adipisicing elit.</p>
                        </div>
                    </div>
                    <div style=""display: flex;"">
                        <div>
                            <i class=""fa fa-check"" style=""color: #ffff;""></i>
                        </div>
                        <d");
                WriteLiteral(@"iv>
                            <p class=""child1_home_text""> ipsum, dolor sit amet consectetur adipisicing elit.</p>
                        </div>
                    </div>
                    <div style=""display: flex;"">
                        <div>
                            <i class=""fa fa-check"" style=""color: #ffff;""></i>
                        </div>
                        <div>
                            <p class=""child1_home_text""> ipsum, dolor sit amet consectetur adipisicing elit.</p>
                        </div>
                    </div>
                </div>
                <div class=""child2_home"">
                    <Button");
                BeginWriteAttribute("value", " value=\"", 3916, "\"", 3924, 0);
                EndWriteAttribute();
                WriteLiteral(" class=\"child2_btn\">Let\'s Book Cleaner</Button>\r\n                </div>\r\n            </div>\r\n\r\n            <div>\r\n                <div class=\"home_img\">\r\n                    <div class=\"home_child_img\">\r\n                        <img src=\"./img/step-1.png\"");
                BeginWriteAttribute("alt", " alt=\"", 4180, "\"", 4186, 0);
                EndWriteAttribute();
                WriteLiteral(" class=\"child_img\">\r\n                        <p class=\"child_img_text\">Enter Your Postcode</p>\r\n                    </div>\r\n                    <div>\r\n                        <img src=\"./img/step-arrow-1.png\"");
                BeginWriteAttribute("alt", " alt=\"", 4395, "\"", 4401, 0);
                EndWriteAttribute();
                WriteLiteral(">\r\n                    </div>\r\n                    <div class=\"home_child_img\">\r\n                        <img src=\"./img/step-2.png\"");
                BeginWriteAttribute("alt", " alt=\"", 4534, "\"", 4540, 0);
                EndWriteAttribute();
                WriteLiteral(" class=\"child_img\">\r\n                        <p class=\"child_img_text\">Enter Your Postcode</p>\r\n                    </div>\r\n                    <div>\r\n                        <img src=\"./img/step-arrow-1-copy.png\"");
                BeginWriteAttribute("alt", " alt=\"", 4754, "\"", 4760, 0);
                EndWriteAttribute();
                WriteLiteral(">\r\n                    </div>\r\n                    <div class=\"home_child_img\">\r\n                        <img src=\"./img/step-3.png\"");
                BeginWriteAttribute("alt", " alt=\"", 4893, "\"", 4899, 0);
                EndWriteAttribute();
                WriteLiteral(" class=\"child_img\">\r\n                        <p class=\"child_img_text\">Enter Your Postcode</p>\r\n                    </div>\r\n                    <div>\r\n                        <img src=\"./img/step-arrow-1.png\"");
                BeginWriteAttribute("alt", " alt=\"", 5108, "\"", 5114, 0);
                EndWriteAttribute();
                WriteLiteral(">\r\n                    </div>\r\n                    <div class=\"home_child_img\">\r\n                        <img src=\"./img/step-4.png\"");
                BeginWriteAttribute("alt", " alt=\"", 5247, "\"", 5253, 0);
                EndWriteAttribute();
                WriteLiteral(@" class=""child_img"">
                        <p class=""child_img_text"">Enter Your Postcode</p>
                    </div>
                </div>
            </div>

            <div class=""down_arrow"">
                <i class=""fa fa-arrow-circle-o-down""></i>
            </div>
        </div>
    </section>

    <!-- why helplerland -->
    <section>
        <div class=""main_div"">
            <div>
                <h1 class=""heading_text"">Why Helperland</h1>
            </div>
            <div class=""perent_div_help"">
                <div class=""child1_help"">
                    <div>
                        <img src=""./img/helper-img-1.png""");
                BeginWriteAttribute("alt", " alt=\"", 5923, "\"", 5929, 0);
                EndWriteAttribute();
                WriteLiteral(@">
                        <h1 class=""child_text"">Experince & Vetted Professionals</h1>
                    </div>
                    <div class=""helpland_text"">
                        <p>Lorem ipsum, dolor sit amet consectetur adipisicing elit. Beatae iste sapiente nostrum totam odio eum possimus et harum voluptas placeat?</p>
                    </div>
                </div>
                <div class=""child2_help"">
                    <div>
                        <img src=""./img/group-23.png""");
                BeginWriteAttribute("alt", " alt=\"", 6441, "\"", 6447, 0);
                EndWriteAttribute();
                WriteLiteral(@">
                        <h1 class=""child_text1"">Secure online Payment</h1>
                    </div>
                    <div class=""helpland_text"">
                        <p>Lorem ipsum, dolor sit amet consectetur adipisicing elit. Beatae iste sapiente nostrum totam odio eum possimus et harum voluptas placeat?</p>
                    </div>
                </div>
                <div class=""child3_help"">
                    <div>
                        <img src=""./img/group-24.png""");
                BeginWriteAttribute("alt", " alt=\"", 6949, "\"", 6955, 0);
                EndWriteAttribute();
                WriteLiteral(@">
                        <h1 class=""child_text"">Dedicated Customer Services</h1>
                    </div>
                    <div class=""helpland_text"">
                        <p>Lorem ipsum, dolor sit amet consectetur adipisicing elit. Beatae iste sapiente nostrum totam odio eum possimus et harum voluptas placeat?</p>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- our blog -->
    <section>
        <div style=""position: relative;"">
            <!-- background image -->
            <div class=""back_img1""><img src=""./img/blog-left-bg.png""");
                BeginWriteAttribute("alt", " alt=\"", 7581, "\"", 7587, 0);
                EndWriteAttribute();
                WriteLiteral("></div>\r\n            <div class=\"back_img2\"><img src=\"./img/blog-right-bg.png\"");
                BeginWriteAttribute("alt", " alt=\"", 7666, "\"", 7672, 0);
                EndWriteAttribute();
                WriteLiteral(@"></div>
            <div class=""main_our_blog_div"">
                <div class=""our_blog_div"">
                    <div class=""perent_div_three"">
                        <div class=""child1_three"">
                            <h1 class=""child1_three_heading"">Lorem ipsum dolor sit amet,<br> consectetur.</h1>
                            <p class=""child1_three_text"">Lorem ipsum dolor sit amet consectetur, adipisicing elit. Sunt, quibusdam Lorem ipsum dolor sit amet consectetur adipisicing..</p>
                            <p class=""child1_three_text"">Lorem ipsum dolor sit amet consectetur adipisicing elit. Aspernatur amet consectetur quam ullam eius sit, itaque atque magni perspiciatis cum. Lorem, ipsum dolor sit amet consectetur adipisicing elit. Saepe, quis?</p>
                            <p class=""child1_three_text"">Lorem ipsum dolor sit, amet consectetur adipisicing elit. Omnis velit fuga, natus aut at quod Lorem ipsum dolor sit amet consectetur..</p>
                        </div>
                ");
                WriteLiteral("        <div class=\"child2_three\">\r\n                            <img src=\"./img/group-36.png\"");
                BeginWriteAttribute("alt", " alt=\"", 8790, "\"", 8796, 0);
                EndWriteAttribute();
                WriteLiteral(@">
                        </div>
                    </div>

                    <div style=""text-align: center;"">
                        <h1 class=""our_blog_heading"">Our Blog</h1>
                    </div>
                    <div class=""our_blog_perent"">
                        <div class=""child1_blog"">
                            <img src=""./img/group-28.png"" style=""width: 100%;""");
                BeginWriteAttribute("alt", " alt=\"", 9193, "\"", 9199, 0);
                EndWriteAttribute();
                WriteLiteral(@">
                            <h1 class=""blog_heading"">Lorem ipsum dolor sit amet consectetur.</h1>
                            <p class=""date_text"">January 28,2019</p>
                            <p class=""blog_text"">Lorem ipsum dolor sit amet consectetur adipisicing elit. Odit quibusdam iure reprehenderit consequuntur assumenda aliquam?</p>
                            <div style=""display: flex;"">
                                <p class=""read_post"">Read the post </p>
                                <i class=""fa fa-long-arrow-right""></i>
                            </div>
                        </div>
                        <div class=""child1_blog"">
                            <img src=""./img/group-29.png"" style=""width: 100%;""");
                BeginWriteAttribute("alt", " alt=\"", 9949, "\"", 9955, 0);
                EndWriteAttribute();
                WriteLiteral(@">
                            <h1 class=""blog_heading"">Lorem ipsum dolor sit amet consectetur.</h1>
                            <p class=""date_text"">January 28,2019</p>
                            <p class=""blog_text"">Lorem ipsum dolor sit amet consectetur adipisicing elit. Odit quibusdam iure reprehenderit consequuntur assumenda aliquam?</p>
                            <div style=""display: flex;"">
                                <p class=""read_post"">Read the post </p>
                                <i class=""fa fa-long-arrow-right""></i>
                            </div>
                        </div>
                        <div class=""child1_blog"">
                            <img src=""./img/group-30.png"" style=""width: 100%;""");
                BeginWriteAttribute("alt", " alt=\"", 10705, "\"", 10711, 0);
                EndWriteAttribute();
                WriteLiteral(@">
                            <h1 class=""blog_heading"">Lorem ipsum dolor sit amet consectetur.</h1>
                            <p class=""date_text"">January 28,2019</p>
                            <p class=""blog_text"">Lorem ipsum dolor sit amet consectetur adipisicing elit. Odit quibusdam iure reprehenderit consequuntur assumenda aliquam?</p>
                            <div style=""display: flex;"">
                                <p class=""read_post"">Read the post </p>
                                <i class=""fa fa-long-arrow-right""></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- customer -->

    <section>
        <div class=""cus_div"">
            <div class=""main_cus_div"">
                <div class=""our_blog_div"">
                    <div style=""text-align: center;"">
                        <h1 class=""cus_heading"">What Our Customers Say</h1>
           ");
                WriteLiteral(@"         </div>
                    <div class=""our_blog_perent"">
                        <div class=""customer_1"">
                            <div class=""vl""></div>
                            <div class=""cus_img"">
                                <img src=""./img/group-31.png""");
                BeginWriteAttribute("alt", " alt=\"", 12018, "\"", 12024, 0);
                EndWriteAttribute();
                WriteLiteral(@">
                                <div class=""city_cus_name"">
                                    <h2 class=""customer_name1"">Lars Johnson</h2>
                                    <p class=""city"">Manchester</p>
                                </div>
                            </div>
                            <div>
                                <p class=""cus_re"">Lorem ipsum, dolor sit amet consectetur adipisicing elit. Qui deleniti facere architecto accusamus iure laboriosam amet perspiciatis ipsum dolores cumque!</p>
                                <p class=""cus_re"">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Explicabo, distinctio!</p>
                            </div>
                            <div style=""display: flex;"">
                                <p class=""read_post"">Read the post </p>
                                <i class=""fa fa-long-arrow-right""></i>
                            </div>
                        </div>
                        <div class=""customer_2");
                WriteLiteral("\">\r\n                            <div class=\"vl\"></div>\r\n                            <div class=\"cus_img\">\r\n                                <img src=\"./img/group-31.png\"");
                BeginWriteAttribute("alt", " alt=\"", 13217, "\"", 13223, 0);
                EndWriteAttribute();
                WriteLiteral(@">
                                <div class=""city_cus_name"">
                                    <h2 class=""customer_name1"">Lars Johnson</h2>
                                    <p class=""city"">Manchester</p>
                                </div>
                            </div>
                            <div>
                                <p class=""cus_re"">Lorem ipsum, dolor sit amet consectetur adipisicing elit. Qui deleniti facere architecto accusamus iure laboriosam amet perspiciatis ipsum dolores cumque!</p>
                                <p class=""cus_re"">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Explicabo, distinctio!</p>
                            </div>
                            <div style=""display: flex;"">
                                <p class=""read_post"">Read the post </p>
                                <i class=""fa fa-long-arrow-right""></i>
                            </div>
                        </div>
                        <div class=""customer_3");
                WriteLiteral("\">\r\n                            <div class=\"vl\"></div>\r\n                            <div class=\"cus_img\">\r\n                                <img src=\"./img/group-31.png\"");
                BeginWriteAttribute("alt", " alt=\"", 14416, "\"", 14422, 0);
                EndWriteAttribute();
                WriteLiteral(@">
                                <div class=""city_cus_name"">
                                    <h2 class=""customer_name1"">Lars Johnson</h2>
                                    <p class=""city"">Manchester</p>
                                </div>
                            </div>
                            <div>
                                <p class=""cus_re"">Lorem ipsum, dolor sit amet consectetur adipisicing elit. Qui deleniti facere architecto accusamus iure laboriosam amet perspiciatis ipsum dolores cumque!</p>
                                <p class=""cus_re"">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Explicabo, distinctio!</p>
                            </div>
                            <div style=""display: flex;"">
                                <p class=""read_post"">Read the post </p>
                                <i class=""fa fa-long-arrow-right""></i>
                            </div>
                        </div>
                    </div>
                </");
                WriteLiteral(@"div>
            </div>
            <!-- get newsletter -->
            <div class=""container-fluid"">
                <div class=""get_new_perent"">
                    <div class=""get_new_child1"">
                        <img src=""./img/right-arrow-grey.png""");
                BeginWriteAttribute("alt", " alt=\"", 15710, "\"", 15716, 0);
                EndWriteAttribute();
                WriteLiteral(@">
                    </div>
                    <div class=""get_new_child2"">
                        <div class=""sellr_main_div"">
                            <div style=""text-align: center;"">
                                <p class=""letter_text"">GET OUR NEWSLETTER</p>
                                <input type=""email"" placeholder=""Your Email"" class=""for_email"">
                                <input type=""submit"" class=""for_submit"">
                            </div>
                        </div>
                    </div>
                    <div class=""get_new_child3"">
                        <img src=""./img/layer-598.png""");
                BeginWriteAttribute("alt", " alt=\"", 16365, "\"", 16371, 0);
                EndWriteAttribute();
                WriteLiteral(" class=\"time_img\">\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </section>\r\n    \r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
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