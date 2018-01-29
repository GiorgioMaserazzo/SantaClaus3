using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SantaClaus3
{
    public class User
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("password_clear_text")]
        public string PasswordClearText { get; set; }

        [BsonElement("screenname")]
        public string ScreenName { get; set; }


        [BsonElement("isAdmin")]
        public Boolean isAdmin { get; set; }





    }
}