<h1 align="center">Blog</h1>
<h4>Projeto com conteúdo disponibilizado no <strong><a href="https://balta.io/">balta.io</a> </strong></h4>

<br />

# Iniciando o projeto
Para rodar o projeto você precisa antes de algumas ferramentas instaladas:
* uma IDE de preferência
#### Ferramentas
* Docker para rodar o Sql 
* Projeto está em .NET 5.0
#### Alguns pacotes do NuGet como:
* Microsoft.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.Design
* Microsoft.EntityFrameworkCore.SqlServer
* Microsoft.EntityFrameworkCore.Tools

# Rodando o projeto:
```bash
dotnet run
dotnet watch
```
* Exemplo para verificar todos endpoints
```bash
http://localhost:5201/swagger/index.html
````

## Clonando o projeto
```bash
git clone https://github.com/Romulo-Queiroz/Blog-c-.git
```

## Dependencias para conexão com Sql Server 
- Você vai precisar se conectar com o Banco de Dados Sql e para isso antes terá que ter o Docker rodando o container.
- <a href="https://balta.io/blog/sql-server-docker" target="_blank"> Como rodar SQL server no Docker</a>


Após ler o artigo basta fazer o primeiro <strong> migration </strong> e o primeiro <strong>update</strong> para lançar no banco segue abaixo instruções(Entity frame work core):
```bash
Add-Migration NOME-DA-MIGRAÇÃO -Context BancoContext
```

```bash
Update-Database -Context BancoContext
```


# Habilidades utilizadas

 - **C#**
 
 - **Sql Server**
 
 - **Docker** 
 
 - **.NET 5.0**
 

# Autor
<div align="center">

| [<img src="https://github.com/Romulo-Queiroz.png?size=115" width=115><br><sub>@Romulo-Queiroz</sub>](https://github.com/Romulo-Queiroz) |
| :-------------------------------------------------------------------------------------------------------------------------------------: |

</div>
