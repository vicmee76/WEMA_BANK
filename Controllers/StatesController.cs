using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEMA_BANK.Models;
using WEMA_BANK.Models.DB;

namespace WEMA_BANK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly WEMAContext _context;

        public StatesController(WEMAContext context)
        {
            _context = context;
        }




        /// <summary>
        /// This get all the list of states and local govment attached to each state
        /// </summary>
        /// <returns>Returns a list of states and lgas</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/States
        /// 
        /// </remarks>
        /// <response code="200">Returns a list of states and lgas </response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(StateResult), 200)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StateResult>>> GetStates()
        {
            var result = await (from s in _context.States
                          select new StateResult
                          {
                              StateId = s.StateId,
                              StateName = s.StateName,
                              Lgas = _context.Lga.Where(x => x.StateId == s.StateId).Select(x => new LgaResult { LgaId = x.LgaId, LgaName = x.LgaName }).ToList()
                          }).ToListAsync();

            return result;
                
        }




        /// <summary>
        /// This get a particular state and local govment attached to that state
        /// </summary>
        /// <returns>Returns a states and associated lgas</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// // GET: api/States/5
        /// 
        /// </remarks>
        /// /// <response code="200">Returns a list of states and lgas </response>
        /// /// <response code="404">Returns Not found </response>
        [ProducesResponseType(typeof(StateResult), 200)]
        [ProducesResponseType(typeof(ResultObjects), 404)]
        [Produces("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<StateResult>> GetState(int id)
        {
            var result = await (from s in _context.States
                                select new StateResult
                                {
                                    StateId = s.StateId,
                                    StateName = s.StateName,
                                    Lgas = _context.Lga.Where(x => x.StateId == s.StateId).Select(x => new LgaResult { LgaId = x.LgaId, LgaName = x.LgaName }).ToList()
                                }).ToListAsync();

            var state = result.Where(x => x.StateId == id).FirstOrDefault();

            if (state == null)
            {
                return NotFound();
            }

            return state;
        }



        /// <summary>
        /// Creates a state and all local goverment for that state
        /// </summary>
        /// <returns>Success or error message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/States
        /// 
        /// </remarks>
        /// <param name="state">Request Payload</param>
        /// <response code="201">Returns the created state and lga </response>
        /// <response code="409">Returns State already exits </response>
        /// 
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(StateResult), 201)]
        [ProducesResponseType(typeof(ResultObjects), 409)]

        public async Task<ActionResult<StateResult>> PostState(StateAndLGA state)
        {
            var check = _context.States.Where(x => x.StateName == state.StateName);

            if (check.Count() > 0)
            {
                return StatusCode(409, (new { success = false, message = "State already exits" }));
            }
            else
            {
                States s = new States()
                {
                    StateName = state.StateName.ToUpper()
                };

                _context.States.Add(s);

                await _context.SaveChangesAsync();

                foreach(var l in state.Lga)
                {
                    _context.Lga.Add(new Lga
                    {
                        StateId = s.StateId,
                        LgaName = l
                    });
                }

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetState", new { id = s.StateId });
            }
        }

       
    }
}
