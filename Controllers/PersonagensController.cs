using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using RpgApi.Models;

namespace RpgApi.Controllers
{
    [Authorize(Roles = "Jogador, Admin")]
    [ApiController]
    [Route("[controller]")]
    public class PersonagensController : ControllerBase
    {
        private readonly DataContext _context;//Declaração        /* DataContext = Local   /   _context = variavel de tabela */
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PersonagensController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context; //inicialização do atributo
            _httpContextAccessor = httpContextAccessor;
        }

        private int ObterUsuarioId()
        {
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }


        [HttpGet("{id}")] //Buscar pelo id
        public async Task<IActionResult> GetSingle(int id)//async = executar mais de um comando
        {
            //Programação será feita aqui
            try //Tentativa
            {//await = só executara a função quando receber o seu conteudo
                Personagem p = await _context.Personagens
                    .Include(ar => ar.Arma)
                    .Include(ph => ph.PersonagemHabilidades)
                        .ThenInclude(h => h.Habilidade)
                         .Include(u => u.Usuario)

                    .FirstOrDefaultAsync(pBusca => pBusca.Id == id);


                return Ok(p);
            }
            catch (System.Exception ex)// ex = variavel que vai receber o erro
            {
                return BadRequest(ex.Message);
            }
        }

        //Identação:  SHIFT + ALT + F

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Personagem> lista = await _context.Personagens.ToListAsync();
                return Ok(lista);    
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Personagem novoPersonagem)
        {
            try
            {
                if(novoPersonagem.PontosVida > 100)
                {
                    throw new System.Exception("Pontos de vida não pode ser maior que 100");
                //  lançar nova excessão    ("msgm");
                }

                await _context.Personagens.AddAsync(novoPersonagem);
                //    database             add async  var do perso que criamos

                // int usuarioId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                // novoPersonagem.Usuario = _context.Usuarios.FirstOrDefault(uBusca => uBusca.Id == usuarioId);
                novoPersonagem.Usuario = _context.Usuarios.FirstOrDefault(uBusca => uBusca.Id == ObterUsuarioId());

                await _context.SaveChangesAsync();
                //    database  salvar alterações no async
                return Ok(novoPersonagem.Id);    
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Personagem novoPersonagem)
        {
            try
            {
                if(novoPersonagem.PontosVida > 100)
                    throw new System.Exception("Pontos de vida não pode ser maior que 100");

                _context.Personagens.Update(novoPersonagem);

                // int usuarioId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                // novoPersonagem.Usuario = _context.Usuarios.FirstOrDefault(uBusca => uBusca.Id == usuarioId);
                novoPersonagem.Usuario = _context.Usuarios.FirstOrDefault(uBusca => uBusca.Id == ObterUsuarioId());

                int linhasAfetadas = await _context.SaveChangesAsync();
                //   ele mostra quais linhas foram afetadas/alteradas

                return Ok(linhasAfetadas);
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
                Personagem pRemover = await _context.Personagens.FirstOrDefaultAsync(p => p.Id == id); 
                
                _context.Personagens.Remove(pRemover);

                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetByUser")]
        public async Task<IActionResult> GetByUserAsync()
        {
            try
            {
                int id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

                List<Personagem> lista = await _context.Personagens
                    .Where(u => u.Usuario.Id == id).ToListAsync();

                return Ok(lista);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string ObterPerfilUsuario()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        [HttpGet("GetByPerfil")]
        public async Task<IActionResult> GetByPerfilAsync()
        {
            try
            {
                List<Personagem> lista = new List<Personagem>();

                if(ObterPerfilUsuario() == "Admin")
                {
                    lista = await _context.Personagens.ToListAsync();
                }
                else
                {
                    lista = await _context.Personagens
                        .Where(p => p.Usuario.Id == ObterUsuarioId()).ToListAsync();
                }
                return Ok(lista);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





    }
}