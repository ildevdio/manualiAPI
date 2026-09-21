using System.Collections.Concurrent;
using ManualiAPI.DTO;
using ManualiAPI.Exceptions;
using ManualiAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace ManualiAPI.Services;

public class AdmService : IAdmService
{
    private readonly ConcurrentDictionary<int, Adm> _adms = new();
    private readonly PasswordHasher<Adm> _hasher = new();
    // Serializa Criar/Atualizar: unicidade (check-then-act) + inserção atômicas,
    // para dois POSTs simultâneos com o mesmo username não passarem juntos.
    private readonly object _cadastroLock = new();

    public GetAdmDto Criar(CreateAdmDto dto)
    {
        ValidarTexto(dto.Password, "Senha");
        ValidarTexto(dto.Username, "Username");
        ValidarTexto(dto.Email, "E-mail");

        // Hash é lento de propósito: calculado FORA do lock
        var senhaHash = _hasher.HashPassword(null!, dto.Password);

        lock (_cadastroLock)
        {
            GarantirUnicidade(dto.Username, dto.Email, ignorarId: null);

            var adm = new Adm(dto.Username.Trim(), senhaHash, dto.Email.Trim());

            _adms[adm.IdAdm] = adm;

            return new GetAdmDto
            {
                IdAdm = adm.IdAdm,
                Username = adm.Username,
                Email = adm.Email
            };
        }
    }

    public IEnumerable<GetAdmDto> Listar()
    {
        return _adms.Values
            .OrderBy(a => a.IdAdm)
            .Select(a => new GetAdmDto
            {
                IdAdm = a.IdAdm,
                Username = a.Username,
                Email = a.Email
            })
            .ToList();
    }

    public GetAdmDto BuscarPorId(int id)
    {
        if (!_adms.TryGetValue(id, out var adm))
        {
            throw new AdmNotFoundException(id);
        }

        return new GetAdmDto
        {
            IdAdm = adm.IdAdm,
            Username = adm.Username,
            Email = adm.Email
        };
    }

    public GetAdmDto BuscarPorUsername(string username)
    {
        var adm = _adms.Values.FirstOrDefault(a =>
            a.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (adm is null)
        {
            throw new AdmNotFoundException(username);
        }

        return new GetAdmDto
        {
            IdAdm = adm.IdAdm,
            Username = adm.Username,
            Email = adm.Email
        };
    }

    public GetAdmDto Atualizar(int id, UpdateAdmDto dto)
    {
        if (!_adms.TryGetValue(id, out var adm))
        {
            throw new AdmNotFoundException(id);
        }

        ValidarTexto(dto.Username, "Username");
        ValidarTexto(dto.Email, "E-mail");

        // Hash de senha é lento: calculado FORA do lock
        string? novoHash = null;
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            novoHash = _hasher.HashPassword(adm, dto.Password);
        }

        lock (_cadastroLock)
        {
            GarantirUnicidade(dto.Username, dto.Email, ignorarId: id);

            adm.Username = dto.Username.Trim();
            adm.Email = dto.Email.Trim();

            if (novoHash is not null)
            {
                adm.Password = novoHash;
            }

            return new GetAdmDto
            {
                IdAdm = adm.IdAdm,
                Username = adm.Username,
                Email = adm.Email
            };
        }
    }

    public void Deletar(int id)
    {
        if (!_adms.TryRemove(id, out _))
        {
            throw new AdmNotFoundException(id);
        }
    }

    // ---------- métodos auxiliares ----------

    private void GarantirUnicidade(string username, string email, int? ignorarId)
    {
        var outros = _adms.Values.Where(a => a.IdAdm != ignorarId);

        if (outros.Any(a => a.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase)))
            throw new UsuarioDuplicadoException($"O username '{username}' já está em uso.");

        if (outros.Any(a => a.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase)))
            throw new UsuarioDuplicadoException($"O e-mail '{email}' já está em uso.");
    }

    private static void ValidarTexto(string? valor, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ArgumentException($"{campo} não pode estar vazio.");
        }
    }
}