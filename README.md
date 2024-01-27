# Durable Orchestrations - Aprovação de pedidos - Fase 2 - Parte 1 - POSTECH
TECH-CHALLENGE Durable Orchestrations - Aprovação de pedidos desenvolvido na Fase 2 da POSTECH Arquitetura de Sistemas .NET com Azure

# Projeto Aprovação de Pedidos
É uma API que permite simular o registro e acompanhamento de pedidos. A solução foi desenvolvida utilizando a linguagem C# na versão 6.0 do .NET Framework e Banco de Dados Azure Table Storage.

## Requisitos:
Disponibilizar endpoints para inserção de produtos, listagem de produtos, adicionar produtos ao carrinho de compras, aprovar ordem de compra, enviar ordem de compra para entrega e finalizar pedido durante o processo de simulação de aprovação pedidos.

## Critérios de aceite:
Para uso da aplicação, o usuário poderá inserir produtos em:

POST /api/HttpInsertProduct

Poderá ser informado os campos, conforme exemplo abaixo:
```
{
    "Name": "Product 1",
    "Price": 50.00
}
```

Para listar os produtos:

GET /api/ListProducts

Para adicionar produtos ao carrinho de compras:

POST /api/HttpAddToCart

Deverá ser informado obrigatoriamente os campos id do produto e a sua quantidade, conforme exemplo abaixo:
```
{
    "Items": [
        {
            "ProductId": "3a6473a2-9852-46b7-a2a3-c1b299af3db1",
            "Quantity": 2
        },
        {
            "ProductId": "50e89af1-55f5-4422-b805-d621e1c2edbf",
            "Quantity": 10
        }
    ]
}
```

E caso queira inserir mais itens no carrinho, basta informar o id do carrinho gerado no momento da primeira inserção de produtos ao carrinho e colocar os novos produtos conforme exemplo abaixo:
```
{
    "Id": "9f502800-e0b2-4519-8652-6a1c618ae85a",
    "Items": [
        {
            "ProductId": "3a6473a2-9852-46b7-a2a3-c1b299af3db1",
            "Quantity": 3
        },
        {
            "ProductId": "50e89af1-55f5-4422-b805-d621e1c2edbf",
            "Quantity": 1
        }
    ]
}
```


Para aprovar a ordem de compra:

POST /api/HttpApproveOrder

Deverá ser informado os campos id do carrinho, consumidor e endereço conforme exemplo abaixo:
```
{
    "CartId": "f1a1ca82-8a72-46e5-b91f-5cbde22f0978",
    "Consumer": "bill@gmail.com",
    "Address": "Rua abc, 123, Jd. Codelândia, São Paulo-SP"
}
```

Para enviar a ordem de compra:

POST /api/HttpSendOrder

Deverá ser informado obrigatoriamente o campo Id gerado no passo anterior, conforme exemplo abaixo:
```
{
    "Id": "c9dab7bb-1d46-436e-b58b-a5243643867e"
}
```

Para finalizar a ordem de compra para o status de entregue ao cliente:

POST /api/HttpCompleteOrder

Deverá ser informado obrigatoriamente o campo Id gerado no passo anterior, conforme exemplo abaixo:
```
{
    "Id": "c9dab7bb-1d46-436e-b58b-a5243643867e"
}
```


## Execução:

Para execução do projeto utilizando o VSCode, faz-se necessário estar logado na sua conta da Azure, após isso será solicitado a conexão a uma Storage Account, criar ou selecionar uma Storage Account, criar ou selecionar um grupo de recursos e selecionar uma localização.

Para realizar testes com a API, pode-se utilizar o POSTMAN:

- POSTMAN (https://www.postman.com/)

Rodar os códigos conforme exemplos mencionados para cada endpoint disponibilizado na aplicação.

## Banco de dados:

O Banco de dados utilizado na API é o Azure Table Storage onde as tabelas serão criadas conforme forem ocorrendo os processos de inserção e para verificar a string de conexão, basta acessar o arquivo "DurableFunctionProject/local.settings.json" e verá que foi adicionado no campo "AzureWebJobsStorage" após rodar o projeto de simulação.


## Links úteis

- https://learn.microsoft.com/en-us/azure/azure-functions/functions-overview?pivots=programming-language-csharp

- https://www.serverless360.com/blog/azure-durable-functions-patterns-best-practices

- https://renatogroffe.medium.com/serverless-azure-functions-guia-de-refer%C3%AAncia-4a3abd496ef0