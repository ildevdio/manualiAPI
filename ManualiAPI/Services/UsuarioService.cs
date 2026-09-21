using System.Collections.Concurrent;
using ManualiAPI.DTO;
using ManualiAPI.Exceptions;
using ManualiAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace ManualiAPI.Services;

public class UsuarioService : IUsuarioService
{
    private readonly ConcurrentDictionary<int, Usuario> _usuarios = new();
    private readonly PasswordHasher<Usuario> _hasher = new();
    // Serializa Criar/Atualizar: unicidade (check-then-act) + inserção atômicas,
    // para dois POSTs simultâneos com o mesmo username não passarem juntos.
    private readonly object _cadastroLock = new();

    public GetUsuarioDto Criar(CreateUsuarioDto dto)
    {
        ValidarTexto(dto.Password, "Senha");
        ValidarTexto(dto.Username, "Username");
        ValidarTexto(dto.Email, "E-mail");

        // Hash é lento de propósito: calculado FORA do lock
        var senhaHash = _hasher.HashPassword(null!, dto.Password);

        lock (_cadastroLock)
        {
            GarantirUnicidade(dto.Username, dto.Email, ignorarId: null);

            var usuario = new Usuario(dto.Username.Trim(), senhaHash, dto.Email.Trim(), dto.Cep);

            _usuarios[usuario.Id] = usuario;

            return new GetUsuarioDto
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Email = usuario.Email,
                Cep = usuario.Cep
            };
        }
    }

    public IEnumerable<GetUsuarioDto> Listar()
    {
        return _usuarios.Values
            .OrderBy(u => u.Id)
            .Select(u => new GetUsuarioDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Cep = u.Cep
            })
            .ToList();
    }

    public GetUsuarioDto BuscarPorId(int id)
    {
        if (!_usuarios.TryGetValue(id, out var usuario))
        {
            throw new UsuarioNotFoundException(id);
        }

        return new GetUsuarioDto
        {
            Id = usuario.Id,
            Username = usuario.Username,
            Email = usuario.Email,
            Cep = usuario.Cep
        };
    }

    public GetUsuarioDto BuscarPorUsername(string username)
    {
        var usuario = _usuarios.Values.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (usuario is null)
        {
            throw new UsuarioNotFoundException(username);
        }

        return new GetUsuarioDto
        {
            Id = usuario.Id,
            Username = usuario.Username,
            Email = usuario.Email,
            Cep = usuario.Cep
        };
    }

    public GetUsuarioDto Atualizar(int id, UpdateUsuarioDto dto)
    {
        if (!_usuarios.TryGetValue(id, out var usuario))
        {
            throw new UsuarioNotFoundException(id);
        }

        ValidarTexto(dto.Username, "Username");
        ValidarTexto(dto.Email, "E-mail");
        ValidarTexto(dto.Cep, "CEP");

        // Hash de senha é lento: calculado FORA do lock
        string? novoHash = null;
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            novoHash = _hasher.HashPassword(usuario, dto.Password);
        }

        lock (_cadastroLock)
        {
            GarantirUnicidade(dto.Username, dto.Email, ignorarId: id);

            usuario.Username = dto.Username.Trim();
            usuario.Email = dto.Email.Trim();
            usuario.Cep = dto.Cep;

            if (novoHash is not null)
            {
                usuario.Password = novoHash;
            }

            return new GetUsuarioDto
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Email = usuario.Email,
                Cep = usuario.Cep
            };
        }
    }

    public void Deletar(int id)
    {
        if (!_usuarios.TryRemove(id, out _))
        {
            throw new UsuarioNotFoundException(id);
        }
    }

    // ---------- métodos auxiliares ----------

    private void GarantirUnicidade(string username, string email, int? ignorarId)
    {
        foreach (var u in _usuarios.Values)
        {
            if (u.Id == ignorarId) continue;

            if (u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase))
                throw new UsuarioDuplicadoException($"O username '{username}' já está em uso.");

            if (u.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase))
                throw new UsuarioDuplicadoException($"O e-mail '{email}' já está em uso.");
        }
    }

    private static void ValidarTexto(string? valor, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ArgumentException($"{campo} não pode estar vazio.");
        }
    }
}