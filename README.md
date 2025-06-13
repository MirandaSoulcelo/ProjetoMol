# Algumas Observações


   * ao clonar o projeto na máquina, digite o comando dotnet restore na raiz do projeto para que os pacotes sejam instalados corretamente

   * o comando dotnet run deve ser feito dentro do diretório do projeto theProjec.WebApi

   * alguns Endpoints estão protegidos pela tag authorize. Faça login para conseguir testá-los

   * O Endpoint Add User não está protegido, isso facilitará testes via swagger

   * Após Efetuar Login, um Token será gerado, o comando Bearer tokengerado deve ser colado no campo Authorize do Swagger

  

# Teste Desenvolvedor C#


   * O projeto foi criado usando o .net 8.0
   * Fique a vontade para usar um banco de dados local ou em memória, o importante é ter as implementações solicidas neste documento.
   * A estrutura do projeto já está pronta, basta implementar os recursos necessários para atender aos requisitos.

# Atividades

   # IoC
   * Implementar injeção de dependência no projeto.

   # Autenticação
   * Implementar autenticação JWT. Para esse projeto não é necessário criar o REVOKE TOKEN.
   * O Token deve ter 15 minutos de duração.

   # Swagger UI
   * Implementar documentação do Swagger no projeto.

   # MediatR
   * Utilizar a biblioteca MediatR para comunicação entre controllers e classes.

   # Autenticador
   * Implementar um endpoint chamado Login no AuthController. Este endpoint deverá ter as seguintes implementações: 
      * Chamar a handler (usando MediatR) resposável por validar o usuário e retornar um token de autenticação válido.
         * A handler que irá recuperar o token deverá:
            * Receber como parâmetro o e-mail e senha do usuário na classe de requisição.
            * Validar se os parâmetros informados são válidos.
            * Validar se as credenciais do usuário estão corretas através de uma consulta ao banco.
            * Caso não seja encontrado nenhum usuário para o e-mail e senha informados deverá retornar uma mensagem dizendo "Nenhum usuário encontrado"
            * Caso seja entrado um usuário para o e-mail e senha informados deverá retornar o token de autenticação.
               * O token de autenticação deverá ter uma claim chamada user com as seguintes informações: name e e-mail.

   # Categories
   * Implementar um endpoint chamado GetAll
      * Chamar a handler (usando MediatR) responsáveis por recuperar uma lista de categorias.
         * A handler que irá recuperar as categorias deverá:
            * Não receber nenhum parâmetro na classe de requisição.
            * Recuperar todas as categorias cadastradas no banco de dados.
            * Retornar uma lista de objetos contendo Id e Name.

   * Implementar um endpoint chamado Add
      * A handler que irá adicionar uma nova categoria deverá:
         * Receber Name como parâmetro na classe de requisição.
         * Validar a classe de requisição.
            * Retornar mensagem de "Nome não informado ou inválido" caso o nome não tenha sido preenchido.
         * Validar duplicidade, ou seja, verificar se já existe uma categoria com o mesmo nome informado antes de cadastrar.
            * Caso já exista uma categoria para o nome informado retornar a mensagem "Já existe uma categoria para o nome informado".
         * Salvar a categoria no banco de dados.

   # Products
   * Implementar um endpoint chamado GetAll
      * A handler que irá recuperar os produtos deverá:
         * Receber search, page e pageSize como parâmetros.
            * Page por padrão deve ser 1.
            * PageSize por padrão deve ser 10.
         * Implementar paginação utilizando os parâmetros page (página atual) e pageSize (quantidade por página).
         * Implementar filtro de busca caso a propriedade search possua valor, ou seja, filtrar os resultados pelo termo de busca em search. O filtro deverá ser aplicado na propriedade Name.
         * Retornar uma lista de produtos paginada contendo as seguintes propriedades: Id, Name e StockQuantity.

   * Implementar um endpoint chamado Add
      * A handler que irá adicionar um novo produto deverá:
         * Receber CategoryId, Name, UnitPrice, StockQuantity e Status como parâmetros na classe de requisição.
         * Validar a classe de requisição.
            * Se CategoryId é maior que 0, caso seja menor ou igual a zero (ID inválido no banco de dados) retornar a mensagem "Código da categoria não informado ou inválido".
            * Se Name foi preenchido. Retornar mensagem "Nome não informado ou inválido" caso o nome não tenha sido preenchido.
            * Se UnitPrice é maior que 0, caso seja menor ou igual a zero retornar a mensagem "Preço Unitário não informado ou inválido".
            * Se StockQuantity é maior ou igual a 0, caso seja menor que zero retornar a mensagem "Quantidade em estoque não informada ou inválida".
         * Validar se categoria informada existe, ou seja, buscar no banco se existe uma categoria para o CategoryId informado. Retornar a mensagem "Categoria informada não encontrada" caso não seja econtrada uma categoria para o CategoryId informado.
         *  Validar duplicidade, ou seja, verificar se já existe um produto para o nome informado. Retornar "Já existe um produto para o nome informado" caso já exista um produto com o mesmo nome.
         * Salvar o produto no banco de dados.

   * Implementar um endpoint chamado Update
      * A handler que irá atualizar um produto existente deverá:
         * Receber Id, CategoryId, Name, UnitPrice, StockQuantity e Status como parâmetros na classe de requisição.
         * Validar a classe de requisição.
            * Se Id é maior que 0, caso seja menor ou igual a zero (ID inválido no banco de dados) retornar a mensagem "Código do produto não informado ou inválido".
            * Se CategoryId é maior que 0, caso seja menor ou igual a zero (ID inválido no banco de dados) retornar a mensagem "Código da categoria não informado ou inválido".
            * Se Name foi preenchido. Retornar mensagem "Nome não informado ou inválido" caso o nome não tenha sido preenchido.
            * Se UnitPrice é maior que 0, caso seja menor ou igual a zero retornar a mensagem "Preço Unitário não informado ou inválido".
            * Se StockQuantity é maior ou igual a 0, caso seja menor que zero retornar a mensagem "Quantidade em estoque não informada ou inválida".
         * Validar se o produto informado existe, ou seja, buscar no banco se o produto informado (Id) existe no banco de dados. Retornar a mensagem "Produto informado não encotrado" caso não seja encontrado um produto para o Id informado.
         * Validar se categoria informada existe, ou seja, buscar no banco se existe uma categoria para o CategoryId informado. Retornar a mensagem "Categoria informada não encontrada" caso não seja econtrada uma categoria para o CategoryId informado.
         *  Validar duplicidade, ou seja, verificar se já existe um produto para o nome informado. Retornar "Já existe um produto para o nome informado" caso já exista um produto com o mesmo nome.
         * Atualizar o produto no banco de dados.

   * Implementar um endpoint chamado Delete
      * A handler que irá deletar um produto existete deverá:
         * Receber Id como parâmetro na classe de requisição.
         * Validar classe de requisição.
            * Se Id é maior que 0, caso seja menor ou igual a zero (ID inválido no banco de dados) retornar a mensagem "Código do produto não informado ou inválido".
         * Validar se o produto informado existe, ou seja, buscar no banco se o produto informado (Id) existe no banco de dados. Retornar a mensagem "Produto informado não encotrado" caso não seja encontrado um produto para o Id informado.
         * Excluir o produto do banco de dados.dotnet

   # Testes (opcional)
   * Implementar testes unitários para os recursos necessários por adicionar um novo produto ao banco de dados. 
  