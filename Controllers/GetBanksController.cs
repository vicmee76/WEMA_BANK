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
        /// <summary>
        /// This endpoint fetches all banks from wema api
        /// </summary>
        /// <returns>Returns a success or error message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/GetAllBanks
        /// 
        /// </remarks>
        /// <response code="200">Returns a successfull or a failed request, please see the output model for error messages or the list of banks fetched</response>
        [HttpGet]
        [Produces("application/json")]
        [Route("GetAllBanks")]
        [ProducesResponseType(typeof(BanksModels), 200)]
        public async Task<ActionResult<BanksModels>> GetAllBanks()
        {
            return await _banks.GetResults("GetAllBanks");
        }

    }
}
