using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Services
{
    public interface IGeoCordsService
    {
        Task<GeoCordsResult> GetCoordsAsync(string name);
    }
}
