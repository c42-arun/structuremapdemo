using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;

namespace StructureMapIoC
{
    class Program
    {
        static void Main(string[] args)
        {
            //var container = new Container();
            var container = new Container(x => x.For<ICreditCard>().Use<MasterCard>().Named("mastercard"));

            //container.Configure(x => x.For<ICreditCard>().Use<MasterCard>());
            container.Configure(x => x.For<ICreditCard>().Use<Visa>().Named("visa"));

            //// Quirk: will resolve MasterCard even though we are registering Visa by name!
            //// StructureMap always returns the type registered last!
            //var shopper = container.GetInstance<Shopper>();

            // getting a named instance
            var creditCard = container.GetInstance<ICreditCard>("mastercard");
            Console.WriteLine(creditCard.Charge());

            //shopper.Charge();
            Console.Read();
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
