# NyTimesApi

## Visão Geral do Projeto

**NyTimesApi** é uma Web API desenvolvida em .NET 8 que serve como um agregador de notícias, integrando-se diretamente com a API do New York Times. A aplicação permite que os usuários busquem notícias por palavras-chave, vejam os artigos mais populares e, mediante autenticação, salvem seus artigos favoritos para leitura posterior.

Este projeto foi construído seguindo os princípios da **Clean Architecture**, garantindo uma clara separação de responsabilidades, alta testabilidade e manutenibilidade. A segurança é gerenciada através de autenticação baseada em tokens JWT.

### Funcionalidades Principais

  * **Busca de Notícias**: Pesquise artigos do NYT por palavra-chave, com filtros opcionais.
  * **Artigos Populares**: Obtenha uma lista dos artigos mais vistos, compartilhados ou enviados por e-mail.
  * **Autenticação de Usuários**: Sistema de registro e login seguro com hash de senhas (BCrypt) e tokens JWT.
  * **Gerenciamento de Artigos Salvos**: Usuários autenticados podem salvar e remover artigos de sua lista pessoal.

### Arquitetura e Tecnologias

  * **Framework**: .NET 8
  * **Arquitetura**: Clean Architecture (Core, Application, Tests)
  * **Banco de Dados**: Entity Framework Core 8 com SQL Server
  * **Autenticação**: JWT Bearer Tokens
  * **Validação**: FluentValidation
  * **Testes**: xUnit e Moq
  * **Documentação da API**: Swagger (OpenAPI)
  * **Containerização**: Suporte a Docker

-----

## ⚙️ Instruções de Configuração e Execução

Siga os passos abaixo para configurar e executar o projeto em seu ambiente de desenvolvimento local.

### Pré-requisitos

  * [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
  * Uma instância do [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (ou outro SGBD compatível com EF Core)
  * [dotnet-ef tools](https://docs.microsoft.com/ef/core/cli/dotnet) (ferramenta de linha de comando do EF Core)
  * (Opcional) [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### 1\. Clonar o Repositório

```bash
git clone <URL_DO_SEU_REPOSITORIO>
cd NyTimesApi
```

### 2\. Configurar as Variáveis de Ambiente

As configurações sensíveis, como a chave da API e a string de conexão, devem ser configuradas no arquivo `appsettings.Development.json` dentro do projeto `src/Application`.

**Local:** `src/Application/appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NyTimesDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "nytApiKey": "SUA_CHAVE_DE_API_DO_NYT_AQUI",
  "articleSearchApi": "https://api.nytimes.com/svc/search/v2/articlesearch.json",
  "mostPopularUrl": "https://api.nytimes.com/svc/mostpopular/v2/viewed",
  "Jwt": {
    "Key": "SUA_CHAVE_JWT",
    "Issuer": "NyTimeApi",
    "Audience": "NyTimeApiUsers"
  }
}
```

  * **`ConnectionStrings`**: Ajuste a string de conexão para apontar para a sua instância do SQL Server.
  * **`nytApiKey`**: Insira a sua chave de API obtida no portal de desenvolvedores do New York Times.
  * **`Jwt:Key`**: Substitua por uma chave secreta forte e longa para a assinatura dos tokens JWT.

### 3\. Aplicar as Migrations do Banco de Dados

Para criar o banco de dados e as tabelas necessárias, execute o seguinte comando a partir da pasta raiz da solução:

```bash
dotnet ef database update --startup-project src/Application
```

### 4\. Executar a Aplicação

Com tudo configurado, você pode iniciar a API.

```bash
dotnet run --project src/Application
```

A aplicação estará disponível em `https://localhost:7247` e `http://localhost:5217`. A documentação interativa do Swagger pode ser acessada em `https://localhost:7247/swagger`.

-----

## 🔗 Documentação da API de Terceiros

Este projeto integra-se com as seguintes APIs do New York Times:

  * **Article Search API**: Usada para pesquisar o acervo de artigos.
  * **Most Popular API**: Usada para buscar os artigos mais populares.

A documentação completa para estas e outras APIs pode ser encontrada no portal oficial de desenvolvedores do NYT:

  * **[New York Times Developer Portal](https://developer.nytimes.com/)**