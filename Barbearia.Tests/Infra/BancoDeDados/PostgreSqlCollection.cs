namespace Barbearia.Tests.Infrastructure.BancoDeDados;

[CollectionDefinition(
    NomeColecao,
    DisableParallelization = true)]
public sealed class PostgreSqlCollection
    : ICollectionFixture<PostgreSqlFixture>
{
    public const string NomeColecao =
        "Testes de integração PostgreSQL";
}