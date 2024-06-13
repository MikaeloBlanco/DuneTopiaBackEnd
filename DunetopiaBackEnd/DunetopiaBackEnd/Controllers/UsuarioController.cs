using DunetopiaBackEnd.Models.Database;
using DunetopiaBackEnd.Models.Database.DTos;
using DunetopiaBackEnd.Models.Database.Entities;
using DunetopiaBackEnd.Models.DTos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DunetopiaBackEnd.Controllers;
[Route("api/[controller]")]
[ApiController]

public class UsuarioController : ControllerBase
{
    private readonly MyDBContext _dbContextDuneTopia;
    private PasswordHasher<string> passwordHasher = new();
    private readonly TokenValidationParameters _tokenParameters;

    public UsuarioController(MyDBContext dbContextDuneTopia, IOptionsMonitor<JwtBearerOptions> jwtOptions)
    {
        _dbContextDuneTopia = dbContextDuneTopia;
        _tokenParameters = jwtOptions.Get(JwtBearerDefaults.AuthenticationScheme).TokenValidationParameters;
    }

    [HttpGet("listaUsuarios")]
    public IEnumerable<UsuarioRegistroDto> GetUser()
    {
        return _dbContextDuneTopia.Usuarios.Select(ToDto);
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Post([FromForm] CreateUsuario usuarioRegistroDto)
    {
        string hashedPassword = passwordHasher.HashPassword(usuarioRegistroDto.Name, usuarioRegistroDto.Password);

        Usuario nuevoUsuario = new Usuario()
        {
            Name = usuarioRegistroDto.Name,
            Email = usuarioRegistroDto.Email,
            Password = hashedPassword,
            Direccion = usuarioRegistroDto.Direccion
        };

        await _dbContextDuneTopia.Usuarios.AddAsync(nuevoUsuario);
        await _dbContextDuneTopia.SaveChangesAsync();

        UsuarioRegistroDto usuarioCreado = ToDto(nuevoUsuario);

        CarroDeCompra nuevoCarro = new CarroDeCompra()
        {
            UsuarioId = usuarioCreado.Id,
            ProductoCarroId = usuarioCreado.Id
        };

        await _dbContextDuneTopia.CarroDeCompras.AddAsync(nuevoCarro);
        await _dbContextDuneTopia.SaveChangesAsync();

        return Created($"/{nuevoUsuario.Id}", usuarioCreado);
    }

    [HttpPost("logueo")]
    public IActionResult Logeo([FromForm] UsuarioLogeoDto usuarioLogeoDto)
    {
        foreach (Usuario listaUsuario in _dbContextDuneTopia.Usuarios.ToList())
        {
            if (listaUsuario.Email == usuarioLogeoDto.Email)
            {
                PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(listaUsuario.Name, listaUsuario.Password, usuarioLogeoDto.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    string rol = " ";

                    if (listaUsuario.IsAdmin == true)
                    {
                        rol = "Admin";
                    }

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Claims = new Dictionary<string, object> {
                            {"id", Guid.NewGuid().ToString() },
                            { ClaimTypes.Role, rol }
                        },
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(
                            _tokenParameters.IssuerSigningKey,
                            SecurityAlgorithms.HmacSha256Signature
                            )
                    };

                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                    string stringToken = tokenHandler.WriteToken(token);

                    return Ok(new { StringToken = stringToken, listaUsuario.Id });
                }
            }
        }
        return Unauthorized("Usuario inexistente");
    }
    [HttpPost("actualizarUsuario")]
    public async Task<bool> Update([FromForm] string name, [FromForm] string email, [FromForm] string password, [FromForm] string address, [FromForm] int idUsuario)
    {
        var usuario = _dbContextDuneTopia.Usuarios.FirstOrDefault(p => p.Id == idUsuario);

        if (!usuario.Name.Equals(name) && name != null)
        {
            usuario.Name = name;
        }
        if (!usuario.Email.Equals(email) && email != null)
        {
            usuario.Email = email;
        }
        if (!usuario.Password.Equals(password) && password != null)
        {
            string hashedPassword = passwordHasher.HashPassword(name, password);
            usuario.Password = password;
        }
        if (!usuario.Direccion.Equals(address) && address != null)
        {
            usuario.Direccion = address;
        }

        _dbContextDuneTopia.Usuarios.Update(usuario);
        await _dbContextDuneTopia.SaveChangesAsync();

        return true;
    }
    [HttpDelete("eliminarUsuario/{idUsuario}")]
    public IActionResult EliminarUsuario(long idUsuario)
    {
        try
        {
            var usuarioAEliminar = _dbContextDuneTopia.Usuarios.FirstOrDefault(usuario => usuario.Id == idUsuario);

            if (usuarioAEliminar == null)
            {
                return NotFound($"Usuario con id {idUsuario} no encontrado o no existe");
            }

            _dbContextDuneTopia.Usuarios.Remove(usuarioAEliminar);
            _dbContextDuneTopia.SaveChanges();

            return Ok(new { Message = "Usuario eliminado con éxito" });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Error = $"Error al eliminar usuario: {ex.Message}"
            });
        }
    }
    [HttpPut("actualizarRolUsuario/{idUsuario}")]
    public IActionResult ActualizarRol(long idUsuario, [FromBody] bool isAdmin)
    {
        try
        {
            var actualizarUsuario = _dbContextDuneTopia.Usuarios.FirstOrDefault(usuario => usuario.Id == idUsuario);

            if (actualizarUsuario == null)
            {
                return NotFound($"Usuario con ID {idUsuario} no encontrado o no existe");
            }

            actualizarUsuario.IsAdmin = isAdmin;
            _dbContextDuneTopia.SaveChanges();

            return Ok(new { Message = "Rol actualizado con exito" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = $"Error al acutalizar el rol de usuario: {ex.Message}" });
        }
    }
    [HttpGet("usuarioinfo/{idUsuario}")]
    public IActionResult GetInfoUsuarioPorId(long idUsuario)
    {
        var usuario = _dbContextDuneTopia.Usuarios
            .Where(u => u.Id == idUsuario)
            .Select(ToDto)
            .FirstOrDefault();
        if (usuario == null)
        {
            return NotFound();
        }
        return Ok(usuario);
    }

    private UsuarioRegistroDto ToDto(Usuario usuario)
    {
        return new UsuarioRegistroDto()
        {
            Id = (int)usuario.Id,
            Nombre = usuario.Name,
            Email = usuario.Email,
            Password = usuario.Password,
            Direccion = usuario.Direccion,
            IsAdmin = usuario.IsAdmin,
        };
    }
}
