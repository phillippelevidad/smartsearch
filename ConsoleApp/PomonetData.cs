using SmartSearch;
using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleApp
{
    class PromonetSearchDomain : SearchDomain
    {
        public PromonetSearchDomain()
        {
            Name = "promonet";
            Fields = new IField[]
            {
                new Field("Id", FieldType.Literal, FieldRelevance.Normal),
                new Field("FullName", FieldType.Text, FieldRelevance.Higher, enableSearching: true, enableSorting: true),
                new Field("Email", FieldType.Literal, FieldRelevance.Normal, enableSearching: true),
                new Field("AddressStreet", FieldType.Text, FieldRelevance.Normal, enableSearching: true, enableSorting: true),
                new Field("AddressNumber", FieldType.Literal, FieldRelevance.Normal, enableSearching: true),
                new Field("AddressNeighborhood", FieldType.Text, FieldRelevance.Normal, enableSearching: true, enableSorting: true),
                new Field("AddressCityAndState", FieldType.Text, FieldRelevance.Normal, enableFaceting: true, enableSearching: true, enableSorting: true)
            };
        }
    }

    class PromonetDocumentProvider : IDocumentProvider
    {
        const string ConnectionString = "Server=tcp:promonetdb.cx3dpulxitxx.us-east-1.rds.amazonaws.com,1433;Initial Catalog=promonet;Persist Security Info=False;User ID=promonetdeveloper;Password=HYgXO2wta7H70r88GsrDKn2dLrsyHqDNXY0awoO3FJGERwYYStePdFgt5dlwK7UT;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
        const string Sql = @"
            SELECT
                Users.Id, Users.FullName, Users.Email,
	            Addr.Street, Addr.Number, Addr.Neighborhood, Addr.CityAndState
            FROM AspNetUsers Users
            LEFT JOIN[Address] Addr ON Users.Address_Id = Addr.Id";

        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;

        bool disposed = false;

        public IDocumentReader GetDocumentReader()
        {
            connection = new SqlConnection(ConnectionString);
            connection.Open();

            command = connection.CreateCommand();
            command.CommandText = Sql;
            command.CommandType = CommandType.Text;

            reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return new PromonetDocumentReader(reader);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                reader.Close();
                command.Dispose();
                connection.Dispose();
            }

            disposed = true;
        }
    }

    class PromonetDocumentReader : IDocumentReader
    {
        SqlDataReader reader;
        bool disposed = false;

        public IDocument CurrentDocument { get; private set; }

        public PromonetDocumentReader(SqlDataReader dataReader)
        {
            reader = dataReader;
        }

        public bool ReadNext()
        {
            if (!reader.Read())
                return false;

            CurrentDocument = new Document(reader["Id"].ToString(), new Dictionary<string, object>
            {
                { "Id", reader.Get<string>("Id") },
                { "FullName", reader.Get<string>("FullName") },
                { "Email", reader.Get<string>("Email") },
                { "AddressStreet", reader.Get<string>("Street") },
                { "AddressNumber", reader.Get<string>("Number") },
                { "AddressNeighborhood", reader.Get<string>("Neighborhood") },
                { "AddressCityAndState", reader.Get<string>("CityAndState") }
            });

            return true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
                reader.Close();

            disposed = true;
        }
    }

    public static class IDataReaderExtensions
    {
        public static T Get<T>(this IDataReader reader, string name)
        {
            var value = reader[name];
            if (value is DBNull) value = null;
            return (T)value;
        }
    }
}
