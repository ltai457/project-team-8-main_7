﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Authentication.Data;
using Project_Authentication.Dto;
using Project_Authentication.Model;

namespace Project_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ProjectDBContext _context;

        public RolesController(ProjectDBContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet("GetRole")]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
          if (_context.Roles == null)
          {
              return NotFound();
          }
            return await _context.Roles.ToListAsync();
        }

        // GET: api/Roles/5
        [HttpGet("GetRoleBy/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
          if (_context.Roles == null)
          {
              return NotFound();
          }
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateRole{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutRole(int id, Role role)
        {
            if (id != role.RoleID)
            {
                return BadRequest();
            }

            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
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

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateNewRole")]
        ////[Authorize(Roles = "Admin")]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
          if (_context.Roles == null)
          {
              return Problem("Entity set 'ProjectDBContext.Roles'  is null.");
          }
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRole", new { id = role.RoleID }, role);
        }

        // DELETE: api/Roles/5
        [HttpDelete("DeleteRole{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            if (_context.Roles == null)
            {
                return NotFound();
            }
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleExists(int id)
        {
            return (_context.Roles?.Any(e => e.RoleID == id)).GetValueOrDefault();
        }
    }
}
