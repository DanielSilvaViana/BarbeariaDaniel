using System;
using System.Collections.Generic;
using System.Text;

namespace Barbearia.Application.DTOs.Clientes;

public sealed record AtualizarClienteRequest(
    string Nome,
    string Telefone,
    string? Email);

