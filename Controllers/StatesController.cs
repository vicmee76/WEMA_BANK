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

        // GET: api/States
        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> GetStates()
        {
            return await _context.States.Include(x => x.Lgas).ToListAsync();
        }

        // GET: api/States/5
        [HttpGet("{id}")]
        public async Task<ActionResult<State>> GetState(int id)
        {
            var get = await _context.States.Include(x => x.Lgas).ToListAsync();
            var state = get.Where(x => x.StateId == id).FirstOrDefault();

            if (state == null)
            {
                return NotFound();
            }

            return state;
        }

        // PUT: api/States/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutState(int id, State state)
        {
            if (id != state.StateId)
            {
                return BadRequest();
            }

            _context.Entry(state).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/States
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<State>> PostState(StateAndLGA state)
        {
            var check = _context.States.Where(x => x.StateName == state.StateName);

            if (check.Count() > 0)
            {
                return StatusCode(409, (new { success = false, message = "State already exits" }));
            }
            else
            {
                State s = new State()
                {
                    StateName = state.StateName.ToUpper()
                };

                _context.States.Add(s);

                await _context.SaveChangesAsync();

                foreach(var l in state.lga)
                {
                    _context.Lgas.Add(new Lga
                    {
                        StateId = s.StateId,
                        LgaName = l
                    });
                }

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetState", new { id = s.StateId }, state);
            }
        }

        // DELETE: api/States/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<State>> DeleteState(int id)
        {
            var state = await _context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }

            _context.States.Remove(state);
            await _context.SaveChangesAsync();

            return state;
        }

        private bool StateExists(int id)
        {
            return _context.States.Any(e => e.StateId == id);
        }
    }
}
