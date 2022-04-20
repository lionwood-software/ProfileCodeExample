using Microsoft.Extensions.Configuration;
using Profile.WebApi.Infrastructure;
using Profile.WebApi.Options;

namespace Profile.WebApi.Extensions
{
    internal static class ConfigurationBuilderExtensions
    {
        internal static IConfigurationBuilder AddAzureKeyVaultSupport(this IConfigurationBuilder builder)
        {
            var configuration = builder.Build();

            var azureKeyVaultOptions = configuration.GetSection(AzureKeyVaultOptions.SectionName).Get<AzureKeyVaultOptions>();

            if (!string.IsNullOrEmpty(azureKeyVaultOptions.Uri))
            {
                if (!string.IsNullOrEmpty(azureKeyVaultOptions.ClientId) && !string.IsNullOrEmpty(azureKeyVaultOptions.ClientSecret))
                {
                    builder.AddAzureKeyVault(azureKeyVaultOptions.Uri, azureKeyVaultOptions.ClientId, azureKeyVaultOptions.ClientSecret, new PrefixKeyVaultSecretManager(azureKeyVaultOptions.Prefix));
                }
                else
                {
                    builder.AddAzureKeyVault(azureKeyVaultOptions.Uri);
                }
            }

            return builder;
        }
    }
}