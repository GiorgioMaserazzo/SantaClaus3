﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SantaClaus3;

namespace SantaClaus3.Models
{
    public class Orders
    {
        public List<Order> EntityList { get; set; }
        public List<ToyKid> ToyList { get; set; }
    }
}