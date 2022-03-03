using System.Collections.Generic;
using BlogApp.Models;

namespace BlogApp.Repositories
{
    interface IAuthorRepository
    {
        void Add( Author author );
        void DeleteById( int id );
        Author GetById( int id );
        List<Author> GetAll();
        void Update( Author author );
    }
}