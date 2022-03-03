using BlogApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BlogApp
{
    class Program
    {
        private static string _connectionString = @"Data Source=IGOR-EGOSHIN-LA\SQLEXPRESS;Initial Catalog=Test;Pooling=true;Integrated Security=SSPI";

        static void Main( string[] args )
        {
            Console.WriteLine( "Доступные команды:" );
            Console.WriteLine( "get-authors - показать список авторов блога" );
            Console.WriteLine( "add-author - добавить автора" );
            Console.WriteLine( "update-author - изменить автора" );
            Console.WriteLine( "delete-author - удалить автора" );
            Console.WriteLine( "exit - выйти из приложения" );

            while ( true )
            {
                string command = Console.ReadLine();

                if ( command == "get-authors" )
                {
                    List<Author> authors = GetAuthors();
                    foreach ( Author author in authors )
                    {
                        Console.WriteLine( $"Id: {author.Id}, Name: {author.Name}" );
                    }
                }
                else if ( command == "add-author" )
                {
                    Console.WriteLine( "Введите имя автора" );
                    string name = Console.ReadLine();

                    AddAuthor( new Author
                    {
                        Name = name
                    } );
                    Console.WriteLine( "Успешно добавлено" );
                }
                else if ( command == "update-author" )
                {
                    Console.WriteLine( "Введите id автора" );
                    int authorId = int.Parse( Console.ReadLine() );
                    Author author = GetAuthorById( authorId );

                    if ( author == null )
                    {
                        Console.WriteLine( "Автор не найден" );
                        continue;
                    }

                    Console.WriteLine( "Введите новое имя автора" );
                    author.Name = Console.ReadLine();

                    UpdateAuthor( author );
                    Console.WriteLine( "Успешно обновлено" );
                }
                else if ( command == "delete-author" )
                {
                    Console.WriteLine( "Введите id автора" );
                    int authorId = int.Parse( Console.ReadLine() );
                    var author = GetAuthorById( authorId );
                    if ( author == null )
                    {
                        Console.WriteLine( "Автор не найден" );
                        continue;
                    }

                    DeleteAuthorById( author.Id );
                    Console.WriteLine( "Успешно удалено" );
                }
                else if ( command == "exit" )
                {
                    return;
                }
                else
                {
                    Console.WriteLine( "Команда не найдена" );
                }
            }
        }

        public static List<Author> GetAuthors()
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

        public static void AddAuthor( Author author )
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

        public static Author GetAuthorById( int id )
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

        public static void UpdateAuthor( Author author )
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

        public static void DeleteAuthorById( int id )
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
