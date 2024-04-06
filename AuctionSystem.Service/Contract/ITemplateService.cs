using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Contract
{
    public interface ITemplateService
    {
        string GenerateHtmlStringFromViewsAsync<T>(string viewName, T model);
    }
}
