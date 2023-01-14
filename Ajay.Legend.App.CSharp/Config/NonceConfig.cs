using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;

namespace Ajay.Legend.App.CSharp.Config
{
    /// <summary>
    /// Nonces used for inline script execution.
    /// </summary>
    public class NonceConfig
    {
        private const string jsEnabledKey = "JsEnabled";
        private const string applicationInsightsKey = "ApplicationInsights";
        private const string applicationInsightsSetAuthenticatedContextUserKey = "ApplicationInsightsSetAuthenticatedContextUser";

        private static readonly RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        private readonly Dictionary<string, string> nonces = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="NonceConfig"/> class.
        /// </summary>
        public NonceConfig()
        {
            this.Add(jsEnabledKey);
            this.Add(applicationInsightsKey);
            this.Add(applicationInsightsSetAuthenticatedContextUserKey);
        }

        /// <summary>
        /// Gets the nonce for the js enabled script.
        /// </summary>
        public string JsEnabled => this.Get(jsEnabledKey);

        /// <summary>
        /// Gets the nonce for the application insights script.
        /// </summary>
        public string ApplicationInsights => this.Get(applicationInsightsKey);

        /// <summary>
        /// Gets the nonce for the application insights set authenticated context user script.
        /// </summary>
        public string ApplicationInsightsSetAuthenticatedContextUser => this.Get(applicationInsightsSetAuthenticatedContextUserKey);

        /// <summary>
        /// Adds a nonce.
        /// </summary>
        /// <param name="name">The name of the nonce.</param>
        public void Add(string name) => this.nonces.Add(name, GenerateNonce(8));

        /// <summary>
        /// Gets a nonce by name.
        /// </summary>
        /// <param name="name">The name of the nonce.</param>
        /// <returns>The nonce.</returns>
        public string Get(string name) => this.nonces[name];

        private static string GenerateNonce(int byteCount)
        {
            var byteArray = new byte[byteCount];
            randomNumberGenerator.GetBytes(byteArray);

            return BitConverter.ToString(byteArray, 0).Replace("-", string.Empty, false, CultureInfo.InvariantCulture);
        }
    }
}
