# To Do List - ASP.NET MVC

Projeto criado para estudar as principais tecnologias utilizadas com ASP.NET MVC e testes automatizados (unitário, de integração e end-to-end).

# Tecnologias/Pacotes Utilizados
- ASP.NET Core MVC
- Fluent Validation
  
* HTML
* CSS
* Bootstrap
* JavaScript
* JQuery
  
- SQL Server
- Entity Framework Core
  
* XUnit
* Selenium
* Fake It Easy
* Fluent Assertions

# Como Executar a Aplicação

## 0 - Pré-requisitos
 Antes de começar, certifique-se que você tenha as seguintes dependências instaladas:
  * .NET Core 6 SDK
  * .NET Entity Framework Tool
  * Instância SQL Server

## 1 - Credenciais
Configure as credenciais para o SQL Server em **ToDoList.App/appSettings.json** e em **ToDoList.App/appsettings.Development.json**

## 2 - Build
Faça a build da solução utilizando uma IDE ou usando CLI com o comando:
```sh
dotnet build
```

## 3 - Rode as migrations
É necessário rodar as migrations antes de iniciar o projeto pela primeira vez. No diretório raiz do projeto, rode o comando:
```sh
dotnet ef --startup-project ToDoList.App/ToDoList.App.csproj database update
```

## 4 - Rode a aplicação
Depois rode a aplicação usando uma IDE ou via CLI:
```sh
dotnet run --project "ToDoList.App/ToDoList.App.csproj"
```

Agora você pode acessá-la no navegar pelas seguintes URLs:
* Sem SSL:
```sh
http://localhost:5234
```
* Com SSL:
```sh
https://localhost:7169
```

# Como Executar os Testes

## 1 - Rode os testes
Rode os testes utilizando uma IDE ou usando CLI com o comando:
```sh
dotnet test
```

# Desenvolvido por
- [@flavioh-assis](https://github.com/flavioh-assis)
