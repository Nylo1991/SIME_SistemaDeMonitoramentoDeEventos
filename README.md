# 📘 Sistema de Eventos - Documentação Completa

## 🧾 Visão Geral

O sistema é composto por **4 projetos**, trabalhando de forma integrada:

* 🖥️ **EventoInterface** → Aplicação cliente (WPF - MVVM)
* 🔄 **EventoSimulador** → Simulador responsável pela geração automática de eventos
* 📦 **Shared** → Biblioteca compartilhada com modelos do sistema
* 🌐 **ApiEventos** → API REST responsável pelo gerenciamento e persistência dos dados

---

## 🧱 Arquitetura do Sistema

```
[ EventoSimulador ] ---> [ ApiEventos ] <--- [ EventoInterface ]
         │                      │                    │
         └──────────► [ Shared ] ◄───────────────────┘
```

📌 A arquitetura segue o princípio de **separação de responsabilidades**, onde cada projeto possui uma função específica no sistema.

---

## 📦 Projetos

---

### 🔹 1. ApiEventos (Backend)

Responsável por:

* Persistência de dados utilizando SQLite
* Regras de negócio
* Exposição de endpoints REST

📁 Estrutura principal:

```
Controllers/
 └── EventosController.cs

Data/
 └── AppDbContext.cs

DTOs/
Migrations/

Program.cs
appsettings.json
eventos.db
```

📍 Base URL:

```
https://localhost:xxxx/api/v1/evento
```

---

### 🔹 2. EventoInterface (Frontend - WPF)

Aplicação desktop que permite:

* 📄 Visualização de eventos
* ➕ Cadastro de eventos
* ✏️ Atualização de eventos
* 🗑️ Remoção de eventos

📌 Padrão utilizado:

* MVVM (Model-View-ViewModel)

📁 Estrutura:

```
Views/
 └── MainWindow.xaml

ViewModels/
Commands/
DTOs_Interface/
```

---

### 🔹 3. EventoSimulador

Responsável por:

* Gerar eventos automaticamente
* Enviar eventos para a API via HTTP

📁 Estrutura:

```
DTOs_Simulador/
 └── EventoRequestDto.cs

Program.cs
```

---

### 🔹 4. Shared

Biblioteca compartilhada contendo:

* Modelo de dados do sistema (`Evento.cs`)

---

## ⚙️ Tecnologias Utilizadas

* .NET 6+
* ASP.NET Core Web API
* WPF (MVVM)
* Entity Framework Core
* SQLite
* HttpClient

---

## ▶️ Como Executar o Sistema

---

### 🔧 Pré-requisitos

* .NET SDK 6 ou superior
* Visual Studio 2022

---

## 🚀 PASSO A PASSO

---

### 🥇 1. Executar a API

```
cd ApiEventos
dotnet restore
dotnet ef database update
dotnet run
```

✔ A API estará disponível em:

```
https://localhost:xxxx
```

✔ Acesse o Swagger:

```
https://localhost:xxxx/swagger
```

---

### 🥈 2. Executar o EventoSimulador

```
cd EventoSimulador
dotnet run
```

✔ O simulador irá gerar e enviar eventos automaticamente

---

### 🥉 3. Executar o EventoInterface

* Abrir o projeto no Visual Studio
* Definir como inicial
* Executar com **F5**

✔ A aplicação irá consumir e exibir os eventos

---

## 🔄 Fluxo de Funcionamento

```
1. EventoSimulador gera eventos automaticamente
2. Envia via HTTP POST → ApiEventos
3. API armazena os dados no SQLite
4. EventoInterface consome via GET
5. Usuário visualiza e gerencia os eventos
```

---

## 📌 Endpoints da API

| Método | Rota                | Descrição       |
| ------ | ------------------- | --------------- |
| GET    | /api/v1/evento      | Lista eventos   |
| GET    | /api/v1/evento/{id} | Busca por ID    |
| POST   | /api/v1/evento      | Cria evento     |
| PUT    | /api/v1/evento/{id} | Atualiza evento |
| DELETE | /api/v1/evento/{id} | Remove evento   |

---

## 📡 Exemplo de Requisição

### POST - Criar Evento

```json
{
  "tipo_Evento": "Show",
  "local_Evento": "Belo Horizonte"
}
```

---

## 🗄️ Banco de Dados

* Tipo: SQLite

Arquivos gerados:

```
eventos.db
eventos.db-shm
eventos.db-wal
```

---

## 🧪 Testes

A API pode ser testada utilizando:

* Swagger
* Postman
* EventoInterface
* EventoSimulador

---

## 🧑‍💻 Autor

Projeto acadêmico desenvolvido no SENAI com foco em:

* Desenvolvimento de APIs REST
* Integração entre aplicações
* Arquitetura em camadas
* Padrão MVVM

---

## 🔥 Conclusão

O sistema demonstra a construção de uma solução distribuída, integrando múltiplos projetos com comunicação via HTTP, persistência de dados e organização baseada em boas práticas de desenvolvimento.

