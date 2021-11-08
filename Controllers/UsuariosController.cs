using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RpgApi.Data;
using RpgApi.Models;

namespace RpgApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        //Códigos aqui
        private readonly DataContext _context; //using RpgApi.Data
        private readonly IConfiguration _configuration;

        public UsuariosController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        //Método "CRIAR PASSWORD HASH"
        private void CriarPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

                                                        //Próxima Etapa

        //Classe "USUARIO EXISTENTE"
        public async Task<bool> UsuarioExistente(string username)
        {
            if(await _context.Usuarios.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
            {
                return true;
            }

            return false;
        }

        //MÉTODO POST "REGISTRAR"
        [AllowAnonymous]
        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarUsuário(Usuario user) //usging RpgApi.Models
        {
            try
            {
                if(await UsuarioExistente(user.Username))
                    throw new System.Exception("Nome de usuário existente.");
                
                CriarPasswordHash(user.PasswordString, out byte[] hash, out byte[] salt);
                user.PasswordString = string.Empty;
                user.PasswordHash = hash;
                user.PasswordSalt = salt;

                await _context.Usuarios.AddAsync(user);
                await _context.SaveChangesAsync();

                return Ok(user.Id);
                
            }
            catch (System.Exception ex)
            {
                return BadRequest (ex.Message);
            }
        }

        //Classe privada "VERIFICAR PASSWORD HASH"
        private bool VerificarPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        //Método POST "Autenticar"
        [AllowAnonymous]
        [HttpPost("Autenticar")]
        public async Task<IActionResult> AutenticarUsuario(Usuario credenciais)
        {
            try
            {
                Usuario usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(credenciais.Username.ToLower() ) );

                if(usuario == null)
                {
                    throw new System.Exception("Usuário não encontrado.");
                }
                else if(!VerificarPasswordHash(credenciais.PasswordString, usuario.PasswordHash, usuario.PasswordSalt))
                {
                    throw new System.Exception("Senha Incorreta.");
                }
                else
                {
                    usuario.DataAcesso = System.DateTime.Now;
                    _context.Usuarios.Update(usuario);
                    await _context.SaveChangesAsync(); //Confirma a alteração no Banco


                    return Ok(CriarToken(usuario)); //Return Ok(usuario.Id)
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //DESAFIO DA SEMANA DA AULA 8
        [HttpPost("AlterarSenha")]
        public async Task<IActionResult> AlterarSenhaUsuario(Usuario credenciais)
        {
            try
            {
                Usuario usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(credenciais.Username.ToLower())); //Busca o usuário no banco atraves do login

                if(usuario == null)//se não achar nenhuma usuario pelo login, retorna a msgm
                {
                    throw new System.Exception("Usuário não encontrado");
                }

                CriarPasswordHash(credenciais.PasswordString, out byte[] hash, out byte[] salt);
                usuario.PasswordHash = hash;//se o usuario existir, executa a criptografia (linha 122)
                usuario.PasswordSalt = salt;//guarda o hash e o salt nas prop do usuario (linhas 123/124)

                _context.Usuarios.Update(usuario);
                int linhasAfetadas = await _context.SaveChangesAsync(); //Confirma a alteração no banco
                return Ok(linhasAfetadas); //Retorna as linhas afetads (geralmente 1)
            
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private string CriarToken(Usuario usuario)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.Role, usuario.Perfil)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_configuration.GetSection("ConfiguracaoToken:Chave").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }



        










        //Final da classe
    }
}