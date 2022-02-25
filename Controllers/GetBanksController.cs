using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEMA_BANK.Interface;
using WEMA_BANK.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEMA_BANK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetBanksController : ControllerBase
    {
        private readonly IGetBanks _banks;

        public GetBanksController(IGetBanks banks)
        {
            _banks = banks;
        }


        // GET: api/<GetBanksController>
        [HttpGet]
        public async Task<ActionResult<ResultObjects>> GetAsync()
        {
            return await _banks.GetBanks("GetAllBanks");
        }

    }
}
