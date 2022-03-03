using BlogApp.Models;
using BlogApp.Repositories;

using System;
using System.Collections.Generic;

namespace BlogApp
{
    class Program
    {
        private static string _connectionString = @"Data Source=IGOR-EGOSHIN-LA\SQLEXPRESS;Initial Catalog=Test;Pooling=true;Integrated Security=SSPI";

        static void Main( string[] args )
        {
            IAuthorRepository authorRepository = new AuthorRawSqlRepository( _connectionString );

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
                    List<Author> authors = authorRepository.GetAll();
                    foreach ( Author author in authors )
                    {
                        Console.WriteLine( $"Id: {author.Id}, Name: {author.Name}" );
                    }
                }
                else if ( command == "add-author" )
                {
                    Console.WriteLine( "Введите имя автора" );
                    string name = Console.ReadLine();

                    authorRepository.Add( new Author
                    {
                        Name = name
                    } );
                    Console.WriteLine( "Успешно добавлено" );
                }
                else if ( command == "update-author" )
                {
                    Console.WriteLine( "Введите id автора" );
                    int authorId = int.Parse( Console.ReadLine() );
                    Author author = authorRepository.GetById( authorId );

                    if ( author == null )
                    {
                        Console.WriteLine( "Автор не найден" );
                        continue;
                    }

                    Console.WriteLine( "Введите новое имя автора" );
                    author.Name = Console.ReadLine();

                    authorRepository.Update( author );
                    Console.WriteLine( "Успешно обновлено" );
                }
                else if ( command == "delete-author" )
                {
                    Console.WriteLine( "Введите id автора" );
                    int authorId = int.Parse( Console.ReadLine() );
                    var author = authorRepository.GetById( authorId );
                    if ( author == null )
                    {
                        Console.WriteLine( "Автор не найден" );
                        continue;
                    }

                    authorRepository.DeleteById( author.Id );
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
    }
}
