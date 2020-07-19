using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiningCore.PoWRSUserManagerClient
{
    public interface IUserManagerClient
    {
        Task<Dictionary<string, decimal>> PostRewardsForAdjustment(Dictionary<string, decimal> rewards, string coinType);
    }
}