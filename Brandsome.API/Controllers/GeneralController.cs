using Brandsome.BLL.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Brandsome.API.Controllers
{

    public class GeneralController : APIBaseController
    {
        private readonly IGeneralBL _generalBL;

        public GeneralController(IGeneralBL generalBL)
        {
            _generalBL = generalBL;
        }

        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            return Ok(await _generalBL.GetCities());
        }
    }
}
