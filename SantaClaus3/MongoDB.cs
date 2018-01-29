using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace SantaClaus3
{
    public class MongoDB : IDataBase
    {
        private IMongoDatabase database
        {
            get
            {
                return MongoConnection.Instance.Database;
            }
        }

        public User GetUser(User user)
        {
            IMongoCollection<User> userCollection = database.GetCollection<User>("users");
            return userCollection.Find(_ => _.Email == user.Email && _.Password == user.Password).FirstOrDefault();
        }

        public Toy GetToy(string name)
        {
            IMongoCollection<Toy> toyCollection = database.GetCollection<Toy>("toys");
            return toyCollection.Find(_ => _.Name == name).FirstOrDefault();
        }

        public IEnumerable<Toy> GetAllToys()
        {
            IMongoCollection<Toy> toyCollection = database.GetCollection<Toy>("toys");
            return toyCollection.Find(new BsonDocument()).ToList();
        }

        public IEnumerable<Order> GetAllRequestKid()
        {
            IMongoCollection<Order> requestCollection = database.GetCollection<Order>("orders");
            return requestCollection.Find(new BsonDocument()).SortBy(t => t.Date).ToList();
        }

        public Order GetRequest(string id)
        {
            IMongoCollection<Order> requestCollection = database.GetCollection<Order>("orders");
            return requestCollection.Find(_ => _.ID == id).FirstOrDefault();
        }

        public bool UpdateStatus(Order requestKid)
        {
            IMongoCollection<Order> requestCollection = database.GetCollection<Order>("orders");
            var filter = Builders<Order>.Filter.Eq("_id", ObjectId.Parse(requestKid.ID));
            var update = Builders<Order>.Update
                .Set("status", requestKid.Status);
            try
            {
                requestCollection.UpdateOne(filter, update);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateAmountToy(Toy toy)
        {
            IMongoCollection<Toy> toyCollection = database.GetCollection<Toy>("toys");
            var filter = Builders<Toy>.Filter.Eq("_id", ObjectId.Parse(toy.ID));
            var update = Builders<Toy>.Update
                .Inc("amount", -1);

            try
            {
                toyCollection.UpdateOne(filter, update);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveToy(string id)
        {
            IMongoCollection<Toy> toyCollection = database.GetCollection<Toy>("toys");
            var filter = Builders<Toy>.Filter.Eq("_id", ObjectId.Parse(id));


            try
            {
                toyCollection.DeleteOne(filter);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Decimal> SumRequest(string name)
        {
            IMongoCollection<Toy> toyCollection = database.GetCollection<Toy>("toys");
            var match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                            {
                                {"Name", name}
                            }
                    }
                };


            var group = new BsonDocument
                {
                    { "$group",
                        new BsonDocument
                            {
                                { "_id", new BsonDocument
                                             {
                                                 {
                                                     "MyUser","$User"
                                                 }
                                             }
                                },
                                {
                                    "Count", new BsonDocument
                                                 {
                                                     {
                                                         "$sum", "$Count"
                                                     }
                                                 }
                                }
                            }
                  }
                };


            var pipeline = new[] { match, group };

            var result = toyCollection.Aggregate<Decimal>(pipeline);
            return result.ToList();



        }

    }
}
