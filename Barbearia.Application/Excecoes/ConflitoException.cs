using System;
using System.Collections.Generic;
using System.Text;

namespace Barbearia.Application.Excecoes;

public sealed class ConflitoException : Exception
{
    public ConflitoException(string mensagem)
        : base(mensagem)
    {
    }
}
