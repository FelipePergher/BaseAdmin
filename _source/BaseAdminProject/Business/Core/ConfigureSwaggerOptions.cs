// <copyright file="ConfigureSwaggerOptions.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BaseAdminProject.Business.Core
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "BaseAdmin API",
                Version = $"API {description.ApiVersion}",
                Description = "BaseAdmin Api",
                Contact = new OpenApiContact()
                {
                    Name = "Felipe Pergher",
                    Email = "felipepergher_10@hotmail.com"
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " Esta versão da api está descontinuada.";
            }

            return info;
        }
    }
}