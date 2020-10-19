# JuntosCodeChallenge

### Informações sobre o projeto ###

A arquitetura do projeto foi feita com base no seguinte guia (https://docs.microsoft.com/pt-br/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice).

Foi utilizado o .net core 3.1 como linguagem, então para alterar/debugar é necessário instalar o SDK dele e recomendo a utilização do visual studio (community) ou visual studio code para estas ações.

Links de apoio:
https://visualstudio.microsoft.com/pt-br/ -- visual studio e visual studio code
https://code.visualstudio.com/docs/editor/debugging -- debugando no visual studio code
https://dotnet.microsoft.com/download/dotnet-core/3.1 -- SDK do core 3.1

É necessário o docker para a execução do projeto.

https://www.docker.com/get-started -- neste link é possível baixar para o OS desejado.

Para o teste da api é necessária uma ferramenta que permita realizar chamadas rest, por exemplo o postman.

https://www.postman.com/downloads/ -- link para download

### Executando o projeto ###

É possível obter o código/projeto de duas formas:
1 - via linha de comando, deve-se instalar o cli do git (https://git-scm.com/book/en/v2/Getting-Started-Installing-Git) e executar o comando:
git clone https://github.com/souzacrick/JuntosCodeChallenge.git

2 - download do zip aqui pelo Github.

Após obter o código, deve-se abrir um terminal (prompt de comando, visual studio code, etc) e navegar até o diretório do projeto e após isso navegar até o diretório que contém o arquivo "docker-compose.yml".

Executar o seguinte comando:

docker-compose up -d

Este comando subirá um container com a API e executará os testes (unitários e de integração) já desenvolvidos, é possível acompanhar este processo pelo terminal.

Com o container no ar é possível consumir a API, ela subirá sempre na porta 44378 (http://localhost:44378), não utilizar https.

Foram desenvolvido dois endpoints:

1 - /api/customer (GET)

Traz em JSON todos os clientes carregados no startup do projeto (este endpoint é possível testar chamando direto no browser).

2 - api/customer/Filter (POST)

Para obter os clientes filtrados, existem 4 opções de filtros: região (region - inteiro), tipo (type - inteiro), genero (gender - text) e email, exemplos de chamada JSON (este endpoint necessita do postman ou ferramenta similar):

Passando no body ->
{
    "type":2,
    "pageNumber": 0,
    "pageSize": 20
}
 | 
{
    "region":2,
    "pageNumber": 0,
    "pageSize": 20
}
 | 
{
    "region":4,
    "gender": "m",
    "pageNumber": 0,
    "pageSize": 20
}

Os valores disponíveis para type são 1 - Special, 2 - Normal e 3 -Laborious.

Os valores disponíveis para region são 1 - Norte, 2 - Nordeste, 3 - CentroOeste, 4 - Sudeste e 5 - Sul.

Em caso de não envio do pageNumber e pageSize, será atribuido de forma automática os valores 0 para o pageNumber e 10 para o pageSize.

### Configurações do projeto ###

Caso deseje alterar a nacionalidade padrão e/ou os valores do bounding box para definir o tipo de cliente eles estão no appsettings.json.

### Testes do projeto ###

Os testes desenvolvidos contemplam alguns dos possíveis retornos para os clientes e filtros, assim como algumas validações de regra de negócio.
