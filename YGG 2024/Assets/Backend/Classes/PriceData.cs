using System;

namespace ESDatabase.Classes
{
    [Serializable]
    public class PriceData
    {
        public decimal price {get;set;}
        public DateTime date {get;set;}
    }
}
