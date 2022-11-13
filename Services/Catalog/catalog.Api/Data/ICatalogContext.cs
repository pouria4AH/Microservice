using catalog.Api.Entities;
using MongoDB.Driver;

namespace catalog.Api.Data
{
    public interface ICatalogContext
    {
        public IMongoCollection<Product> products { get; } 
    }
}
