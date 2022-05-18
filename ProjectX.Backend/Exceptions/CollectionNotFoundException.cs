using System;

namespace ProjectX.Backend.Exceptions
{
    public class CollectionNotFoundException : Exception
    {
        public CollectionNotFoundException(string name, string dbName)
            : base($"The collection {name} doesn't exist in the database {dbName}")
        {

        }
    }
}