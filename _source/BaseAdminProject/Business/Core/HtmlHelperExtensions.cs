// <copyright file="HtmlHelperExtensions.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseAdminProject.Business.Core
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent GetActiveClass(this IHtmlHelper helper, HttpRequest httpRequest, string route)
        {
            bool active = httpRequest.Path == route;

            return new HtmlString(active ? "active" : string.Empty);
        }
    }
}
