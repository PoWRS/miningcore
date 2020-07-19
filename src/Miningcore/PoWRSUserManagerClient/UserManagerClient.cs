using Miningcore.Configuration;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MiningCore.PoWRSUserManagerClient
{
    public class UserManagerClient : IUserManagerClient
    {
        private readonly ClusterConfig clusterConfig;
        private readonly HttpClient httpClient;
        private static readonly ILogger logger = LogManager.GetLogger("Usermanager client", typeof(UserManagerClient));

        public UserManagerClient(ClusterConfig config, HttpClient client)
        {
            clusterConfig = config;
            httpClient = client;
        }

        public async Task<Dictionary<string, decimal>> PostRewardsForAdjustment(Dictionary<string, decimal> rewards, string coinType)
        {
            var serialShares = JsonConvert.SerializeObject(rewards);
            Dictionary<string, decimal> ret = new Dictionary<string, decimal>();
            var url = new Uri(clusterConfig.poWRS.TransactionsApiUrl + "?coinType=" + coinType);
            var content = new StringContent(serialShares, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                ret = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(result);
                if (ret == null)
                {
                    logger.Warn("Request returned unexpected (null) result", new System.Diagnostics.StackTrace());
                }
                return ret;
            }

            throw new Exception("Unable to get the desired response from the remote resource.");
        }
    }
}
