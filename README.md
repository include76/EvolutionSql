# EvolutionSql
Simple dot net database access library, if you don't like Full-ORM framework such as EntityFramework, and you want to write your own sql and/or stored procedure, EvolutionSql is what you want.

by using EvolutionSql, it's very simple to execute either inline sql or stored procedure; EvolutionSql extend DbConnection with two methods ```Sql()``` and ```Procedure()```, for execute inline sql and stored procedure respectively.

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
## Result mapping
When query from database, column name are auto mapped to property of modal, the following two pattern are legal, and both case-insensitive
1. column name and property name are same: FirstName -> FirstName
2. column name with under score: first_name -> FirstName

## Stored Procedure parameter mapping mysql sample
###### use anonymous type as parameter
```C#
    var userFromDb = connection.Procedure("usp_user_get")                    
                    .QueryOne<User>(new { pUserId = userId });

```
```SQL
  DROP PROCEDURE IF EXISTS usp_user_get;
  DELIMITER //
  CREATE PROCEDURE usp_user_get(
    IN pUserId CHAR(36)
  )
  BEGIN
    SELECT * FROM `user` 
      WHERE user_id = pUserId;
  END//
  DELIMITER ;
```

###### use named type as parameter
```C#
    userFromDb.FirstName = "Bob";
    userFromDb.UpdatedBy = "system";
    userFromDb.UpdatedOn = DateTime.Now;
    connection.Procedure("usp_user_upd")
        .ParameterPrefix("p_")// call this to indicate that the stored procedure parameters have p_ prefix
        .Execute(userFromDb);

```
```SQL
  DROP PROCEDURE IF EXISTS usp_user_upd;
  DELIMITER //
  CREATE PROCEDURE usp_user_upd(
    p_user_id CHAR(36),
    p_first_name VARCHAR(256),
    p_last_name VARCHAR(256),
    p_updated_by VARCHAR(256),
    p_updated_on DATETIME
  )
  BEGIN
    UPDATE `user`
      SET first_name = p_first_name,
        last_name = p_last_name,
              updated_by = p_updated_by,
              updated_on = p_updated_on
    WHERE user_id = p_user_id;
  END//
  DELIMITER ;
```
