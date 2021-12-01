using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using RpgApi.Models;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class ArmasController : ControllerBase
    {
         private readonly DataContext _context;//Declaração        /* DataContext = Local   /   _context = variavel de tabela */
        
        public ArmasController(DataContext context)
        {
            _context = context; //inicialização do atributo

        }
        


        

        [HttpGet("GetArma")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Arma> listaArma = await _context.Armas.ToListAsync();
                return Ok(listaArma);    
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            try
            {
                Arma a = await _context.Armas.FirstOrDefaultAsync(aBusca => aBusca.Id == id);
                return Ok(a);
            }
            catch (System.Exception ex)
            {
                return BadRequest (ex.Message);
                
            }
        }



        [HttpPost]
        public async Task<IActionResult> Add(Arma novaArma)
        {
            try
            {
                if(novaArma.Dano == 0)
                    throw new System.Exception("O dano da arma não pode ser 0");

                Personagem p = await _context.Personagens
                    .FirstOrDefaultAsync(p => p.Id == novaArma.PersonagemId);

                if(p == null)
                    throw new System.Exception("Não existe personagem com Id informado.");

                Arma buscaArma = await _context.Armas
                    .FirstOrDefaultAsync(a => a.PersonagemId == novaArma.PersonagemId);

                await _context.Armas.AddAsync(novaArma);
                await _context.SaveChangesAsync();

                return Ok(novaArma.Id);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Arma aRemover = await _context.Armas.FirstOrDefaultAsync(a => a.Id == id); 
                
                _context.Armas.Remove(aRemover);

                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        

        






        
    }
}