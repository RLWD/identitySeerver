using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace VDW.SalesApp.IdentityServer.Certificates
{
	public class KeyVaultCertificateService
	{
		private readonly string _certificateName;
		private readonly SecretClient _secretClient;
		private readonly CertificateClient _certificateClient;

		public KeyVaultCertificateService(string endpoint, string certificateName)
		{
			if (string.IsNullOrEmpty(endpoint))
				throw new ArgumentException("missing keyVaultEndpoint");

			_certificateName = certificateName;

			var credential = new DefaultAzureCredential();

			_secretClient = new SecretClient(vaultUri: new Uri(endpoint), credential);
			_certificateClient = new CertificateClient(vaultUri: new Uri(endpoint), credential);
		}

		public async Task<(X509Certificate2 ActiveCertificate, X509Certificate2 SecondaryCertificate)> GetCertificatesFromKeyVault()
		{
			(X509Certificate2 ActiveCertificate, X509Certificate2 SecondaryCertificate) certs = (null, null);

			var certificateItems = GetAllEnabledCertificateVersions();

			var certificateFirstItem = certificateItems.FirstOrDefault();
			if (certificateFirstItem != null)
			{
				certs.ActiveCertificate = await GetCertificateAsync(_secretClient, _certificateName, certificateFirstItem.Version);
			}

			if (certificateItems.Count > 1)
			{
				certs.SecondaryCertificate = await GetCertificateAsync(_secretClient, _certificateName, certificateItems[1].Version);
			}

			return certs;
		}

		private List<CertificateProperties> GetAllEnabledCertificateVersions()
		{
			var certificateVersions = _certificateClient.GetPropertiesOfCertificateVersions(_certificateName);
			var certificateItems = certificateVersions.ToList();

			// Find all enabled versions of the certificate and sort them by creation date in decending order 
			return certificateVersions
			  .Where(certVersion => certVersion.Enabled.HasValue && certVersion.Enabled.Value)
			  .OrderByDescending(certVersion => certVersion.CreatedOn)
			  .ToList();
		}

		private static async Task<X509Certificate2> GetCertificateAsync(SecretClient secretClient, string certName, string version)
		{
			// Create a new secret using the secret client.
			var secretName = certName;
			KeyVaultSecret secret = await secretClient.GetSecretAsync(secretName, version);

			var privateKeyBytes = Convert.FromBase64String(secret.Value);

			var certificateWithPrivateKey = new X509Certificate2(privateKeyBytes,
				(string)null,
				X509KeyStorageFlags.MachineKeySet);

			return certificateWithPrivateKey;
		}

	}
}
