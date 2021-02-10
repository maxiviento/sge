using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGE.Web.Models
{
    public class ClienteModel2
    {
        public virtual List<Cliente2> GetAllCustomer()
        {
            List<Cliente2> customerList = new List<Cliente2> {
                new Cliente2 { Id = 1, Name = "Patrick", Address = "Geuzenstraat 29", Place = "Amsterdam" },
                new Cliente2 { Id = 2, Name = "Fred", Address = "Flink 9a", Place = "Rotterdam" },
                new Cliente2 { Id = 3, Name = "Sjonnie", Address = "Paternatenplaats 44", Place = "Enkhuizen" },
                new Cliente2 { Id = 4, Name = "Henk", Address = "Wakerdijk 74", Place = "Utrecht" },
                new Cliente2 { Id = 5, Name = "Klaas", Address = "Paternatenplaats 44", Place = "Plaantan" },
                    new Cliente2 { Id = 6, Name = "Andre", Address = "Wolbrantskerkweg 90B ", Place = "Los Angeles" },
                    new Cliente2 { Id = 7, Name = "Pieter", Address = "Sam van Houtenstraat 191H", Place = "Emmen" },
                    new Cliente2 { Id = 8, Name = "Sjohn", Address = "Polostraat, M. 103-II", Place = "Kantens" },
                    new Cliente2 { Id = 9, Name = "John", Address = "Paternatenplaats 44", Place = "Leiden" },
                    new Cliente2 { Id = 10, Name = "Bruin", Address = "Anthony Spatzierhof 9", Place = "Maasbracht" },
                    new Cliente2 { Id = 11, Name = "Sjonnie", Address = "Van Heuven Goedhartlaan 201", Place = "Potters" },
                    new Cliente2 { Id = 12, Name = "Lumunon", Address = "Paternatenplaats 44", Place = "Utrecht" },
                    new Cliente2 { Id = 13, Name = "Friese", Address = "Burgemeester Roellstr 243-4", Place = "Rotterdam" },
                    new Cliente2 { Id = 14, Name = "Hudephol", Address = "Bilhamerstraat 4", Place = "Vlaardingen" },
                    new Cliente2 { Id = 15, Name = "Postema", Address = "Mastbos 77", Place = "Schiedam" },
                    new Cliente2 { Id = 16, Name = "Scharn", Address = "Marius Bauerstraat 123", Place = "Dordrecht" },
                    new Cliente2 { Id = 17, Name = "Wagenmakers", Address = "Pieter Postsingel 16", Place = "Ede" },
                    new Cliente2 { Id = 18, Name = "Gedikli", Address = "Burgemeester Hogguerstr 283", Place = "Twello" },
                    new Cliente2 { Id = 19, Name = "Zwollo", Address = "Meervalweg 140", Place = "Terschelling" },
                    new Cliente2 { Id = 20, Name = "Sjonnie", Address = "Ruys de Beerenbrouckstr 79A", Place = "Ter Aar" },
                    new Cliente2 { Id = 21, Name = "Schimmelmann", Address = "Ritzema Bosstraat 28-2", Place = "Vierenman" },
                    new Cliente2 { Id = 22, Name = "Makhlouf", Address = "Ln vd Helende Meesters 12", Place = "Eindhoven" },
                    new Cliente2 { Id = 23, Name = "Meyer", Address = "Burgemeester v Leeuwenln 79H", Place = "Breda" },
            };

            return customerList;

        }
    }
}