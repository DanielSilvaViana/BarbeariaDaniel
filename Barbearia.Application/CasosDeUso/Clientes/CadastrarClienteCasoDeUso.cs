using Barbearia.Application.DTOs.Clientes;
using Barbearia.Application.Excecoes;
using Barbearia.Application.Interfaces.CasosDeUso.Clientes;
using Barbearia.Application.Interfaces.Repositorios;
using Barbearia.Application.Mapeamentos;
using Barbearia.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Barbearia.Application.CasosDeUso.Clientes
{
    public sealed class CadastrarClienteCasoDeUso : ICadastrarClienteCasoDeUso
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public CadastrarClienteCasoDeUso(
            IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public async Task<ClienteResponse> ExecutarAsync(
            CadastrarClienteRequest request,
            CancellationToken cancellationToken = default)
        {
            var telefoneJaCadastrado =
                await _clienteRepositorio.ExisteTelefoneAsync(
                    request.Telefone,
                    cancellationToken);

            if (telefoneJaCadastrado)
            {
                throw new ConflitoException(
                    "Já existe um cliente cadastrado com este telefone.");
            }

            var cliente = new Cliente(
                request.Nome,
                request.Telefone,
                request.Email);

            await _clienteRepositorio.AdicionarAsync(
                cliente,
                cancellationToken);

            await _clienteRepositorio.SalvarAlteracoesAsync(
                cancellationToken);

            return cliente.ParaResponse();
        }
    }
}
