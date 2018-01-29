using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaClaus3
{
    public interface IDataBase
    {
        User GetUser(User user);

        IEnumerable<Toy> GetAllToys();

        Toy GetToy(string name);

        IEnumerable<Order> GetAllRequestKid();

        Order GetRequest(string id);

        bool UpdateStatus(Order requestKid);

        bool UpdateAmountToy(Toy toy);

        bool RemoveToy(string id);

        List<Decimal> SumRequest(string name);


    }
}