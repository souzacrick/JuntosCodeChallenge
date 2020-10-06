# JuntosCodeChallenge

A arquitetura do projeto foi feita com base no seguinte guia (https://docs.microsoft.com/pt-br/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice).

Foi desenvolvido somente o backend/API.

Ao executar o projeto abrirá uma rota que traz (em JSON) todos os clientes carregados, foi feito desta forma para testar o upload de clientes no startup.

A api para obter os clientes foi desenvolvida com 4 opções de filtros: região, tipo, genero e email, exemplos de chamada JSON:

(POST)
{
    "type":2,
    "pageNumber": 0,
    "pageSize": 20
}
{
    "region":2,
    "pageNumber": 0,
    "pageSize": 20
}
{
    "region":4,
    "gender": "m",
    "pageNumber": 0,
    "pageSize": 20
}

Os valores disponíveis para type são 1 - Special, 2 - Normal e 3 -Laborious

Os valores disponíveis para region são 1 - Norte, 2 - Nordeste, 3 - CentroOeste, 4 - Sudeste e 5 - Sul

Caso deseje alterar a nacionalidade padrão e/ou os valores do bounding box para definir o tipo de cliente eles estão no appsettings.json.

Os testes desenvolvidos contemplam alguns dos possíveis retornos para os clientes e filtros, assim como algumas validações de regra de negócio.
