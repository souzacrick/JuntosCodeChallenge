# JuntosCodeChallenge

A arquitetura do projeto foi feita com base no seguinte guia (https://docs.microsoft.com/pt-br/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice).

Foi desenvolvido somente o backend/API.

Ao executar o projeto abrirá uma rota que traz (em JSON) todos os clientes carregados, foi feito desta forma para testar o upload de clientes no startup.

A api para obter os clientes foi desenvolvida com 4 opções de filtros: região, tipo, genero e email, abaixo exemplo de chamada JSON:
(POST)
{
    "type":2,
    "pageNumber": 0,
    "pageSize": 20
}

Os testes desenvolvidos contemplam alguns dos possíveis retornos para os clientes e filtros, assim como algumas validações de regra de negócio.
