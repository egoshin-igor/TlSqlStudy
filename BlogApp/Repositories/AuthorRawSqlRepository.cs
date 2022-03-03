using BlogApp.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BlogApp.Repositories
{
    class AuthorRawSqlRepository : IAuthorRepository
    {
        private readonly string _connectionString;

        public AuthorRawSqlRepository( string connectionString )
        {
            _connectionString = connectionString;
        }

        public List<Author> GetAll()
        {
            var result = new List<Author>();

            using ( var connection = new SqlConnection( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand command = connection.CreateCommand() )
                {
                    command.CommandText = "select [Id], [Name] from [Author]";

                    using ( var reader = command.ExecuteReader() )
                    {
                        while ( reader.Read() )
                        {
                            result.Add( new Author
                            {
                                Id = Convert.ToInt32( reader["Id"] ),
                                Name = Convert.ToString( reader["Name"] )
                            } );
                        }
                    }
                }
            }

            return result;
        }

        public void Add( Author author )
        {
            using ( var connection = new SqlConnection( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand command = connection.CreateCommand() )
                {
                    command.CommandText =
                        @"insert into [Author]
                            ([Name])
                        values
                            (@name)
                        select SCOPE_IDENTITY()";

                    command.Parameters.Add( "@name", SqlDbType.NVarChar ).Value = author.Name;

                    author.Id = Convert.ToInt32( command.ExecuteScalar() );
                }
            }
        }

        public Author GetById( int id )
        {
            using ( var connection = new SqlConnection( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand command = connection.CreateCommand() )
                {
                    command.CommandText =
                        @"select [Id], [Name]
                        from [Author]
                        where [Id] = @id";

                    command.Parameters.Add( "@id", SqlDbType.Int ).Value = id;
                    using ( var reader = command.ExecuteReader() )
                    {
                        if ( reader.Read() )
                        {
                            return new Author
                            {
                                Id = Convert.ToInt32( reader["Id"] ),
                                Name = Convert.ToString( reader["Name"] )
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void Update( Author author )
        {
            using ( var connection = new SqlConnection( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand command = connection.CreateCommand() )
                {
                    command.CommandText =
                        @"update [Author]
                        set [Name] = @name
                        where [Id] = @id";

                    command.Parameters.Add( "@name", SqlDbType.NVarChar ).Value = author.Name;
                    command.Parameters.Add( "@id", SqlDbType.Int ).Value = author.Id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteById( int id )
        {
            using ( var connection = new SqlConnection( _connectionString ) )
            {
                connection.Open();
                using ( SqlCommand command = connection.CreateCommand() )
                {
                    command.CommandText =
                        @"delete [Author]
                        where [Id] = @id";

                    command.Parameters.Add( "@id", SqlDbType.Int ).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
