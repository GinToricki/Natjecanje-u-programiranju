/*
 * ---------------------------------------------
 * 
 * Autor: Tin Fabijan Gorički
 * Projekt: Natjecanje U programiranju
 * Predmet: Osnove Programiranja
 * Ustanova: VŠMTI
 * Godina: 2023.
 * 
 * ---------------------------------------------
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Natjecanje_u_programiranju
{
    public struct Korisnik
    {
        public Guid id;
        public string imeKorisnika;
        public string lozinkaKorisnika;
        public string razinaPravaKorisnika;

        public Korisnik(Guid noviId, string imeKorisnikaKon, string lozinkaKorisnikaKon,string razinaPravaKorisnikaKon)
        {
            id = noviId;
            imeKorisnika = imeKorisnikaKon;
            lozinkaKorisnika = lozinkaKorisnikaKon;
            razinaPravaKorisnika = razinaPravaKorisnikaKon;
        } 
    }
    public struct KontantInformacije
    {
        public string brojMobitela;
        public string email;

        public KontantInformacije(string bMobitela, string emailKon)
        {
            brojMobitela = bMobitela;
            email = emailKon;
        }
    }
    
    public struct VoditeljNatjecanja
    {
        public Guid id;
        public string imeVoditelja;
        public string titulaVoditelja;

        public VoditeljNatjecanja(Guid noviId, string imeVoditeljaKon, string titulaVoditeljaKon)
        {
            id = noviId;
            imeVoditelja = imeVoditeljaKon;
            titulaVoditelja = titulaVoditeljaKon;
        }
    }
    public struct Organizator
    {
        public Guid id;
        public string imeOrganizatora;
        public string titulaOrganizatora;
        public KontantInformacije kInformacije;

        public Organizator(Guid noviId, string imeOrg, string titulaOrg, KontantInformacije kInformacijeKon)
        {
            id = noviId;
            imeOrganizatora = imeOrg;
            titulaOrganizatora = titulaOrg;
            kInformacije = kInformacijeKon;
        }
    }
    public struct ProgramskiJezik
    {
        public Guid id;
        public string imeProgramskogJezika;
        public Organizator organizatorJezika;

        public ProgramskiJezik(Guid noviId, string imeProgramskogJezikaKon, Organizator organizatorJezikaKon)
        {
            id = noviId;
            imeProgramskogJezika = imeProgramskogJezikaKon;
            organizatorJezika = organizatorJezikaKon;
        }
    }
    public struct Natjecatelj
    {
        public Guid id;
        public string imeNatjecatelja;

        public Natjecatelj(Guid noviId, string imeNatjecateljaKon)
        {
            id = noviId;
            imeNatjecatelja = imeNatjecateljaKon;
        }
    }
    public struct Tim
    {
        public Guid id;
        public string imeTima;
        public Natjecatelj kapetanTima;
        public List<Natjecatelj> lClanoviTima;
        public List<ProgramskiJezik> programskiJezikTima;
        public KontantInformacije kontaktTima;
        public string institucija;

        public Tim(Guid noviId, string imeTimaKon, Natjecatelj kapetanTimaKon, List<Natjecatelj> lClanoviTimaKon, List<ProgramskiJezik> programskiJezikTimaKon, KontantInformacije kontaktTimaKon, string institucijaKon)
        {
            id = noviId;
            imeTima = imeTimaKon;
            kapetanTima = kapetanTimaKon;
            lClanoviTima = lClanoviTimaKon;
            programskiJezikTima = programskiJezikTimaKon;
            kontaktTima = kontaktTimaKon;
            institucija = institucijaKon;
        }
    }
    class Program
    {
        static Korisnik Login()
        {
            Console.WriteLine("Unesite korisnicko ime");
            string korisnickoIme = Console.ReadLine();
            Console.WriteLine("Unesite lozinku");
            string lozinka = Console.ReadLine();

            List <Korisnik> lKorisnici= new List<Korisnik>();

            using (StreamReader sr = new StreamReader(@"C:\Users\exibo\source\repos\Natjecanje u programiranju\korisnici.json"))
            {
                string json = sr.ReadToEnd();

                lKorisnici = JsonConvert.DeserializeObject<List<Korisnik>>(json);
            }

            while (true)
            {
                foreach (Korisnik korisnik in lKorisnici)
                {
                    if (korisnik.imeKorisnika == korisnickoIme && korisnik.lozinkaKorisnika == lozinka)
                    {
                        return korisnik;
                    }
                }
                Console.WriteLine("Krivo uneseno ime ili lozinka. Pokušajte Ponovo.");
                Console.WriteLine("Unesite korisnicko ime");
                korisnickoIme = Console.ReadLine();
                Console.WriteLine("Unesite lozinku");
                lozinka = Console.ReadLine();
            }
        }
        static void Izbornik()
        {
            Korisnik trenutacniKorisnik = Login();

            
        }
        static void Main(string[] args)
        {
            Korisnik natjecatelj = new Korisnik(Guid.NewGuid(), "Tin", "1234", "natjecatelj");
            Korisnik organizator = new Korisnik(Guid.NewGuid(), "Luka", "4321", "admin");

            List<Korisnik> lKorisnici = new List<Korisnik> { natjecatelj, organizator };

            string noviJson = JsonConvert.SerializeObject(lKorisnici);

            using (StreamWriter sw = new StreamWriter(@"C:\Users\exibo\source\repos\Natjecanje u programiranju\korisnici.json"))
            {
                sw.Write(noviJson);
            }

            Izbornik();
            Console.ReadKey();
        }
    }
}
