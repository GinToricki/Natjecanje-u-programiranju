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
    public struct KontanktInformacije
    {
        public string brojMobitela;
        public string email;

        public KontanktInformacije(string bMobitela, string emailKon)
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
        public KontanktInformacije kInformacije;

        public Organizator(Guid noviId, string imeOrg, string titulaOrg, KontanktInformacije kInformacijeKon)
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
        public KontanktInformacije kontaktTima;
        public string institucija;

        public Tim(Guid noviId, string imeTimaKon, Natjecatelj kapetanTimaKon, List<Natjecatelj> lClanoviTimaKon, List<ProgramskiJezik> programskiJezikTimaKon, KontanktInformacije kontaktTimaKon, string institucijaKon)
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
        static string dohvatiDatoteku(string imeDatoteke)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, imeDatoteke);
            string json = "";
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    json = sr.ReadToEnd();

                }
            }
            catch
            {
                Console.WriteLine("Error pri pisanje u datoteku {0}", imeDatoteke);
            }
            return json;
        }
        static void ZapisiDatoteku(string imeDatoteke, string noviJson)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, imeDatoteke);
            try
            {

                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(noviJson);
                }
            }
            catch
            {
                Console.WriteLine("Error pri pisanje u datoteku {0}", imeDatoteke);
            }
        }
        static Korisnik Login()
        {
            Console.WriteLine("Unesite korisnicko ime");
            string korisnickoIme = Console.ReadLine();
            Console.WriteLine("Unesite lozinku");
            string lozinka = Console.ReadLine();

            List <Korisnik> lKorisnici= lKorisnici = JsonConvert.DeserializeObject<List<Korisnik>>(dohvatiDatoteku("korisnici.json"));

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
        
        static void kreirajTimove()
        {
            string nazivDatoteke = "timovi.json";

            List<Natjecatelj> lNatjecatelja = JsonConvert.DeserializeObject<List<Natjecatelj>>(dohvatiDatoteku("natjecatelji.json"));
            List<ProgramskiJezik> lProgramskihJezika = JsonConvert.DeserializeObject<List<ProgramskiJezik>>(dohvatiDatoteku("programski_jezici.json"));

            Tim tim01 = new Tim(Guid.NewGuid(), "A-tim", lNatjecatelja[0], new List<Natjecatelj> { lNatjecatelja[0], lNatjecatelja[1], lNatjecatelja[2] }, new List<ProgramskiJezik> { lProgramskihJezika[0] }, new KontanktInformacije("123-456-7777", "a-tim@mail.com"), "VŠMTI");
            Tim tim02 = new Tim(Guid.NewGuid(), "B-tim", lNatjecatelja[3], new List<Natjecatelj> { lNatjecatelja[3], lNatjecatelja[4], lNatjecatelja[5] }, new List<ProgramskiJezik> { lProgramskihJezika[0], lProgramskihJezika[2], lProgramskihJezika[3] }, new KontanktInformacije("222-456-7777", "b-tim@mail.com"), "FER");
            Tim tim03 = new Tim(Guid.NewGuid(), "C-tim", lNatjecatelja[6], new List<Natjecatelj> { lNatjecatelja[6], lNatjecatelja[7], lNatjecatelja[8] }, new List<ProgramskiJezik> { lProgramskihJezika[4], lProgramskihJezika[2] }, new KontanktInformacije("555-456-7777", "c-tim@mail.com"), "TVZ");
            Tim tim04 = new Tim(Guid.NewGuid(), "D-tim", lNatjecatelja[9], new List<Natjecatelj> { lNatjecatelja[9], lNatjecatelja[10], lNatjecatelja[11] }, new List<ProgramskiJezik> { lProgramskihJezika[0], lProgramskihJezika[1], lProgramskihJezika[2], lProgramskihJezika[3], lProgramskihJezika[4] }, new KontanktInformacije("123-654-7777", "d-tim@mail.com"), "VŠMTI");
            Tim tim05 = new Tim(Guid.NewGuid(), "E-tim", lNatjecatelja[12], new List<Natjecatelj> { lNatjecatelja[12], lNatjecatelja[13], lNatjecatelja[14] }, new List<ProgramskiJezik> { lProgramskihJezika[0], lProgramskihJezika[4] }, new KontanktInformacije("333-456-7777", "e-tim@mail.com"), "FOI");

            List<Tim> lTimova = new List<Tim> { tim01, tim02, tim03, tim04, tim05 };
            string noviJson = JsonConvert.SerializeObject(lTimova);

            ZapisiDatoteku(nazivDatoteke, noviJson);
        }
        static void kreirajOrganizatore()
        {
            string nazivDatoteke = "organizatori.json";

            Organizator organizator01 = new Organizator(Guid.NewGuid(), "Josip", "Mag.", new KontanktInformacije("09912345678", "josip@mail.com"));
            Organizator organizator02 = new Organizator(Guid.NewGuid(), "Marko", "Mag.", new KontanktInformacije("09912345678", "marko@mail.com"));
            Organizator organizator03 = new Organizator(Guid.NewGuid(), "Petar", "Mag.", new KontanktInformacije("09912345678", "petar@mail.com"));
            Organizator organizator04 = new Organizator(Guid.NewGuid(), "Dario", "Mag.", new KontanktInformacije("09912345678", "dario@mail.com"));
            Organizator organizator05 = new Organizator(Guid.NewGuid(), "Luka", "Mag.", new KontanktInformacije("09912345678", "luka@mail.com"));

            List<Organizator> lOrganizatora = new List<Organizator> { organizator01, organizator02, organizator03, organizator04, organizator05 };
            string noviJson = JsonConvert.SerializeObject(lOrganizatora);
            ZapisiDatoteku(nazivDatoteke, noviJson);
        }

        static void kreirajProgramskeJezike()
        {
            string nazivDatoteke = "programski_jezici.json";

            List<Organizator> lOrganizatora = JsonConvert.DeserializeObject<List<Organizator>>(dohvatiDatoteku("organizatori.json"));

            ProgramskiJezik python = new ProgramskiJezik(Guid.NewGuid(), "python", lOrganizatora[0]);
            ProgramskiJezik csharp = new ProgramskiJezik(Guid.NewGuid(), "csharp", lOrganizatora[1]);
            ProgramskiJezik c = new ProgramskiJezik(Guid.NewGuid(), "c", lOrganizatora[2]);
            ProgramskiJezik cplusplus = new ProgramskiJezik(Guid.NewGuid(), "cplusplus", lOrganizatora[3]);
            ProgramskiJezik javascript = new ProgramskiJezik(Guid.NewGuid(), "python", lOrganizatora[4]);

            List<ProgramskiJezik> lProgramskihJezika = new List<ProgramskiJezik> { python, csharp, c, cplusplus, javascript };

            string noviJson = JsonConvert.SerializeObject(lProgramskihJezika);

            ZapisiDatoteku(nazivDatoteke, noviJson);
        }
        static void kreirajNatjecatelje()
        {
            string nazivDatoteke = "natjecatelji.json";

            Natjecatelj natjecatelj01 = new Natjecatelj(Guid.NewGuid(), "Marko");
            Natjecatelj natjecatelj02 = new Natjecatelj(Guid.NewGuid(), "Dominik");
            Natjecatelj natjecatelj03 = new Natjecatelj(Guid.NewGuid(), "Petar");
            Natjecatelj natjecatelj04 = new Natjecatelj(Guid.NewGuid(), "Ana");
            Natjecatelj natjecatelj05 = new Natjecatelj(Guid.NewGuid(), "Mario");
            Natjecatelj natjecatelj06 = new Natjecatelj(Guid.NewGuid(), "Luka");
            Natjecatelj natjecatelj07 = new Natjecatelj(Guid.NewGuid(), "Barbara");
            Natjecatelj natjecatelj08 = new Natjecatelj(Guid.NewGuid(), "Antonio");
            Natjecatelj natjecatelj09 = new Natjecatelj(Guid.NewGuid(), "Domagoj");
            Natjecatelj natjecatelj10 = new Natjecatelj(Guid.NewGuid(), "Antonela");
            Natjecatelj natjecatelj11 = new Natjecatelj(Guid.NewGuid(), "Rudi");
            Natjecatelj natjecatelj12 = new Natjecatelj(Guid.NewGuid(), "Tomislav");
            Natjecatelj natjecatelj13 = new Natjecatelj(Guid.NewGuid(), "Karlo");
            Natjecatelj natjecatelj14 = new Natjecatelj(Guid.NewGuid(), "Fran");
            Natjecatelj natjecatelj15 = new Natjecatelj(Guid.NewGuid(), "Iva");

            List<Natjecatelj> lNatljecatelja = new List<Natjecatelj> { natjecatelj01, natjecatelj02, natjecatelj03, natjecatelj04, natjecatelj05, natjecatelj06, natjecatelj07, natjecatelj08, natjecatelj09, natjecatelj10, natjecatelj11, natjecatelj12, natjecatelj13, natjecatelj14, natjecatelj15 };
            string noviJson = JsonConvert.SerializeObject(lNatljecatelja);
            ZapisiDatoteku(nazivDatoteke, noviJson);
        }
        static void Main(string[] args)
        {
            Korisnik natjecatelj = new Korisnik(Guid.NewGuid(), "Antun", "1234", "natjecatelj");
            Korisnik organizator = new Korisnik(Guid.NewGuid(), "Luka", "4321", "admin");

            List<Korisnik> lKorisnici = new List<Korisnik> { natjecatelj, organizator };

            string noviJson = JsonConvert.SerializeObject(lKorisnici);

            string nazivDatoteke = "korisnici.json";
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, nazivDatoteke);

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(noviJson);
            }
            //kreirajProgramskeJezike();
            //kreirajOrganizatore();
            //kreirajNatjecatelje();
            //kreirajTimove();
            Izbornik();
            Console.ReadKey();
        }
    }
}
