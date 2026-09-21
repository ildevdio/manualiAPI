using System.Collections.Concurrent;
using ManualiAPI.DTO;
using ManualiAPI.Exceptions;
using ManualiAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace ManualiAPI.Services;

public class ArtesaoService : IArtesaoService
{
    private readonly ConcurrentDictionary<int, Artesao> _artesaos = new();
    private readonly PasswordHasher<Artesao> _hasher = new();

    public GetArtesaoDto Criar(CreateArtesaoDto dto)
    {
        ValidarTexto(dto.Password, "Senha");
        ValidarTexto(dto.Username, "Username");
        ValidarTexto(dto.Email, "E-mail");
        ValidarTexto(dto.Cep, "CEP");
        ValidarTexto(dto.Cpf, "CPF");

        var cpf = NormalizarCpf(dto.Cpf);
        GarantirUnicidade(dto.Username, dto.Email, cpf, ignorarId: null);

        var senhaHash = _hasher.HashPassword(null!, dto.Password);

        var artesao = new Artesao(
            dto.Username.Trim(), senhaHash, dto.Email.Trim(), dto.Cep, cpf);

        _artesaos[artesao.Id] = artesao;

        return new GetArtesaoDto
        {
            Id = artesao.Id,
            Username = artesao.Username,
            Email = artesao.Email,
            Cep = artesao.Cep
        };
    }

    public IEnumerable<GetArtesaoDto> Listar()
    {
        return _artesaos.Values
            .OrderBy(a => a.Id)
            .Select(a => new GetArtesaoDto
            {
                Id = a.Id,
                Username = a.Username,
                Email = a.Email,
                Cep = a.Cep
            })
            .ToList();
    }

    public GetArtesaoDto BuscarPorId(int id)
    {
        if (!_artesaos.TryGetValue(id, out var artesao))
        {
            throw new ArtesaoNotFoundException(id);
        }

        return new GetArtesaoDto
        {
            Id = artesao.Id,
            Username = artesao.Username,
            Email = artesao.Email,
            Cep = artesao.Cep
        };
    }

    public GetArtesaoDto BuscarPorUsername(string username)
    {
        var artesao = _artesaos.Values.FirstOrDefault(a =>
            a.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (artesao is null)
        {
            throw new ArtesaoNotFoundException(username);
        }

        return new GetArtesaoDto
        {
            Id = artesao.Id,
            Username = artesao.Username,
            Email = artesao.Email,
            Cep = artesao.Cep
        };
    }

    public GetArtesaoDto Atualizar(int id, UpdateArtesaoDto dto)
    {
        if (!_artesaos.TryGetValue(id, out var artesao))
        {
            throw new ArtesaoNotFoundException(id);
        }

        ValidarTexto(dto.Username, "Username");
        ValidarTexto(dto.Email, "E-mail");
        ValidarTexto(dto.Cep, "CEP");
        ValidarTexto(dto.Cpf, "CPF");

        var cpf = NormalizarCpf(dto.Cpf);
        GarantirUnicidade(dto.Username, dto.Email, cpf, ignorarId: id);

        artesao.Username = dto.Username.Trim();
        artesao.Email = dto.Email.Trim();
        artesao.Cep = dto.Cep;
        artesao.Cpf = cpf;

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            artesao.Password = _hasher.HashPassword(artesao, dto.Password);
        }

        return new GetArtesaoDto
        {
            Id = artesao.Id,
            Username = artesao.Username,
            Email = artesao.Email,
            Cep = artesao.Cep
        };
    }

    public void Deletar(int id)
    {
        if (!_artesaos.TryRemove(id, out _))
        {
            throw new ArtesaoNotFoundException(id);
        }
    }

    // ---------- métodos auxiliares ----------

    private void GarantirUnicidade(string username, string email, string cpf, int? ignorarId)
    {
        var outros = _artesaos.Values.Where(a => a.Id != ignorarId);

        if (outros.Any(a => a.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase)))
            throw new UsuarioDuplicadoException($"O username '{username}' já está em uso.");

        if (outros.Any(a => a.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase)))
            throw new UsuarioDuplicadoException($"O e-mail '{email}' já está em uso.");

        if (outros.Any(a => a.Cpf == cpf))
            throw new UsuarioDuplicadoException("Este CPF já está cadastrado.");
    }

    // Remove pontos e traço: "123.456.789-09" -> "12345678909"
    private static string NormalizarCpf(string cpf)
    {
        return new string(cpf.Where(char.IsDigit).ToArray());
    }

    private static void ValidarTexto(string? valor, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ArgumentException($"{campo} não pode estar vazio.");
        }
    }
}