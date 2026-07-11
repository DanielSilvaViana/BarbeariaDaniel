using System;
using System.Collections.Generic;
using System.Text;

namespace Barbearia.Application.Excecoes;

public sealed class NaoEncontradoException : Exception
{
    public NaoEncontradoException(string mensagem)
        : base(mensagem)
    {
    }
}
