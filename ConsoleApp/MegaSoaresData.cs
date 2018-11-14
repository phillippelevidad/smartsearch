using Dapper;
using SmartSearch;
using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ConsoleApp
{
    class MegaSoaresSearchDomain : SearchDomain
    {
        const string VariantTypes =
            "Acessórios,Alimentação,Aparelhos Fitness,Bocas,Câmera,Câmera Frontal,Capacidade,Celular,Console,Cor,Disco Rígido,Eletrodomésticos," +
            "Eletrônicos,Esporte & Lazer,Ferramentas,Função,Gamer,Infantil,Informática,Marca,Memória Interna,Memória RAM," +
            "Móveis,Pás,Polegadas,Portas,Portáteis,Potência,Processador,Quantidade,Serviços,Sistema Operacional,Tamanho,Tampas,Telefone," +
            "Tigelas,Tipo,Utilidades Domésticas,Velocidade,Voltagem";

        public MegaSoaresSearchDomain()
        {
            var variantFields = VariantTypes.Split(',')
                .Select(v => v.Trim())
                .Select(v => new Field(v, FieldType.TextArray, FieldRelevance.Normal, enableFaceting: true, enableSearching: true));

            var fixedFields = new IField[]
            {
                new Field("Id", FieldType.Literal, FieldRelevance.Normal),
                new Field("CustomId", FieldType.Literal, FieldRelevance.Normal),
                new Field("FriendlyUrl", FieldType.Literal, FieldRelevance.Normal),
                new Field("Title", FieldType.Text, FieldRelevance.Higher, enableSearching: true),
                new Field("Description", FieldType.Text, FieldRelevance.Normal, enableSearching: true),
                new Field("Specifications", FieldType.Text, FieldRelevance.Normal, enableSearching: true),
                new Field("Price", FieldType.Double, FieldRelevance.Normal),
                new Field("CoverImageUrl", FieldType.Literal, FieldRelevance.Normal),
                new Field("CurrentStock", FieldType.Int, FieldRelevance.Normal),
                new Field("CategoryId", FieldType.Int, FieldRelevance.Normal),
                new Field("CategoryName", FieldType.Text, FieldRelevance.High, enableSearching: true),
                new Field("SellerId", FieldType.Literal, FieldRelevance.Normal),
                new Field("SellerEmail", FieldType.Literal, FieldRelevance.Normal, enableSearching: true),
                new Field("SellerName", FieldType.Text, FieldRelevance.Normal, enableSearching: true),
                new Field("SellerPublicName", FieldType.Text, FieldRelevance.Normal, enableSearching: true)
            };

            Name = "MegaSoares";
            Fields = fixedFields.Union(variantFields).ToArray();
            AnalysisSettings = new AnalysisSettings
            {
                Synonyms = new[]
                {
                    new[] { "qrcode", "megasoares", "mega soares", "ms" },
                    new[] { "loja", "store" },
                    new[] { "branco", "white" },
                    new[] { "frizer", "freezer" },
                }
            };
        }
    }

    class MegaSoaresDocumentProvider : IDocumentProvider
    {
        MegaSoaresDocumentReader reader;
        bool disposed = false;

        public IDocumentReader GetDocumentReader()
        {
            reader = new MegaSoaresDocumentReader();
            return reader;
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
                if (reader != null)
                    reader.Dispose();
            }

            disposed = true;
        }
    }

    class MegaSoaresDocumentReader : IDocumentReader
    {
        const string ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=megasoareshlog-2018-5-14-10-31;Integrated Security=True;";
        const string Sql = @"
            SELECT
	            Product.Id, Product.CustomId, Product.FriendlyUrl, Product.Title, Product.[Description], Product.Specifications, Product.Price, Product.CoverImageUrl, Product.CurrentStock,
	            Category.Id CategoryId, Category.[Name] CategoryName,
	            Seller.Id SellerId, Seller.Email SellerEmail, Seller.FullName SellerName, Seller.AlternativeName SellerPublicName
            FROM Product
            INNER JOIN ProductCategory Category ON Product.Category_Id = Category.Id
            INNER JOIN Store ON Product.Store_SellerId = Store.SellerId
            INNER JOIN AspNetUsers Seller ON Store.SellerId = Seller.Id
            WHERE Product.Deleted = 0
	            AND Seller.[Enabled] = 1

            SELECT
	            ProductVariantProduct.Product_Id ProductId,
	            ProductVariantType.[Name] VariantType,
	            ProductVariant.[Name] VariantName
            FROM ProductVariantType
            INNER JOIN ProductVariant ON ProductVariantType.Id = ProductVariant.[Type_Id]
            INNER JOIN ProductVariantProduct ON ProductVariant.Id = ProductVariantProduct.ProductVariant_Id
            WHERE
	            ProductVariantType.Deleted = 0
	            AND ProductVariant.Deleted = 0
            ORDER BY
	            ProductVariantType.[Order],
	            ProductVariant.[Order]";

        bool disposed = false;
        List<Product> products;
        int index = -1;

        public IDocumentOperation CurrentDocument { get; private set; }

        void ReadAllProducts()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var grids = conn.QueryMultiple(Sql);
                var products = grids.Read<Product>();
                var variants = grids.Read<Variant>();

                foreach (var p in products)
                    p.Variants = variants
                        .Where(v => v.ProductId == p.Id)
                        .GroupBy(v => v.VariantType)
                        .ToDictionary(
                            group => group.Key,
                            group => group.Select(g => g.VariantName).ToArray());

                this.products = products.ToList();
            }
        }

        public bool ReadNext()
        {
            if (products == null)
                ReadAllProducts();

            if (++index >= products.Count)
                return false;

            SetCurrentDocumentFromProduct(products[index]);
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
            {
                if (products != null)
                    products.Clear();

                products = null;
            }

            disposed = true;
        }

        void SetCurrentDocumentFromProduct(Product p)
        {
            var values = new Dictionary<string, object>
            {
                { "Id", p.Id.ToString() },
                { "CustomId", p.CustomId },
                { "FriendlyUrl", p.FriendlyUrl },
                { "Title", p.Title },
                { "Description", p.Description },
                { "Specifications", p.Specifications },
                { "Price", p.Price },
                { "CoverImageUrl", p.CoverImageUrl },
                { "CurrentStock", p.CurrentStock },
                { "CategoryId", p.CategoryId },
                { "CategoryName", p.CategoryName },
                { "SellerId", p.SellerId },
                { "SellerEmail", p.SellerEmail },
                { "SellerName", p.SellerName },
                { "SellerPublicName", p.SellerPublicName }
            };

            foreach (var variant in p.Variants)
                values.Add(variant.Key, variant.Value);

            CurrentDocument = new DocumentOperation(p.Id.ToString(), values);
        }

        class Product
        {
            public Guid Id { get; set; }

            public string CustomId { get; set; }

            public string FriendlyUrl { get; set; }

            public string Title { get; set; }

            public string Description { get; set; }

            public string Specifications { get; set; }

            public decimal Price { get; set; }

            public string CoverImageUrl { get; set; }

            public int CurrentStock { get; set; }

            public int CategoryId { get; set; }

            public string CategoryName { get; set; }

            public string SellerId { get; set; }

            public string SellerEmail { get; set; }

            public string SellerName { get; set; }

            public string SellerPublicName { get; set; }

            public Dictionary<string, string[]> Variants { get; set; }
        }

        class Variant
        {
            public Guid ProductId { get; set; }

            public string VariantType { get; set; }

            public string VariantName { get; set; }
        }
    }
}
