﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using StructureMap.Pipeline;

namespace StructureMapIoC
{
    class Program
    {
        static void Main(string[] args)
        {
            // default lifecycle is Transient, here we are overriding as Singleton
            var container = new Container(new MyRegistry());

            var shopper = container.GetInstance<Shopper>();
            shopper.Charge();
            Console.WriteLine(shopper.ChargesForCurrentCard);

            var shopper2 = container.GetInstance<Shopper>();
            shopper2.Charge();
            Console.WriteLine(shopper2.ChargesForCurrentCard);

            Console.Read();
        }

        public class MyRegistry : Registry
        {
            public MyRegistry()
            {
                For<ICreditCard>().LifecycleIs(new SingletonLifecycle()).Use<MasterCard>().Named("mastercard");
            }
        }

        public class Visa : ICreditCard
        {
            public string Charge()
            {
                return "Visa... Visa";
            }

            public int ChargeCount
            {
                get { return 0; }
            }
        }

        public class MasterCard : ICreditCard
        {
            public string Charge()
            {
                ChargeCount++;
                return "Charging with the MasterCard!";
            }

            public int ChargeCount { get; set; }
        }

        public interface ICreditCard
        {
            string Charge();
            int ChargeCount { get; }
        }

        public class Shopper
        {
            private readonly ICreditCard creditCard;

            public Shopper(ICreditCard creditCard)
            {
                this.creditCard = creditCard;
            }

            public int ChargesForCurrentCard
            {
                get { return creditCard.ChargeCount; }
            }

            public void Charge()
            {
                Console.WriteLine(creditCard.Charge());
            }
        }
    }
}
