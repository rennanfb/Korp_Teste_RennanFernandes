# Sistema de Notas Fiscais com Microsserviços

## Visão Geral
<p>
Este projeto é composto por dois microsserviços independentes desenvolvidos com ASP.NET Core:
</p>

<ul>
  <li><b>Serviço de Estoque</b>: responsável pelo gerenciamento de produtos e controle de estoque</li>
  <li><b>Serviço de Faturamento</b>: responsável pela criação, atualização e validação de notas fiscais através do Serviço de Estoque</li>
</ul>

<p>
O sistema demonstra comunicação entre microsserviços via HTTP, tratamento de concorrência, tolerância a falhas e integração com frontend em Angular.
</p>

---

## Arquitetura

<pre>
Frontend (Angular)
        ↓
Serviço de Faturamento
        ↓ (HTTP)
Serviço de Estoque
        ↓
SQL Server
</pre>

<p>
Cada serviço é totalmente independente e se comunica exclusivamente por meio de APIs REST. Não há compartilhamento de banco de dados entre os serviços.
</p>

---

## Serviço de Estoque

<p><b>Responsável pelo gerenciamento de produtos e estoque.</b></p>

### Funcionalidades
<ul>
  <li>Criar, atualizar e excluir produtos</li>
  <li>Consultar dados de produtos</li>
  <li>Reduzir estoque com lógica segura para concorrência</li>
</ul>

### Endpoints
<pre>
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
POST   /api/products/{id}/decrease
</pre>

---

## Serviço de Faturamento

<p><b>Responsável pelo ciclo de vida das notas fiscais e validações.</b></p>

### Funcionalidades
<ul>
  <li>Criar notas fiscais com múltiplos itens</li>
  <li>Atualizar dados da nota fiscal</li>
  <li>Excluir notas fiscais</li>
  <li>Validar produtos via Serviço de Estoque</li>
  <li>Gerar número da nota automaticamente</li>
  <li>Finalizar (imprimir) notas com validação de estoque</li>
  <li>Tratar indisponibilidade do Serviço de Estoque</li>
</ul>

### Endpoints
<pre>
POST   /api/invoices
GET    /api/invoices
GET    /api/invoices/{id}
PUT    /api/invoices/{id}
DELETE /api/invoices/{id}
POST   /api/invoices/{id}/print
GET    /api/invoices/test-concurrency/{productId}
</pre>

---

## Frontend (Angular)

### Funcionalidades
<ul>
  <li>Criar produtos</li>
  <li>Criar notas fiscais com múltiplos itens</li>
  <li>Finalizar (imprimir) notas fiscais</li>
  <li>Estados de carregamento para operações assíncronas (criação e impressão)</li>
  <li>Feedback básico ao usuário e validações</li>
</ul>

---

## Comunicação entre Serviços

<p>O Serviço de Faturamento se comunica com o Serviço de Estoque utilizando HttpClient.</p>

<ul>
  <li>Validação da existência de produtos</li>
  <li>Recuperação de dados de produtos</li>
  <li>Verificação e redução de estoque</li>
</ul>

---

## Tratamento de Concorrência

<p>As atualizações de estoque são feitas utilizando operações atômicas no banco de dados para evitar condições de corrida (race conditions).</p>

<ul>
  <li>Evita valores negativos de estoque</li>
  <li>Garante atualizações seguras em cenários concorrentes</li>
  <li>Mantém consistência em requisições paralelas</li>
</ul>

---

## Tolerância a Falhas

<p>Quando o Serviço de Estoque está indisponível:</p>

<ul>
  <li>O Serviço de Faturamento retorna HTTP 503 (Service Unavailable)</li>
  <li>As operações de nota fiscal são bloqueadas para evitar inconsistência de dados</li>
</ul>

---

## Teste de Concorrência

<pre>
GET /api/invoices/test-concurrency/{productId}
</pre>

<p>
Este endpoint simula múltiplas requisições simultâneas para validar o tratamento de concorrência no estoque.
</p>

---

## Observações sobre o Modelo de Dados

<ul>
  <li>Itens da nota fiscal armazenam ProductId, Quantity e ProductDescription</li>
  <li>Os dados do produto são obtidos do Serviço de Estoque no momento da criação</li>
  <li>Não há banco de dados compartilhado entre os microsserviços</li>
</ul>

---

## Fluxo de Funcionamento

<ol>
  <li>O usuário envia uma requisição de criação de nota fiscal pelo frontend</li>
  <li>O Serviço de Faturamento valida os itens via Serviço de Estoque</li>
  <li>Os dados do produto são obtidos e armazenados no item da nota</li>
  <li>O estoque é reduzido de forma atômica ao finalizar a nota</li>
  <li>A nota é persistida no SQL Server</li>
  <li>A resposta é retornada ao cliente</li>
</ol>

---

## Tecnologias Utilizadas

<ul>
  <li>ASP.NET Core Web API</li>
  <li>Entity Framework Core</li>
  <li>SQL Server</li>
  <li>AutoMapper</li>
  <li>HttpClient</li>
  <li>Angular</li>
  <li>Swagger</li>
</ul>

---

## Detalhamento Técnico

### Ciclo de Vida do Angular

<p>
O lifecycle hook <b>ngOnInit</b> foi utilizado para realizar o carregamento inicial dos dados quando os componentes são inicializados, como a busca de produtos e notas fiscais a partir das APIs.
</p>

### Uso de RxJS

<p>
O RxJS é utilizado de forma indireta através do HttpClient do Angular. Todas as requisições HTTP retornam Observables, que são consumidos utilizando <b>subscribe()</b> para tratar respostas e erros de forma assíncrona.
</p>

### Bibliotecas Utilizadas

<ul>
  <li><b>HttpClient (Angular)</b>: comunicação com as APIs do backend</li>
  <li><b>ASP.NET Core Web API</b>: implementação dos serviços backend</li>
  <li><b>Entity Framework Core</b>: acesso e persistência de dados no banco</li>
  <li><b>AutoMapper</b>: mapeamento entre entidades e DTOs</li>
  <li><b>Swagger</b>: documentação e testes dos endpoints da API</li>
</ul>

### Componentes Visuais

<p>
Nenhuma biblioteca externa de UI foi utilizada. O frontend foi construído com componentes padrão do Angular, HTML e CSS customizado.
</p>

### Gerenciamento de Dependências

<p>
As dependências do backend são gerenciadas via NuGet e definidas nos arquivos <b>.csproj</b> de cada microsserviço.
</p>

### Frameworks Utilizados no Backend

<p>
O backend foi desenvolvido utilizando ASP.NET Core Web API em conjunto com Entity Framework Core.
</p>

### Tratamento de Erros

<p>
O tratamento de erros é realizado utilizando blocos try/catch em operações críticas. Em falhas de comunicação com o Serviço de Estoque, exceções como <b>HttpRequestException</b> são tratadas e uma resposta <b>503 Service Unavailable</b> é retornada para evitar inconsistência de dados.
</p>

### Uso de LINQ

<p>
O LINQ é utilizado em conjunto com o Entity Framework Core para consultas ao banco de dados, incluindo métodos como <b>AnyAsync</b>, <b>FindAsync</b> e operações de filtragem para validação e recuperação de dados.
</p>

---

## Observações Finais

<ul>
  <li>Os microsserviços são totalmente independentes</li>
  <li>A comunicação ocorre exclusivamente via HTTP</li>
  <li>Não há banco de dados compartilhado entre os serviços</li>
  <li>Os dados de produtos são obtidos via comunicação entre APIs</li>
  <li>O frontend inclui melhorias básicas de experiência do usuário, como estados de carregamento</li>
</ul>