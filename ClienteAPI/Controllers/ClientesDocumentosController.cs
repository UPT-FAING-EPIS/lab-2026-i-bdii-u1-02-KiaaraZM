using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClienteAPI.Data;
using ClienteAPI.Models;

namespace ClienteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesDocumentosController : ControllerBase
    {
        private readonly BdClientesContext _context;

        public ClientesDocumentosController(BdClientesContext context)
        {
            _context = context;
        }

        // GET: api/ClientesDocumentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientesDocumento>>> GetClientesDocumentos()
        {
            return await _context.ClientesDocumentos.ToListAsync();
        }

        // GET: api/ClientesDocumentos/5/1
        [HttpGet("{idCliente}/{idTipoDocumento}")]
        public async Task<ActionResult<ClientesDocumento>> GetClientesDocumento(int idCliente, byte idTipoDocumento)
        {
            var clientesDocumento = await _context.ClientesDocumentos
                .FindAsync(idCliente, idTipoDocumento);

            if (clientesDocumento == null)
            {
                return NotFound();
            }

            return clientesDocumento;
        }

        // PUT: api/ClientesDocumentos/5/1
        [HttpPut("{idCliente}/{idTipoDocumento}")]
        public async Task<IActionResult> PutClientesDocumento(int idCliente, byte idTipoDocumento, ClientesDocumento clientesDocumento)
        {
            if (idCliente != clientesDocumento.IdCliente || idTipoDocumento != clientesDocumento.IdTipoDocumento)
            {
                return BadRequest();
            }

            _context.Entry(clientesDocumento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientesDocumentoExists(idCliente, idTipoDocumento))
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

        // POST: api/ClientesDocumentos
        [HttpPost]
        public async Task<ActionResult<ClientesDocumento>> PostClientesDocumento(ClientesDocumento clientesDocumento)
        {
            _context.ClientesDocumentos.Add(clientesDocumento);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClientesDocumentoExists(clientesDocumento.IdCliente, clientesDocumento.IdTipoDocumento))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClientesDocumento",
                new { idCliente = clientesDocumento.IdCliente, idTipoDocumento = clientesDocumento.IdTipoDocumento },
                clientesDocumento);
        }

        // DELETE: api/ClientesDocumentos/5/1
        [HttpDelete("{idCliente}/{idTipoDocumento}")]
        public async Task<IActionResult> DeleteClientesDocumento(int idCliente, byte idTipoDocumento)
        {
            var clientesDocumento = await _context.ClientesDocumentos
                .FindAsync(idCliente, idTipoDocumento);

            if (clientesDocumento == null)
            {
                return NotFound();
            }

            _context.ClientesDocumentos.Remove(clientesDocumento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientesDocumentoExists(int idCliente, byte idTipoDocumento)
        {
            return _context.ClientesDocumentos
                .Any(e => e.IdCliente == idCliente && e.IdTipoDocumento == idTipoDocumento);
        }
    }
}
