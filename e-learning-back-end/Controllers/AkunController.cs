using e_learning_back_end.logics.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace e_learning_back_end.Controllers
{
    [Route("React-Backend/[controller]/[action]")]
    [ApiController]
    public class AkunController : Controller
    {
        private readonly IAkunRepo akunRepo;
        public AkunController(IAkunRepo _akunRepo)
        {
            akunRepo = _akunRepo;
        }

        [HttpGet]
        public IActionResult GetProviceList()
        {
            var result = akunRepo.GetProvinsiList();
            return Ok(result);
        }
    }
}
