## README

# Projeto de Exemplo: Cache Distribuído com Redis, Scrutor e Padrão Decorator no ASP.NET Core 8

Este é um projeto de exemplo que demonstra como implementar um cache distribuído utilizando Redis em uma aplicação ASP.NET Core 8. O projeto também utiliza o padrão de projeto Decorator e a biblioteca Scrutor para interceptar requisições que iriam para o banco de dados, verificando se os dados já estão no cache.

## Requisitos

- .NET 8 SDK
- Redis

## Configuração do Projeto

### 1. Clonar o Repositório

```bash
git clone [https://github.com/fabiosalomaosilva/TesteCacheRedisDecorator](https://github.com/fabiosalomaosilva/TesteCacheRedisDecorator)
cd TesteCacheRedisDecorator
```

### 2. Instalar Dependências

Execute o seguinte comando para restaurar as dependências do projeto:

```bash
dotnet restore
```

### 3. Configurar o Redis

Certifique-se de que o Redis esteja instalado e em execução na sua máquina. Você pode configurar o Redis no arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=app.db"
  },
  "Redis": {
    "Configuration": "localhost:6379",
    "InstanceName": "SampleInstance"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### 4. Configurar o DbContext

Adicione a configuração do SQLite e do Redis no `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = builder.Configuration["Redis:InstanceName"];
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Services
builder.Services.AddTransient<RedisContext>();
builder.Services.AddTransient<IRepository<Pessoa>, PessoaRepository>();
builder.Services.AddTransient<IRepository<Endereco>, EnderecoRepository>();
builder.Services.Decorate(typeof(IRepository<>), typeof(CachedRepository<>));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

### 5. Criar Migrações e Atualizar o Banco de Dados

Execute os comandos a seguir para criar as migrações e atualizar o banco de dados:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Executar o Projeto

Para executar o projeto, use o comando:

```bash
dotnet run
```

A aplicação estará disponível em `https://localhost:5001` (ou `http://localhost:5000`).

## Estrutura do Projeto

- **Controllers**: Contém os controladores da API.
- **Models**: Contém as classes de modelo de dados.
- **Repositories**: Contém as implementações dos repositórios.
- **Decorators**: Contém as implementações dos decoradores que adicionam comportamentos adicionais aos repositórios.

## Exemplo de Uso

Você pode interagir com a API utilizando ferramentas como Postman ou curl. Aqui estão alguns exemplos de endpoints:

### 1. Obter Todos as Pessoas

```http
GET /api/pessoas
```

### 2. Obter Pessoa por ID

```http
GET /api/pessoas/{id}
```

### 3. Adicionar Nova Pessoa

```http
POST /api/pessoas
Content-Type: application/json

{
  "idPessoa": 0,
  "nome": "string",
  "idade": 0,
  "tipoPessoa": 0,
  "enderecos": [
    {
      "idEndereco": 0,
      "logradouro": "string",
      "numero": "string",
      "bairro": "string",
      "idPessoa": 0,
      "pessoa": "string"
    }
  ]
}
```

### 4. Atualizar Pessoa

```http
PUT /api/pessoas/{id}
Content-Type: application/json

{
  "idPessoa": 0,
  "nome": "string",
  "idade": 0,
  "tipoPessoa": 0,
  "enderecos": [
    {
      "idEndereco": 0,
      "logradouro": "string",
      "numero": "string",
      "bairro": "string",
      "idPessoa": 0,
      "pessoa": "string"
    }
  ]
}
```

### 5. Deletar Pessoa

```http
DELETE /api/products/{id}
```

## Contribuindo

Se você encontrar problemas ou tiver sugestões, sinta-se à vontade para abrir uma issue ou enviar um pull request.

## Licença

Este projeto está licenciado sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.
