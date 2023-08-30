using System;

namespace MekaruStudios.MonsterCreator.FileSaving
{
    public class FilePathNotValidException : Exception
    {
        public FilePathNotValidException()
        {
        }

        public FilePathNotValidException(string message) : base(message)
        {
        }

    }
}
