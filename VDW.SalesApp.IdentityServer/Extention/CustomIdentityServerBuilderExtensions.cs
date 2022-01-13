﻿using Microsoft.Extensions.DependencyInjection;
using VDW.SalesApp.IdentityServer.Services;

namespace VDW.SalesApp.IdentityServer.Extention
{
    public static class CustomIdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddCustomUserStore(this IIdentityServerBuilder builder)
        {
            builder.AddProfileService<ProfileService>();
            builder.AddResourceOwnerValidator<ResourceOwnerServices>();
            return builder;
        }
    }
}
