# EvolutionSql
Simple dot net decoration style database access library, if you don't like Full-ORM framework like EntityFramework, and you want to write your own sql and/or stored procedure, EvolutionSql is what you want.

## Supported Database
- [x] Mysql
- [x] SqlServer
- [ ] PostgreSql in near furture

## Core component
1. CommandAttribute, attribute to decorate your modals, define your command <br/>
2. SqlSession, which manage connection, transaction and execute your command

## Smaple
```c#
  //define your command via CommandAttribute on you modal
  [Command(Name = "Insert"
        , Text = @"insert into [user](UserId, FirstName, LastName) values(@UserId, @FirstName, @LastName);"
        , CommandType = CommandType.Text)]
  [Command(Name = "uspUserGet", Text = "uspUserGet")]
  public class User
  {
      public Guid UserId { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }

      public DateTime CreatedOn { get; set; }
      public DateTime UpdatedOn { get; set; }
  }
```

###### insert sample with inline sql
```c#
  var user = new User
  {
      UserId = Guid.NewGuid(),
      FirstName = "Bruce",
      LastName = "Lee"
  };
  //
  using (var sqlSession = new SqlSession(new SqlConnection(connectionStr)))
  {
    var result = sqlSession.Execute<User>("insert", user);
  }
```

###### get sample with stored procedure
  ```sql
  create procedure uspUserGet(
    @userId uniqueidentifier,
    @totalCount int out
  )
  as
  begin
    select * from [user] where userid = @userId
    select @totalCount = count(1) from [user]
  end
  ```
  
  ```c#
  using (var sqlSession = new SqlSession(new SqlConnection(connectionStr)))
  {
    var userFromDb = sqlSession.QueryOne<User>("uspUserGet", new { UserId = userId });
  }
  
  // if you want the output value from the stored procedure
  var outPuts = new Dictionary<string, dynamic>();
  using (var sqlSession = new SqlSession(new SqlConnection(connectionStr)))
  {
    var userFromDb = sqlSession.QueryOne<User>("uspUserGet", new { UserId = userId }, out outPuts);
  }
```
