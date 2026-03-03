# DesafioPedidos — Instruções de Execução

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

---

## Como executar

### 1. Configurar Certificado HTTPS (Recomendado)

Para evitar avisos de segurança no browser, executa este comando na primeira utilização:

```powershell
dotnet dev-certs https --trust
```

> Quando aparecer uma janela de confirmação, clique em **Sim**.

### 2. Executar a API

Abre o terminal na raiz do projeto e navega até à pasta da API:

```powershell
cd src/DesafioPedidos.API
dotnet run
```

A API ficará disponível em:

- **HTTPS:** `https://localhost:64405`
- **HTTP:** `http://localhost:64406`

> A aplicação irá compilar e arrancar automaticamente. O terminal mostrará o endereço, por exemplo:
> `Now listening on: https://localhost:64405`
> `Now listening on: http://localhost:64406`

### 3. Aceder ao Swagger UI

Abre o browser e acede ao endereço indicado no terminal, adicionando /swagger no fim:

```
https://localhost:64405/swagger
```

---

## Executar os Testes

Na raiz da solução (onde está o ficheiro `.sln`):

```powershell
dotnet test
```

---

## Endpoints disponíveis

### Pedidos — `/api/pedido`

| Método | URL                          | Descrição                 |
| ------ | ---------------------------- | ------------------------- |
| GET    | `/api/pedido`                | Lista todos os pedidos    |
| GET    | `/api/pedido/{numeroPedido}` | Busca pedido por número   |
| POST   | `/api/pedido`                | Cria novo pedido          |
| PUT    | `/api/pedido/{numeroPedido}` | Atualiza pedido existente |
| DELETE | `/api/pedido/{numeroPedido}` | Remove pedido             |

### Status — `/api/status`

| Método | URL           | Descrição                  |
| ------ | ------------- | -------------------------- |
| POST   | `/api/status` | Processa mudança de status |

---
