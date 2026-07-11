using System;
using System.Collections.Generic;
using System.Text;

namespace Barbearia.Application.DTOs.Clientes;

public sealed record ClienteResponse(
    Guid Id,
    string Nome,
    string Telefone,
    string? Email,
    DateTime CriadoEm);
