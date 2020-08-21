using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CalculaPontoCentralEDistaciaDeCoodenada
{
    class Program
    {
        public class Coordenada
        {
            public Coordenada(double latitude, double longitude)
            {
                Latitude = latitude;
                Longitude = longitude;
            }

            public Coordenada() { }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public double Distancia { get; set; }
        }

        static void Main(string[] args)
        {
            List<Coordenada> list = new List<Coordenada>();
            string coordenadas = "-43.7171499896,-22.6363074887,10 -43.717119216,-22.6363263215,10 -43.7170421579,-22.6362149745,10 -43.7170759299,-22.6361973881,10 -43.7171499896,-22.6363074887,10";
            foreach (string ponto in coordenadas.Split(" ") ?? Enumerable.Empty<string>())
            {
                string[] coordenada = ponto.Split(",");
                list.Add(new Coordenada()
                {
                    Longitude = Convert.ToDouble(coordenada[0], CultureInfo.InvariantCulture),
                    Latitude = Convert.ToDouble(coordenada[1], CultureInfo.InvariantCulture),
                });
            }

            Coordenada centro = RecuperaPontoCentral(list);

            double calc = RecuperaDistancia(list[0], list[1]);

            Console.WriteLine($"AS coordenadas do ponto central são [Longitude{centro.Longitude}] [Latitude{centro.Latitude}] e a distância entre o primeiro e seguindo ponto é de {calc} metros");
            Console.ReadKey();
        }

        static Coordenada RecuperaPontoCentral(IList<Coordenada> coordenadas)
        {
            if (coordenadas.Count == 1)
            {
                return coordenadas.Single();
            }

            double x = 0;
            double y = 0;
            double z = 0;

            foreach (var geoCoordinate in coordenadas)
            {
                var latitude = geoCoordinate.Latitude * Math.PI / 180;
                var longitude = geoCoordinate.Longitude * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
            }

            var total = coordenadas.Count;

            x = x / total;
            y = y / total;
            z = z / total;

            var centralLongitude = Math.Atan2(y, x);
            var centralSquareRoot = Math.Sqrt(x * x + y * y);
            var centralLatitude = Math.Atan2(z, centralSquareRoot);

            return new Coordenada(centralLatitude * 180 / Math.PI, centralLongitude * 180 / Math.PI);
        }

        static double RecuperaDistancia(Coordenada coordenada1, Coordenada coordenada2)
        {
            var d1 = coordenada1.Latitude * (Math.PI / 180.0);
            var num1 = coordenada1.Longitude * (Math.PI / 180.0);
            var d2 = coordenada2.Latitude * (Math.PI / 180.0);
            var num2 = coordenada2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}
