# EvolutionSql
Simple dot net database access library, if you don't like Full-ORM framework like EntityFramework, and you want to write your own sql and/or stored procedure, EvolutionSql is what you want.

by using EvolutionSql, it's very simple to execute either inline sql or stored procedure; EvolutionSql extend DbConnection with two methods Sql() and Procedure(), for execute inline sql and stored procedure respectively.

## Supported Database
- [x] Mysql
- [x] SqlServer
- [ ] PostgreSql in near furture

## Sample
```c#

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
      LastName = "Lee",
      CreatedOn = DateTime.Now
  };
  //
  using (var connection = new SqlConnection(connectionStr))
  {
    var result = connection.Sql(@"insert into [user](UserId, FirstName, LastName, CreatedOn) 
                                  values(@UserId, @FirstName, @LastName, @CreatedOn)").Execute(user);
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
  using (var connection = new SqlConnection(connectionStr))
  {
      var userFromDb = connection.Procedure("uspUserGet").Query<User>(new { UserId = userId }).FirstOrDefault();
  }
  
  // if you want the output value from the stored procedure
  var outPuts = new Dictionary<string, dynamic>();
  using (var connection = new SqlConnection(connectionStr))
  {
      var userFromDb = connection.Procedure("uspUserGet").Query<User>(new { UserId = userId }, outPuts).FirstOrDefault();
  }
```
