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
using ConsoleTables;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace Natjecanje_u_programiranju
{
    public struct Obavijest
    {
        public Guid idKorisnika;
        public Guid idOrganizatora;
        public string obavijest;
        public DateTime datum;

        public Obavijest(Guid iKorisnika, Guid iOrganizatora, string obavijestKon, DateTime datumKon)
        {
            idKorisnika = iKorisnika;
            idOrganizatora = iOrganizatora;
            obavijest = obavijestKon;
            datum = datumKon;
        }
    }
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
    public struct Organizator
    {
        public Guid id;
        public string imeOrganizatora;
        public string prezimeOrganizatora;
        public string OIBOrganizatora;
        public string titulaOrganizatora;
        public bool voditelj;
        public KontanktInformacije kInformacije;

        public Organizator(Guid noviId, string imeOrg,string prezimeOrganizatoraKon,string OIBOrganizatoraKon, string titulaOrg, bool voditeljKon, KontanktInformacije kInformacijeKon)
        {
            id = noviId;
            imeOrganizatora = imeOrg;
            prezimeOrganizatora = prezimeOrganizatoraKon;
            OIBOrganizatora = OIBOrganizatoraKon;
            titulaOrganizatora = titulaOrg;
            kInformacije = kInformacijeKon;
            voditelj = voditeljKon;
        }
    }
    public struct ProgramskiJezik
    {
        public Guid id;
        public string imeProgramskogJezika;
        public List<Guid> lIdOrganizatorJezika;

        public ProgramskiJezik(Guid noviId, string imeProgramskogJezikaKon, List<Guid> lIdOrganizatorJezikaKon)
        {
            id = noviId;
            imeProgramskogJezika = imeProgramskogJezikaKon;
            lIdOrganizatorJezika = lIdOrganizatorJezikaKon;
        }
    }
    public struct Natjecatelj
    {
        public Guid id;
        public string imeNatjecatelja;
        public string prezimeNatjecatelja;
        public string OIBNatjecatelja;

        public Natjecatelj(Guid noviId, string imeNatjecateljaKon, string prezimeNatjecateljaKon, string Oib)
        {
            id = noviId;
            imeNatjecatelja = imeNatjecateljaKon;
            prezimeNatjecatelja = prezimeNatjecateljaKon;
            OIBNatjecatelja = Oib;
        }
    }
    public struct Tim
    {
        public Guid id;
        public string imeTima;
        public Natjecatelj kapetanTima;
        public Guid idKapetanaTima;
        public List<Natjecatelj> lClanoviTima;
        public List<Guid> lIdClanovaTima;
        public List<Guid> lIdProgramskihJezikaTima;
        public KontanktInformacije kontaktTima;
        public string institucija;

        public Tim(Guid noviId, string imeTimaKon, Natjecatelj kapetanTimaKon,Guid idKapetanaTimaKon, List<Natjecatelj> lClanoviTimaKon,List<Guid> lIdClanovaTimaKon,List<Guid> lIdProgramskihJezikaTimaKon, KontanktInformacije kontaktTimaKon, string institucijaKon)
        {
            id = noviId;
            imeTima = imeTimaKon;
            kapetanTima = kapetanTimaKon;
            idKapetanaTima = idKapetanaTimaKon;
            lClanoviTima = lClanoviTimaKon;
            lIdClanovaTima = lIdClanovaTimaKon;
            lIdProgramskihJezikaTima = lIdProgramskihJezikaTimaKon;
            kontaktTima = kontaktTimaKon;
            institucija = institucijaKon;
        }
    }

    public struct Rezultati
    {
        public int redniBrojKola;
        public Guid idProgramskogJezika; //id programskog jezika;
        public Guid idTima; //id tima;
        public int[] zadaci;
        public int ukupniBrojBodova;

        public Rezultati(int redBrojKolaKon, Guid programskiJezikNatjecanjaKon, Guid timNatjecanjeRezKon, int[] zadaciKon, int ukupniBrojBodovaKon)
        {
            redniBrojKola = redBrojKolaKon;
            idProgramskogJezika = programskiJezikNatjecanjaKon;
            idTima = timNatjecanjeRezKon;
            zadaci = zadaciKon;
            ukupniBrojBodova = ukupniBrojBodovaKon;
        }
    }
    class Program
    {
        public static Korisnik TRENUTACNI_KORISNIK;
        static void ZapisiLog(string akcija)
        {
            /*
             * Funkcija koja zapisuje string(akciju) u log.txt
             */
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "logovi.txt");
            string tekst = $"{DateTime.Now} Korisnik {TRENUTACNI_KORISNIK.imeKorisnika} (id: {TRENUTACNI_KORISNIK.id}) " + akcija;
            using(StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(tekst);
            }
        }
        static string dohvatiDatoteku(string imeDatoteke)
        {
            /*
             * Funkcija koja dohvaća putanju datoteke.
             */
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
            /*
             * Funkcija Koja zapisuje u odredenu datoteku.
             */
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
        static void pregledObavijesti(List<Natjecatelj> ulaznaListaNatjecatelja, List<Organizator> ulaznaListaOrganizatora, List<Obavijest> ulaznaListaObavijesti)
        {
            /*
             * Funkcija koja omugucava administratoru pregled obavijesti.
             */
            foreach(Obavijest ob in ulaznaListaObavijesti)
            {
                if(ob.idOrganizatora == TRENUTACNI_KORISNIK.id)
                {
                    foreach (Natjecatelj natjecatelj in ulaznaListaNatjecatelja)
                    {
                        if (ob.idKorisnika == natjecatelj.id)
                        {
                            Console.WriteLine($"Za: {TRENUTACNI_KORISNIK.imeKorisnika}\nOd: {natjecatelj.imeNatjecatelja} {natjecatelj.prezimeNatjecatelja}\n{ob.obavijest}\nPoslano:{ob.datum}");
                        }
                    }
                }
            }
            
        }
        static void DodajKorisnika(string imeKorisnika, Guid idKorisnika, string razinaPrava)
        {
            /*
             * Funkcija s kojom dodajemo novog korisnika
             */
            List<Korisnik> lKorisnika = JsonConvert.DeserializeObject<List<Korisnik>>(dohvatiDatoteku("korisnici.json"));
            Console.WriteLine("Unesite Lozinku za novog korisnika: ");
            string lozinka = Console.ReadLine();
            if(razinaPrava == "admin")
            {
                Korisnik noviKorisnik = new Korisnik(idKorisnika, imeKorisnika, "4321", razinaPrava);
                lKorisnika.Add(noviKorisnik);
            }
            else
            {
                Korisnik noviKorisnik = new Korisnik(idKorisnika, imeKorisnika, "1234", razinaPrava);
                lKorisnika.Add(noviKorisnik);
            }
            string noviJson = JsonConvert.SerializeObject(lKorisnika);
            ZapisiDatoteku("korisnici.json", noviJson);
        }
        static Korisnik Login()
        {
            /*
             * Login funkcija u kojoj korisnik unosi korisnicko ime i lozinku
             */
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
                Console.WriteLine("Krivo uneseno ime ili lozinka. Pokušajte Ponovo. Unesite 0 za izlaz iz programa.");
                Console.WriteLine("Unesite korisnicko ime");
                korisnickoIme = Console.ReadLine();
                if(korisnickoIme == "0")
                {
                    break;
                }
                Console.WriteLine("Unesite lozinku");
                lozinka = Console.ReadLine();
            }
            return new Korisnik();
        }
        static string PronadiClana(Guid idTrazenogClana, List<Natjecatelj> ulaznaListaNatjecatelja)
        {
            /*
             * Funkcija pomocu koje trazimo odredenog clana
             */
            string imePrezimeClana = "";
            foreach(Natjecatelj natjecatelj in ulaznaListaNatjecatelja)
            {
                if(idTrazenogClana == natjecatelj.id)
                {
                    imePrezimeClana = $"{natjecatelj.imeNatjecatelja} {natjecatelj.prezimeNatjecatelja}";
                }
            }
            return imePrezimeClana;
        }
        static string PronadiProgramskeJezike(Guid idTrazenogJezika, List<ProgramskiJezik> ulaznaListaProgramskihJezika)
        {
            /*
             * Funkcija pomocu koje trazimo odredeni programski jezik
             */
            string nazivProgramskogJezika = "";
            foreach(ProgramskiJezik progJezik in ulaznaListaProgramskihJezika)
            {
                if(idTrazenogJezika == progJezik.id)
                {
                    nazivProgramskogJezika = progJezik.imeProgramskogJezika;
                }
            }
            return nazivProgramskogJezika;
        }
        static void PrikaziTimove(List<Tim> ulaznaListaTimova, List<Natjecatelj> ulaznaListaNatjecatelja, List<ProgramskiJezik> ulaznaListaProgJezika)
        {
            /*
             * Funkcija pomocu koje prikazujemo timove
             */
            int rBr = 1;
            var table = new ConsoleTable("R.br.", "ID", "Naziv Tima", "Clanovi Tima", "Kapetan tima", "Kontakt", "Programski jezici", "Institucija");
            foreach(Tim tim in ulaznaListaTimova)
            {
                string clanoviTima = "";
                for(int k = 0; k < tim.lIdClanovaTima.Count; k++)
                {
                    if(k == tim.lIdClanovaTima.Count -1)
                    {
                        clanoviTima += PronadiClana(tim.lIdClanovaTima[k], ulaznaListaNatjecatelja);
                    }else
                    {
                        clanoviTima += $"{PronadiClana(tim.lIdClanovaTima[k], ulaznaListaNatjecatelja)}, ";
                    }
                }
                string kontantTima = "Mobitel: " + tim.kontaktTima.brojMobitela + ",Email: " +tim.kontaktTima.email;
                string programskiJeziciTima = "";
                for(int i = 0; i < tim.lIdProgramskihJezikaTima.Count; i++)
                {
                    if (i == tim.lIdProgramskihJezikaTima.Count - 1)
                    {
                        programskiJeziciTima += PronadiProgramskeJezike(tim.lIdProgramskihJezikaTima[i], ulaznaListaProgJezika);
                    }
                    else
                    {
                        programskiJeziciTima += $"{PronadiProgramskeJezike(tim.lIdProgramskihJezikaTima[i], ulaznaListaProgJezika)}, ";
                    }
                }
                table.AddRow(rBr++, tim.id, tim.imeTima, clanoviTima,PronadiClana(tim.idKapetanaTima, ulaznaListaNatjecatelja), kontantTima, programskiJeziciTima, tim.institucija );
            }
            table.Write();
        }
        static void ispisiOrganizatore(List<Organizator> ulaznaListaOrganizatora, List<ProgramskiJezik> ulaznaListaProgramskihJezika)
        {
            /*
             * Funkcija pomocu koje ispisujemo organizatore.
             */
            int redniBroj = 1;
            var table = new ConsoleTable("R.Br.", "Ime i prezime", "Titula", "Kontakt informacije", "OIB", "Tip" );
            foreach (Organizator org in ulaznaListaOrganizatora)
            {
                if (org.voditelj)
                {
                    string imePrezime = $"{org.imeOrganizatora} {org.prezimeOrganizatora}";
                    string kInformacije = $"Broj mobitela: {org.imeOrganizatora}, email: {org.kInformacije.email}";
                    table.AddRow(redniBroj++, imePrezime, org.titulaOrganizatora, kInformacije, org.OIBOrganizatora, "Voditelj");
                }
            }

            foreach (ProgramskiJezik pJezik in ulaznaListaProgramskihJezika)
            {
                foreach (Guid id in pJezik.lIdOrganizatorJezika)
                {
                    foreach (Organizator org in ulaznaListaOrganizatora)
                    {
                        if (org.id == id && !org.voditelj)
                        {
                            string imePrezime = $"{org.imeOrganizatora} {org.prezimeOrganizatora}";
                            string kInformacije = $"Broj mobitela: {org.imeOrganizatora}, email: {org.kInformacije.email}";
                            table.AddRow(redniBroj++, imePrezime, org.titulaOrganizatora, kInformacije, org.OIBOrganizatora, "Organizator");
                        }
                    }
                }
            }
            table.Write();
        }
        static void napisiObavijest(Guid organizatora, Guid korisnika)
        {
            /*
             * Funkcija pomocu koje pisemo obavijesti
             */
            Console.WriteLine("Unesite poruku koju želite poslati Organizatoru");
            string poruka = Console.ReadLine();
            Obavijest obavijest = new Obavijest(korisnika, organizatora, poruka, DateTime.Now);
            List < Obavijest > lObavijesti = JsonConvert.DeserializeObject<List<Obavijest>>(dohvatiDatoteku("obavijesti.json"));
            lObavijesti.Add(obavijest);
            string Json = JsonConvert.SerializeObject(lObavijesti);
            ZapisiDatoteku("obavijesti.json", Json);
        }

        static void prikaziOrganizatore(List<Organizator> ulaznaListaOrganizatora, List<ProgramskiJezik> ulaznaListaPJezika, bool korisnik, Guid idKorisnika = new Guid())
        {
            /*
             * Funckija pomocu koje prikazujemo organizatore
             */
            if (korisnik)
            {
                ispisiOrganizatore(ulaznaListaOrganizatora, ulaznaListaPJezika);
                bool odabir = true;
                while (odabir)
                {
                    Console.WriteLine("1. Ako zelite poslati obavijest organizatoru\n2. Za povratak");
                    string odabirAkcije = Console.ReadLine();
                    switch (odabirAkcije)
                    {
                        case "1":
                            Console.WriteLine("Unesite redni broj organizatora kojem želite poslati obavijest");
                            int rBr = Convert.ToInt32(Console.ReadLine()) - 1;
                            while (rBr < 0 || rBr > ulaznaListaOrganizatora.Count)
                            {
                                Console.WriteLine("Pogresan unos.\n Unesite broj opet");
                                rBr = Convert.ToInt32(Console.ReadLine());
                            }
                            napisiObavijest(ulaznaListaOrganizatora[rBr].id, idKorisnika);
                            break;
                        case "2":
                            odabir = false;
                            break;
                        default:
                            Console.WriteLine("Krivi odabir");
                            break;
                    }  
                }
            }
            else
            {
                ispisiOrganizatore(ulaznaListaOrganizatora, ulaznaListaPJezika);
            }
           
        }

        static List<Guid> prikaziProgramskeJezike(List<ProgramskiJezik> ulaznaListaProgramskihJezika)
        {
            /*
             * Funkcija pomocu koje prikazujemo programske jezike
             */
            Console.WriteLine("Odaberite Programski jezik");
            List<Guid> lProgramskihJezikaTima = new List<Guid>();
            List<ProgramskiJezik> lProgJezikaKojiNisuOdabrani = new List<ProgramskiJezik>(ulaznaListaProgramskihJezika);
            for(int i = 0; i<lProgJezikaKojiNisuOdabrani.Count;i++)
            {
                int redniBroj = i + 1;
                Console.WriteLine("{0}. Ime Programskog jezika {1}",redniBroj, ulaznaListaProgramskihJezika[i].imeProgramskogJezika);
            }
            int odabir = Convert.ToInt32(Console.ReadLine())-1;
            lProgramskihJezikaTima.Add(lProgJezikaKojiNisuOdabrani[odabir].id);
            lProgJezikaKojiNisuOdabrani.Remove(lProgJezikaKojiNisuOdabrani[odabir]);
            bool exit = true;
            while (exit)
            {
                Console.WriteLine("Želite li dodati još programskih Jezika?\n1. Da 2. Ne");
                string izbor = Console.ReadLine();
                switch (izbor)
                {
                    case "1":
                        if (lProgJezikaKojiNisuOdabrani.Count == 0)
                        {
                            exit = false;
                            Console.WriteLine("Nema vise programskih jezika za odabir");
                        }
                        else
                        {
                            int redniBroj = 1;
                            Console.WriteLine("Odaberite Programski Jezik");
                            for (int i = 0; i < lProgJezikaKojiNisuOdabrani.Count; i++)
                            {
                                Console.WriteLine($"{redniBroj++}. Ime Programskog jezika {lProgJezikaKojiNisuOdabrani[i].imeProgramskogJezika}");
                            }
                            odabir = Convert.ToInt32(Console.ReadLine()) - 1;
                            lProgramskihJezikaTima.Add(lProgJezikaKojiNisuOdabrani[odabir].id);
                            lProgJezikaKojiNisuOdabrani.Remove(lProgJezikaKojiNisuOdabrani[odabir]);
                        }
                        break;
                    case "2":
                        exit = false;
                        break;
                    default:
                        Console.WriteLine("Krivi unos");
                        break;
                }
            }

            return lProgramskihJezikaTima;
        }
        static bool JeBroj(string broj)
        {
            return Regex.Match(broj, @"^(\[0-9]{10})$").Success;
        }
        static string potvrdiBrojMobitela()
        {
            Console.WriteLine("Unesite broj mobitela");
            string bMobitela = Console.ReadLine();
            while (JeBroj(bMobitela))
            {
                Console.WriteLine("Ponovo unesite broj mobitela");
                bMobitela = Console.ReadLine();
            }
            return bMobitela;
        }
        static bool valjaEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        static string potvrdiEmail()
        {
            Console.WriteLine("Unesite email");
            string email = Console.ReadLine();
            while (!valjaEmail(email))
            {
                Console.WriteLine("Ponovno unesite email");
                email = Console.ReadLine();
            }
            return email;
        }
        static KontanktInformacije DodavanjeKontakta()
        {
            string brojMobitela = potvrdiBrojMobitela();
            string email = potvrdiEmail();
            return new KontanktInformacije(brojMobitela, email);
        }
        
        static string DodavanjeInstitucije()
        {
            Console.WriteLine("Unesite instituciju");
            return Console.ReadLine();
        }
        static string PotvrdiOib()
        {
            Console.WriteLine("Unesite Oib");
            string oib = Console.ReadLine();
            while(!Int64.TryParse(oib, out long _) || oib.Length != 10)
            {
                Console.WriteLine("Pogresan unos. Ponovno Unesite oib");
                oib = Console.ReadLine();
            }
            return oib;
        }
       
        static Natjecatelj DodavanjeNatjecatelja(List<Natjecatelj> ulaznaListaNatjecatelja)
        {
            /*
             * Funkcija pomocu koje dodajemo korisnicke podatke novog natjecatelja
             */
            Console.WriteLine("Unesite Ime natjecatelja kojeg želite dodati:");
            string imeNatjecatelja = Console.ReadLine();
            Console.WriteLine("Unesite prezime natjecatelja kojeg želite dodati");
            string prezimeNatjecatelja = Console.ReadLine();
            Natjecatelj noviNatjecatelj = new Natjecatelj(Guid.NewGuid(), imeNatjecatelja, prezimeNatjecatelja, PotvrdiOib());
            ulaznaListaNatjecatelja.Add(noviNatjecatelj);
            DodajKorisnika(imeNatjecatelja, noviNatjecatelj.id, "natjecatelj");
            string noviJson = JsonConvert.SerializeObject(ulaznaListaNatjecatelja);
            ZapisiDatoteku("natjecatelji.json", noviJson);
            return noviNatjecatelj;
        }
        static ProgramskiJezik BiranjeProgramskogJezikaOrganizatora(List<ProgramskiJezik> ulaznaListaProgramskihJezika)
        {
            /*
             * Funkcija pomocu koje biramo programski jezik nekog organizatora.
             */
            Console.WriteLine("Odaberite Programski jezik");
            for (int i = 0; i < ulaznaListaProgramskihJezika.Count; i++)
            {
                int redniBroj = i + 1;
                Console.WriteLine("{0}. Ime Programskog jezika {1}", redniBroj, ulaznaListaProgramskihJezika[i].imeProgramskogJezika);
            }
            int odabir = Convert.ToInt32(Console.ReadLine()) - 1;

            return ulaznaListaProgramskihJezika[odabir];
        }
        static void DodavanjeOrganizatora(List<Organizator> ulaznaListaOrganizatora, List<ProgramskiJezik> ulaznaListaProgramskihJezika)
        {
            /*
             * Funkcija pomocu koje dodajemo organizatora
             */
            ProgramskiJezik odabraniProgramskiJezik = BiranjeProgramskogJezikaOrganizatora(ulaznaListaProgramskihJezika);
            Console.WriteLine("Unesite Ime Organizatora");
            string imeOrganizatora = Console.ReadLine();
            Console.WriteLine("Unesite titulu organizatora");
            string titulaOrganizatora = Console.ReadLine();
            Console.WriteLine("Unesite prezime Organizatora");
            string prezimeOrganizatora = Console.ReadLine();
            Organizator noviOrganizator = new Organizator(Guid.NewGuid(), imeOrganizatora,prezimeOrganizatora, PotvrdiOib(), titulaOrganizatora,false, DodavanjeKontakta());

            DodajKorisnika(noviOrganizator.imeOrganizatora, noviOrganizator.id, "natjecatelj");
            ulaznaListaOrganizatora.Add(noviOrganizator);
            string noviJson = JsonConvert.SerializeObject(ulaznaListaOrganizatora);
            ZapisiDatoteku("organizatori.json", noviJson);
            for(int i = 0; i < ulaznaListaProgramskihJezika.Count; i++)
            {
                if (ulaznaListaProgramskihJezika[i].Equals(odabraniProgramskiJezik))
                {
                    List<Guid> novaListaOrganizatora = new List<Guid>(ulaznaListaProgramskihJezika[i].lIdOrganizatorJezika);
                    novaListaOrganizatora.Add(noviOrganizator.id);
                    ulaznaListaProgramskihJezika[i] = new ProgramskiJezik(ulaznaListaProgramskihJezika[i].id, ulaznaListaProgramskihJezika[i].imeProgramskogJezika, novaListaOrganizatora);
                }
            }
            noviJson = JsonConvert.SerializeObject(ulaznaListaProgramskihJezika);
            ZapisiDatoteku("programski_jezici.json", noviJson);
        }
        static void DodavanjeOsobe(List<Organizator> ulaznaListaOrganizatora, List<ProgramskiJezik> ulaznaListaPjezika, List<Natjecatelj> ulaznaListaNatjecatelja)
        {
            /*
             * Funkcija pomocu koje dodajemo osobe
             */
            string odabir;
            bool exit = true;
            do
            {
                Console.WriteLine("Unesite vaš odabir:\n1. Dodavanje Organizatora\n2. Dodavanje Natjecatelja\n3. Izlaz");

                odabir = Console.ReadLine();
                switch (odabir)
                {
                    case "1":
                        DodavanjeOrganizatora(ulaznaListaOrganizatora, ulaznaListaPjezika);
                        break;
                    case "2":
                        DodavanjeNatjecatelja(ulaznaListaNatjecatelja);
                        break;
                    case "3":
                        exit = false;
                        break;
                    default:
                        Console.WriteLine("Krivi odabir");
                        break;
                }
            } while (exit);
            
        }

        static void azurirajNazivTima(List<Tim> ulaznaListaTimova, int ulazniRedniBroj)
        {
            /*
             * Funkcija pomocu koje azuriramo naziv tima.
             */
            Console.WriteLine("Ime tima koje mijenjate {0}", ulaznaListaTimova[ulazniRedniBroj].imeTima);
            Console.WriteLine("Unesite novo Ime");
            string novoImeTima = Console.ReadLine();
            ulaznaListaTimova[ulazniRedniBroj] = new Tim(ulaznaListaTimova[ulazniRedniBroj].id, novoImeTima, ulaznaListaTimova[ulazniRedniBroj].kapetanTima, ulaznaListaTimova[ulazniRedniBroj].idKapetanaTima, ulaznaListaTimova[ulazniRedniBroj].lClanoviTima, ulaznaListaTimova[ulazniRedniBroj].lIdClanovaTima , ulaznaListaTimova[ulazniRedniBroj].lIdProgramskihJezikaTima, ulaznaListaTimova[ulazniRedniBroj].kontaktTima, ulaznaListaTimova[ulazniRedniBroj].institucija);

            string noviJson = JsonConvert.SerializeObject(ulaznaListaTimova);
            ZapisiDatoteku("timovi.json", noviJson);
        }
        static void prikaziClanoveTima(List<Natjecatelj> ulaznaListaNatjecatelja)
        {
            /*
             *Funkcija koja nam sluzi za prikaz svih clanova tima 
             */
            for(int i = 0; i < ulaznaListaNatjecatelja.Count; i++)
            {
                Console.WriteLine("R.br. Natjecatelja {0}\tIme Natjecatelja {1}", i+1, ulaznaListaNatjecatelja[i].imeNatjecatelja);
            }
        }
        static Natjecatelj promijeniNatjecateljaUDatoteci(Guid id, string novoIme, string novoPrezime)
        {
            /*
             * Funkcija u kojoj mijenjamo natjecatelja u datoteci
             */
            List<Natjecatelj> lNatjecatelja = JsonConvert.DeserializeObject<List<Natjecatelj>>(dohvatiDatoteku("natjecatelji.json"));
            Natjecatelj natjecateljSNovimImenom = new Natjecatelj();
            for(int i = 0; i < lNatjecatelja.Count; i++)
            {
                if(id == lNatjecatelja[i].id)
                {
                    lNatjecatelja[i] = new Natjecatelj(id, novoIme, novoPrezime, PotvrdiOib());
                    natjecateljSNovimImenom = lNatjecatelja[i];
                }
            }
            string noviJson = JsonConvert.SerializeObject(lNatjecatelja);
            ZapisiDatoteku("natjecatelji.json", noviJson);
            return natjecateljSNovimImenom;
        }
        static Natjecatelj promijeniImeClanaTima(List<Natjecatelj> ulaznaListaNatjecatelja)
        {
            /*
             * Funkcija pomocu koje mozemo promjeniti podatke o natjecatelju
             */
            prikaziClanoveTima(ulaznaListaNatjecatelja);
            Console.WriteLine("Odaberite clana kojem zelite promijeniti ime");
            int odabirClana = Convert.ToInt32(Console.ReadLine()) -1;
            Console.WriteLine("Unesite novo ime za {0}", ulaznaListaNatjecatelja[odabirClana].imeNatjecatelja);
            string novoIme = Console.ReadLine();
            Console.WriteLine("Unesite novo prezime za {0}", ulaznaListaNatjecatelja[odabirClana].imeNatjecatelja);
            string novoPrezime = Console.ReadLine();
            Natjecatelj updateanNatjecatelj = promijeniNatjecateljaUDatoteci(ulaznaListaNatjecatelja[odabirClana].id, novoIme, novoPrezime);
            ulaznaListaNatjecatelja[odabirClana] = updateanNatjecatelj;
            return updateanNatjecatelj;
        }
        static void updateTimove(List<Tim> ulaznaListaTimova)
        {
            string noviJson = JsonConvert.SerializeObject(ulaznaListaTimova);
            ZapisiDatoteku("timovi.json", noviJson);
        }
        static void dodajClanaTima(List<Tim> ulaznaListaTimova, int ulazniRedniBroj, List<Natjecatelj> ulaznaListaNatjecatelja)
        {
            Natjecatelj noviClanTima = DodavanjeNatjecatelja(ulaznaListaNatjecatelja);
            ulaznaListaTimova[ulazniRedniBroj].lClanoviTima.Add(noviClanTima);
            ulaznaListaTimova[ulazniRedniBroj].lIdClanovaTima.Add(noviClanTima.id);
            updateTimove(ulaznaListaTimova);
        }
        static void izbrisiTim(List<Tim> ulaznaListaTimova, int ulazniRedniBroj = 0, bool odabir = false)
        {
            /*
             * Funkcija koja nam sluzi za brisanje odredenog tima.
             */
            if(odabir)
            {
                //prikaziTimove();
                Console.WriteLine("Unesite redni broj tima kojeg zelite izbrisati");
                int redniBrojTimaZaIbrisat = Convert.ToInt32(Console.ReadLine()) - 1;
                Console.WriteLine("Brisemo tim {0}", ulaznaListaTimova[redniBrojTimaZaIbrisat].imeTima);
                ulaznaListaTimova.RemoveAt(redniBrojTimaZaIbrisat);
            }else
            {
                Console.WriteLine("Brisemo tim {0} ", ulaznaListaTimova[ulazniRedniBroj].imeTima);
               ulaznaListaTimova.RemoveAt(ulazniRedniBroj);
            }
            updateTimove(ulaznaListaTimova);
        }
        static Tim azurirajKapetanaTima(Tim ulazniTim, int odabir = -1)
        {
            /*
             * Funkcija koja sluzi za azuriranje tima.
             */
            Tim reTim = new Tim();
            if (odabir != -1)
            {
                Console.WriteLine("Odaberite novog kapetana tima");
                for (int i = 0; i < ulazniTim.lClanoviTima.Count; i++)
                {
                    int redniBroj = 1 + i;
                    if (odabir != i)
                    {
                        Console.WriteLine("{0}. Ime Clana Tima: {1}", redniBroj, ulazniTim.lClanoviTima[i].imeNatjecatelja);
                    }
                }
                int odabirNovogKapetana = Convert.ToInt32(Console.ReadLine()) - 1;
                reTim = new Tim(ulazniTim.id, ulazniTim.imeTima, ulazniTim.lClanoviTima[odabirNovogKapetana], ulazniTim.lClanoviTima[odabirNovogKapetana].id,  ulazniTim.lClanoviTima, ulazniTim.lIdClanovaTima , ulazniTim.lIdProgramskihJezikaTima, ulazniTim.kontaktTima, ulazniTim.institucija);
            }
            else
            {
                for (int i = 0; i < ulazniTim.lClanoviTima.Count; i++)
                {
                    int redniBroj = 1 + i;
                    if (!ulazniTim.kapetanTima.Equals(ulazniTim.lClanoviTima[i]))
                    {
                        Console.WriteLine("{0}. Ime Clana Tima: {1}", redniBroj, ulazniTim.lClanoviTima[i].imeNatjecatelja);
                    }
                }
                int odabirNovogKapetana = Convert.ToInt32(Console.ReadLine()) - 1;
                reTim = new Tim(ulazniTim.id, ulazniTim.imeTima, ulazniTim.lClanoviTima[odabirNovogKapetana], ulazniTim.lClanoviTima[odabirNovogKapetana].id, ulazniTim.lClanoviTima, ulazniTim.lIdClanovaTima, ulazniTim.lIdProgramskihJezikaTima, ulazniTim.kontaktTima, ulazniTim.institucija);
            }
            return reTim;
        }
        static void izbrisiClanaTima(List<Tim> ulaznaListaTimova, int ulazniRedniBroj)
        {
            /*
             * Funkcija za brisanje odredenog clana iz tima
             */
            prikaziClanoveTima(ulaznaListaTimova[ulazniRedniBroj].lClanoviTima);
            Console.WriteLine("Odaberite kojeg clana zelite izbrisati");
            int odabir = Convert.ToInt32(Console.ReadLine()) - 1;
            if(ulaznaListaTimova[ulazniRedniBroj].lClanoviTima.Count <= 3)
            {
                izbrisiTim(ulaznaListaTimova, ulazniRedniBroj);
            }
            else
            {
                if(ulaznaListaTimova[ulazniRedniBroj].lClanoviTima[odabir].Equals(ulaznaListaTimova[ulazniRedniBroj].kapetanTima))
                {
                    ulaznaListaTimova[ulazniRedniBroj] = azurirajKapetanaTima(ulaznaListaTimova[ulazniRedniBroj], odabir);
                }
                ulaznaListaTimova[ulazniRedniBroj].lClanoviTima.RemoveAt(odabir);
                ulaznaListaTimova[ulazniRedniBroj].lIdClanovaTima.RemoveAt(odabir);
            }
            updateTimove(ulaznaListaTimova);
        }
        static void azurirajClanoveTima(List<Tim> ulaznaListaTimova, int ulazniRedniBroj, List<Natjecatelj> ulaznaListaNatjecatelja)
        {
            /*
             * Funkcija koja sluzi za azuriranje clanova tima
             */
            Console.WriteLine("1. Promijeni ime Clana Tima\n2. Dodaj Clana Tima\n3. Izbaci Clana Tima");
            string odabir = Console.ReadLine();
            bool validOdabir = true;
            while (validOdabir)
            {
                if(odabir == "2" && ulaznaListaTimova[ulazniRedniBroj].lClanoviTima.Count == 4)
                {
                    Console.WriteLine("Vec ima 4 clana u timu. Ne mozete dodati vise clanova. Ponovo unesite odabir.");
                    odabir = Console.ReadLine() ;
                }
                else if(odabir == "3" && ulaznaListaTimova[ulazniRedniBroj].lClanoviTima.Count == 3)
                {
                    Console.WriteLine("Tim ima 3 clana. Ako izbacite clana tim ce se obrisati");
                    validOdabir = false;
                }else
                {
                    validOdabir = false;
                }
            }
            switch(odabir)
            {
                case "1":
                Natjecatelj updateanNatjecatelj = promijeniImeClanaTima(ulaznaListaTimova[ulazniRedniBroj].lClanoviTima);
                if (updateanNatjecatelj.id == ulaznaListaTimova[ulazniRedniBroj].kapetanTima.id)
                {
                    ulaznaListaTimova[ulazniRedniBroj] = new Tim(ulaznaListaTimova[ulazniRedniBroj].id, ulaznaListaTimova[ulazniRedniBroj].imeTima, updateanNatjecatelj,updateanNatjecatelj.id ,ulaznaListaTimova[ulazniRedniBroj].lClanoviTima,ulaznaListaTimova[ulazniRedniBroj].lIdClanovaTima, ulaznaListaTimova[ulazniRedniBroj].lIdProgramskihJezikaTima, ulaznaListaTimova[ulazniRedniBroj].kontaktTima, ulaznaListaTimova[ulazniRedniBroj].institucija);
                }
                updateTimove(ulaznaListaTimova);
                    break;
                case "2":
                    dodajClanaTima(ulaznaListaTimova, ulazniRedniBroj, ulaznaListaNatjecatelja);
                    break;
                case "3":
                    izbrisiClanaTima(ulaznaListaTimova, ulazniRedniBroj);
                    break;
                default:
                    Console.WriteLine("Error pri odabiru kod azuriranja clanova tima");
                    break;
            }
        }
        static void prikaziProgramskeJezikeTima(List<ProgramskiJezik> ulaznaListaProgramskihJezika, List<Guid> ulaznaListaProgramskihJezikaTima)
        { 
            /*
             * Funkcija pomocu koje prikazujemo programske jezike
             */
            Console.WriteLine("Programski Jezici tima su:");
            for(int i = 0; i < ulaznaListaProgramskihJezikaTima.Count; i++)
            {     
                foreach(ProgramskiJezik pJezik in ulaznaListaProgramskihJezika)
                {
                    if(pJezik.id == ulaznaListaProgramskihJezikaTima[i])
                    {
                        int redniBroj = i + 1;
                        Console.WriteLine("{0}.\t {1}", redniBroj, pJezik.imeProgramskogJezika);
                    }
                }
            }
        }
        static int izbrisiProgramskiJezikTima(List<ProgramskiJezik> ulaznaListaProgramskihJezika, List<Guid> ulaznaListaProgramskihJezikaTima)
        {
            /*
             * Funkcija koja sluzi da izbrisemo programski jezik tima
             */
            int odabir = -1;
            if(ulaznaListaProgramskihJezikaTima.Count != 0)
            {
                prikaziProgramskeJezikeTima(ulaznaListaProgramskihJezika, ulaznaListaProgramskihJezikaTima);
                Console.WriteLine("Unesite redni broj programskog jezika koji zelite izbrisati");
                odabir = Convert.ToInt32(Console.ReadLine()) - 1;
            }
            else
            {
                Console.WriteLine("Tim nema prijavljene programske jezike za natjecanje");
            }
            return odabir;
        }
        static List<Guid> azurirajProgramskeJezike(List<ProgramskiJezik> ulaznaListaProgramskihJezika, List<Guid> ulaznaListaProgramskihJezikaTima)
        {
            /*
             * Funkcija koja sluzi za azuriranje Programskih jezika tima
             */
            List<Guid> retProgramskiJezici = new List<Guid>(ulaznaListaProgramskihJezikaTima);
            prikaziProgramskeJezikeTima(ulaznaListaProgramskihJezika, ulaznaListaProgramskihJezikaTima);
            Console.WriteLine("1. Dodaj Programski jezik\n2. Izbrisi Programski jezik");
            string odabir = Console.ReadLine();
            switch(odabir)
            {
                case "1":
                    int rBr = 1;
                    List<ProgramskiJezik> lProgramskihJezikaKojiNisuUTimu = new List<ProgramskiJezik>();
                    for (int i = 0; i < ulaznaListaProgramskihJezika.Count; i++)
                    {
                        if (!ulaznaListaProgramskihJezikaTima.Contains(ulaznaListaProgramskihJezika[i].id))
                        {
                            Console.WriteLine($"{rBr}\t{ulaznaListaProgramskihJezika[i].imeProgramskogJezika}");
                            rBr++;
                            lProgramskihJezikaKojiNisuUTimu.Add(ulaznaListaProgramskihJezika[i]);
                        }
                    }
                    Console.WriteLine("Unesite redni broj programskog jezika kojeg želite dodati");
                    int odabirDodanogProgramskogJezika = Convert.ToInt32(Console.ReadLine())-1;
                    retProgramskiJezici.Add(lProgramskihJezikaKojiNisuUTimu[odabirDodanogProgramskogJezika].id);
                    break;
                case "2":
                    int programskiJezikZaIzbrisat = izbrisiProgramskiJezikTima(ulaznaListaProgramskihJezika, ulaznaListaProgramskihJezikaTima);
                    if (programskiJezikZaIzbrisat != -1)
                    {
                        retProgramskiJezici.RemoveAt(programskiJezikZaIzbrisat);
                    }
                    break;
                default:
                    Console.WriteLine("Error pri switch-u u funkciji azurirajProgramskeJezike");
                    break;
            }
            return retProgramskiJezici;
        }
        static void azurirajTimove(List<Tim> ulaznaListaTimova, List<ProgramskiJezik> ulaznaListaProgramskihJezika, List<Natjecatelj> ulaznaListaNatjecatelja)
        {
            /*
             * Funkcija koja sluzi da se azuriraju timove
             */
            PrikaziTimove(ulaznaListaTimova, ulaznaListaNatjecatelja, ulaznaListaProgramskihJezika);
            Console.WriteLine("Odaberite redni broj tima kojeg želite ažurirati");
            int redniBroj = Convert.ToInt32(Console.ReadLine()) - 1;
            Console.WriteLine("Koju informaciju želite ažurirati za tim {0}.", redniBroj + 1);
            Console.WriteLine("1. Naziv Tima\n2. Clanove tima\n3. Kapetana tima\n4. Kontakt\n5. Programske jezike\n6. Instituciju");
            string odabir = Console.ReadLine();

            switch(odabir)
            {
                case "1":
                    ZapisiLog("Azurirao naziv tima");
                    azurirajNazivTima(ulaznaListaTimova, redniBroj);
                    break;
                case "2":
                    ZapisiLog("Azurirao clanove tima");
                    azurirajClanoveTima(ulaznaListaTimova, redniBroj, ulaznaListaNatjecatelja);
                    break;
                case "3":
                    ZapisiLog("Azurirao kapetana tima");
                    ulaznaListaTimova[redniBroj] = azurirajKapetanaTima(ulaznaListaTimova[redniBroj]);
                    break;
                case "4":
                    ZapisiLog("Azurirao kontakt tima");
                    ulaznaListaTimova[redniBroj] = new Tim(ulaznaListaTimova[redniBroj].id, ulaznaListaTimova[redniBroj].imeTima, ulaznaListaTimova[redniBroj].kapetanTima, ulaznaListaTimova[redniBroj].idKapetanaTima, ulaznaListaTimova[redniBroj].lClanoviTima, ulaznaListaTimova[redniBroj].lIdClanovaTima, ulaznaListaTimova[redniBroj].lIdProgramskihJezikaTima, DodavanjeKontakta(), ulaznaListaTimova[redniBroj].institucija);
                    break;
                case "5":
                    ZapisiLog("Azurirao programske jezike tima");
                    ulaznaListaTimova[redniBroj] = new Tim(ulaznaListaTimova[redniBroj].id, ulaznaListaTimova[redniBroj].imeTima, ulaznaListaTimova[redniBroj].kapetanTima, ulaznaListaTimova[redniBroj].idKapetanaTima, ulaznaListaTimova[redniBroj].lClanoviTima, ulaznaListaTimova[redniBroj].lIdClanovaTima, azurirajProgramskeJezike(ulaznaListaProgramskihJezika, ulaznaListaTimova[redniBroj].lIdProgramskihJezikaTima), ulaznaListaTimova[redniBroj].kontaktTima, ulaznaListaTimova[redniBroj].institucija);
                    break;
                case "6":
                    ZapisiLog("Azurirao instituciju");
                    ulaznaListaTimova[redniBroj] = new Tim(ulaznaListaTimova[redniBroj].id, ulaznaListaTimova[redniBroj].imeTima, ulaznaListaTimova[redniBroj].kapetanTima, ulaznaListaTimova[redniBroj].idKapetanaTima, ulaznaListaTimova[redniBroj].lClanoviTima, ulaznaListaTimova[redniBroj].lIdClanovaTima, ulaznaListaTimova[redniBroj].lIdProgramskihJezikaTima, ulaznaListaTimova[redniBroj].kontaktTima, DodavanjeInstitucije());
                    break;
                default:
                    Console.WriteLine("Error pri switch u funkciji azurirajTimove");
                    break;
            }
            updateTimove(ulaznaListaTimova);
        }
        
        static void dodavanjeTima(List<Tim> ulaznaListaTimova, List<Natjecatelj> ulaznaListaNatjecatelja, List<ProgramskiJezik> ulaznaListaProgramskihJezika)
        {
            /*
             * Funkcija koja sluzi za dodavanje Tima
             */
            List<Natjecatelj> lNatjecateljaUTimu = new List<Natjecatelj>();
            List<Guid> odabraniProgramskiJezik = prikaziProgramskeJezike(ulaznaListaProgramskihJezika);
            foreach(Tim tim in ulaznaListaTimova)
            {
                foreach(Natjecatelj natjecatelj in tim.lClanoviTima)
                {
                    lNatjecateljaUTimu.Add(natjecatelj);
                }
            }
            List<Natjecatelj> lSlobodnihNatjecatelja = ulaznaListaNatjecatelja.Except(lNatjecateljaUTimu).ToList();
            string odabir;
            int brojClanova = 0;
            bool invalid = true;
            do
            {
                Console.WriteLine("1. Dodaj 3 člana u tim\n2. Dodaj 4 člana u tim");
                odabir = Console.ReadLine();
                switch (odabir)
                {
                    case "1":
                        brojClanova = 3;
                        invalid = false;
                        break;
                    case "2":
                        brojClanova = 4;
                        invalid = false;
                        break;
                    default:
                        Console.WriteLine("Pogrešan unos");
                        break;
                }
            } while (invalid);
            Natjecatelj kapetanNovogTima = new Natjecatelj();
            Guid idKapetanaNovogTima = new Guid() ;
            List<Natjecatelj> lNatjecateljaNovogTima = new List<Natjecatelj>();
            List<Guid> lIdNatjecateljaNovogTima = new List<Guid>();
            bool kapetanCheck = true;
            do
            {
                Console.WriteLine("1. Dodaj člana bez tima\n2. Dodaj novog člana");
                string odabirKakoDodatiČlana = Console.ReadLine();
                switch (odabirKakoDodatiČlana)
                {
                    case "1":
                        if(lSlobodnihNatjecatelja.Count == 0)
                        {
                            Console.WriteLine("Nema natjecatelja bez tima");
                        }
                        else
                        {
                            if (kapetanCheck)
                            {
                                int max = 1;
                                Console.WriteLine("Dodajete kapetana tima");
                                for(int i = 0; i < lSlobodnihNatjecatelja.Count; i++)
                                {
                                    Console.WriteLine($"{i+1}\t{lSlobodnihNatjecatelja[i].imeNatjecatelja} {lSlobodnihNatjecatelja[i].prezimeNatjecatelja}");
                                    max++;
                                }
                                Console.WriteLine("Unesite odabir člana kojeg želite dodati kao kapetana");
                                int odabirKap = Convert.ToInt32(Console.ReadLine()) - 1;
                                while(odabirKap<0 || odabirKap > max)
                                {
                                    Console.WriteLine("Unijeli ste pogrešan broj");
                                    odabirKap = Convert.ToInt32(Console.ReadLine()) -1;
                                }
                                kapetanNovogTima = lSlobodnihNatjecatelja[odabirKap];
                                idKapetanaNovogTima = lSlobodnihNatjecatelja[odabirKap].id;
                                lNatjecateljaNovogTima.Add(lSlobodnihNatjecatelja[odabirKap]);
                                lIdNatjecateljaNovogTima.Add(lSlobodnihNatjecatelja[odabirKap].id);
                                lSlobodnihNatjecatelja.Remove(lSlobodnihNatjecatelja[odabirKap]);
                                kapetanCheck = false;
                            }
                            else
                            {
                                int max = 1;
                                Console.WriteLine("Odaberite kojeg člana želite dodati u tim");
                                for(int i = 0; i < lSlobodnihNatjecatelja.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}\t{lSlobodnihNatjecatelja[i].imeNatjecatelja} {lSlobodnihNatjecatelja[i].prezimeNatjecatelja}");
                                    max++;
                                }
                                int odabirClana = Convert.ToInt32(Console.ReadLine())-1;
                                while(odabirClana < 0 || odabirClana > max)
                                {
                                    Console.WriteLine("Unijeli ste pogresan broj");
                                    odabirClana = Convert.ToInt32(Console.ReadLine())-1;
                                }
                                lNatjecateljaNovogTima.Add(lSlobodnihNatjecatelja[odabirClana]);
                                lIdNatjecateljaNovogTima.Add(lSlobodnihNatjecatelja[odabirClana].id);
                                lSlobodnihNatjecatelja.Remove(lSlobodnihNatjecatelja[odabirClana]);
                            }
                        }
                        break;
                    case "2":
                        if (kapetanCheck)
                        {
                            Console.WriteLine("Dodajete kapetana tima");
                            kapetanNovogTima = DodavanjeNatjecatelja(ulaznaListaNatjecatelja);
                            idKapetanaNovogTima = kapetanNovogTima.id;
                            lNatjecateljaNovogTima.Add(kapetanNovogTima);
                            lIdNatjecateljaNovogTima.Add(kapetanNovogTima.id);
                            kapetanCheck = false;
                        }
                        else
                        {
                            Console.WriteLine("Dodajete člana tima");
                            Natjecatelj noviNatjecatelj = DodavanjeNatjecatelja(ulaznaListaNatjecatelja);
                            lIdNatjecateljaNovogTima.Add(noviNatjecatelj.id);
                            lNatjecateljaNovogTima.Add(noviNatjecatelj);
                        }
                        break;
                    default:
                        Console.WriteLine("Krivi unos");
                        break;
                }
            } while (lNatjecateljaNovogTima.Count != brojClanova);
            Console.WriteLine("Unesite ime tima");
            string imeNovogTima = Console.ReadLine();
            ulaznaListaTimova.Add(new Tim(Guid.NewGuid(), imeNovogTima, kapetanNovogTima, idKapetanaNovogTima,lNatjecateljaNovogTima,lIdNatjecateljaNovogTima,odabraniProgramskiJezik, DodavanjeKontakta(), DodavanjeInstitucije()));
            string noviJson = JsonConvert.SerializeObject(ulaznaListaTimova);
            ZapisiDatoteku("timovi.json", noviJson);
        }
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        static int generirajRandom(int min, int max)
        {
            lock(syncLock)
            {
                return random.Next(min, max);
            }
        }
        static int[] generirajBodove()
        {
            /*
             * Funkcija koja sluzi za generiranje random broja bodova.
             */
            int[] MAXBODOVI = new int[] { 30, 40, 60, 70 };
            int[] bodovi = new int[4];
            for (int i = 0; i < bodovi.Length; i++)
            {
                bodovi[i] = generirajRandom(0, MAXBODOVI[i]);
            }
            return bodovi;
        }
        static List<Rezultati> generirajKolo(List<Tim> ulaznaListaTimova, List<ProgramskiJezik> ulaznaListaProgramskihJezika, int redniBrojKola)
        {
            /*
             * Funkcija koja sluzi za generiranje kola
             */
            List<Rezultati> novoKolo = new List<Rezultati>();
            for(int i = 0; i < ulaznaListaProgramskihJezika.Count; i++)
            {
               foreach(Tim tim in ulaznaListaTimova)
                {
                    foreach(Guid progJezik in tim.lIdProgramskihJezikaTima)
                    {
                        if (progJezik == ulaznaListaProgramskihJezika[i].id)
                        {
                            int[] bodovi = generirajBodove();
                            int ukupanBrojBodova = 0;
                            foreach(int bod in bodovi)
                            {
                                ukupanBrojBodova += bod;
                            }
                            Rezultati rezultatiTima = new Rezultati(redniBrojKola, ulaznaListaProgramskihJezika[i].id, tim.id, generirajBodove(), ukupanBrojBodova);
                            novoKolo.Add(rezultatiTima);
                        }
                    }
                }
            }
            
            return novoKolo;
        }
        static void generirajRezultate(List<Tim> ulaznaListaTimova, List<ProgramskiJezik> ulaznaListaProgramskihJezika)
        {
            /*
             * Funkcija koja sluzi za generiranje Rezultata
             */
            int BROJKOLA = 10;
            List<Rezultati> rezultatiNatjecanja = new List<Rezultati>();
            for(int i=0; i < BROJKOLA; i++)
            {           
                foreach(Rezultati rez in generirajKolo(ulaznaListaTimova, ulaznaListaProgramskihJezika, i))
                {
                    rezultatiNatjecanja.Add(rez);
                }
            }
            string noviJson = JsonConvert.SerializeObject(rezultatiNatjecanja);
            ZapisiDatoteku("rezultati.json", noviJson);
        }
        static void PrikaziStranicu(List<Rezultati> ulaznaListaRezultata, List<Tim> ulaznaListaTimova, List<ProgramskiJezik> ulaznaListaProgramskihJezika, int rBr)
        {
            /*
             * Funkcija koja sluzi za prikazivanje stranice
             */
            /*foreach (Rezultati rez in ulaznaListaRezultata)
            {
                if (rBr == rez.redniBrojKola)
                {
                    foreach (Tim tim in ulaznaListaTimova)
                    {
                        if (tim.id == rez.idTima)
                        {
                            foreach (ProgramskiJezik pJezik in ulaznaListaProgramskihJezika)
                            {
                                if (pJezik.id == rez.idProgramskogJezika)
                                {
                                    Console.WriteLine($"Tim {tim.imeTima} ima bodova po zadacima u programskom jeziku {pJezik.imeProgramskogJezika}");
                                    for (int i = 0; i < rez.zadaci.Length; i++)
                                    {
                                        Console.WriteLine($"U zadatku {i + 1} tim ima {rez.zadaci[i]} bodova");
                                    }
                                    Console.WriteLine($"Tim ima ukupno bodova {rez.ukupniBrojBodova}");
                                }
                            }

                        }
                    }
                }
            }*/
            foreach(ProgramskiJezik pJezik in ulaznaListaProgramskihJezika)
            {
                var tablica = new ConsoleTable("Ime tima", "Programski Jezik", "Ukupan broj bodova", "Broj bodova po zadacima");
                foreach(Rezultati rez in ulaznaListaRezultata)
                {
                    foreach(Tim tim in ulaznaListaTimova)
                    {
                        if(rez.redniBrojKola == rBr && rez.idTima == tim.id && rez.idProgramskogJezika == pJezik.id)
                        {
                            string bodoviPoZadacima = "";
                            for(int i = 0; i < rez.zadaci.Length; i++)
                            {
                                bodoviPoZadacima += $"{i + 1}: {rez.zadaci[i]} ";
                            }
                            tablica.AddRow(tim.imeTima, pJezik.imeProgramskogJezika, rez.ukupniBrojBodova, bodoviPoZadacima);
                        }
                    }
                }
                tablica.Write(Format.Alternative);
            }
        }
        static void pronadiPobjednika(List<Rezultati> ulaznaListaRezultata, List<Tim> ulaznaListaTimova, List<ProgramskiJezik> ulaznaListaProgramskihJezika, int rBr)
        {
           /*
            * Funkcija koja sluzi da pronademo pobjednika
            */
            foreach(ProgramskiJezik pJezik in ulaznaListaProgramskihJezika)
            {
                Rezultati rezultatPobjednika = new Rezultati();
                foreach (Rezultati rez in ulaznaListaRezultata)
                {
                    if(rBr == rez.redniBrojKola)
                    {
                        if(pJezik.id == rez.idProgramskogJezika)
                        {
                            if(rezultatPobjednika.ukupniBrojBodova < rez.ukupniBrojBodova)
                            {
                                rezultatPobjednika = new Rezultati(rez.redniBrojKola, rez.idProgramskogJezika, rez.idTima, rez.zadaci, rez.ukupniBrojBodova);
                            }
                        }
                    }
                }
                foreach(Tim tim in ulaznaListaTimova)
                {
                    if(tim.id == rezultatPobjednika.idTima)
                    {
                        Console.WriteLine($"Pobjednik u programskom jeziku {pJezik.imeProgramskogJezika} je tim: {tim.imeTima} s ukupno bodova {rezultatPobjednika.ukupniBrojBodova}");
                    }
                }
            }
        }
        static void IzracunajUkupanRezultat(List<Rezultati> ulaznaListaRezultata)
        {
            /*
             * Funkcija koja sluzi da se izracuna ukupan rezultat
             */
            for (int i = 1; i < 10; i++)
            {
                for (int j = 0; j < ulaznaListaRezultata.Count; j++)
                {
                    if (ulaznaListaRezultata[j].redniBrojKola == i)
                    {
                        int stariUkupan = 0;
                        foreach (Rezultati rez in ulaznaListaRezultata)
                        {
                            if (rez.idTima == ulaznaListaRezultata[j].idTima && rez.redniBrojKola == ulaznaListaRezultata[j].redniBrojKola - 1 && rez.idProgramskogJezika == ulaznaListaRezultata[j].idProgramskogJezika)
                            {
                                stariUkupan = rez.ukupniBrojBodova;
                            }
                        }
                        int noviUkupan = ulaznaListaRezultata[j].ukupniBrojBodova + stariUkupan;
                        ulaznaListaRezultata[j] = new Rezultati(ulaznaListaRezultata[j].redniBrojKola, ulaznaListaRezultata[j].idProgramskogJezika, ulaznaListaRezultata[j].idTima, ulaznaListaRezultata[j].zadaci, noviUkupan);
                    }
                }
            }
        }
        static void SortirajRezultate(List<Rezultati> ulaznaListaRezultata)
        {
            /*
             * Funkcija koja sluzi za sortiranje rezultata
             */
            for(int i = 0; i < ulaznaListaRezultata.Count; i++)
            {
                int max = ulaznaListaRezultata[i].ukupniBrojBodova;
                int indeksMax = i;
                for(int j = i+1; j < ulaznaListaRezultata.Count; j++)
                {
                    if(max < ulaznaListaRezultata[j].ukupniBrojBodova)
                    {
                        max = ulaznaListaRezultata[j].ukupniBrojBodova;
                        indeksMax = j;
                    }
                }
                Rezultati temp = ulaznaListaRezultata[i];
                ulaznaListaRezultata[i] = ulaznaListaRezultata[indeksMax];
                ulaznaListaRezultata[indeksMax] = temp;
            }
        }
        static void PrikazRezultate(List<Rezultati> ulaznaListaRezultata, List<Tim> ulaznaListaTimova, List<ProgramskiJezik> ulaznaListaProgramskihJezika) 
        {
            /*
             * Funkcija koja sluzi za prikaz rezultata.
             */
            Console.Clear();
            List<int> ukupniBrojeviTimova = new List<int>();
            int rBr = 0;
            bool exit = true;
            IzracunajUkupanRezultat(ulaznaListaRezultata);
            SortirajRezultate(ulaznaListaRezultata);
            PrikaziStranicu(ulaznaListaRezultata, ulaznaListaTimova, ulaznaListaProgramskihJezika, rBr);
            pronadiPobjednika(ulaznaListaRezultata, ulaznaListaTimova, ulaznaListaProgramskihJezika, rBr);
            while (exit)
            {
                Console.WriteLine("1. Za Prethodnu stranicu\n2. Za sljedeću stranicu\n3. Za izlaz");
                string odabir = Console.ReadLine();
                switch (odabir)
                {
                    case "1":
                        if(rBr == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("Nema prethodne stranice");
                            PrikaziStranicu(ulaznaListaRezultata, ulaznaListaTimova, ulaznaListaProgramskihJezika, rBr);
                            pronadiPobjednika(ulaznaListaRezultata, ulaznaListaTimova, ulaznaListaProgramskihJezika, rBr);
                            Console.WriteLine($"Broj trenutačnog kola prikazanog na ekranu {rBr+1}");
                        }
                        else
                        {
                            Console.Clear();
                            rBr--;
                            PrikaziStranicu(ulaznaListaRezultata, ulaznaListaTimova, ulaznaListaProgramskihJezika, rBr);
                            pronadiPobjednika(ulaznaListaRezultata, ulaznaListaTimova, ulaznaListaProgramskihJezika, rBr);
                            Console.WriteLine($"Broj trenutačnog kola prikazanog na ekranu {rBr + 1}");
                        }
                        break;
                    case "2":
                        if (rBr == 9)
                        {
                            Console.Clear();
                            Console.WriteLine("Nema sljedeće stranice");
                            PrikaziStranicu(ulaznaListaRezultata, ulaznaListaTimova, ulaznaListaProgramskihJezika, rBr);
                            pronadiPobjednika(ulaznaListaRezultata, ulaznaListaTimova, ulaznaListaProgramskihJezika, rBr);
                            Console.WriteLine($"Broj trenutačnog kola prikazanog na ekranu {rBr + 1}");
                        }
                        else
                        {
                            Console.Clear();
                            rBr++;
                            PrikaziStranicu(ulaznaListaRezultata, ulaznaListaTimova, ulaznaListaProgramskihJezika, rBr);
                            pronadiPobjednika(ulaznaListaRezultata, ulaznaListaTimova, ulaznaListaProgramskihJezika, rBr);
                            Console.WriteLine($"Broj trenutačnog kola prikazanog na ekranu {rBr + 1}");
                        }
                        break;
                    case "3":
                        exit = false;
                        break;
                    default:
                        Console.WriteLine("krivi unos");
                        break;
                }
            }
           
        }

        static void SimulacijaLige(List<Tim> ulaznaListaTimova, List<ProgramskiJezik> ulaznaListaProgramskihJezika, List<Rezultati> ulaznaListaRezultata)
        {
            /*
             * Funkcija pomocu koje radimo simulaciju lige.
             */
            bool exit = true;
            while (exit)
            {
                Console.WriteLine("1. Pregled rezultata po kolu\n2. Generiranje novih rezultata\n3. izlaz");
                string odabir = Console.ReadLine();
                switch (odabir)
                {
                    case "1":
                        PrikazRezultate(ulaznaListaRezultata, ulaznaListaTimova, ulaznaListaProgramskihJezika);
                        break;
                    case "2":
                        generirajRezultate(ulaznaListaTimova, ulaznaListaProgramskihJezika);
                        break;
                    case "3":
                        exit = false;
                        break;
                    default:
                        Console.WriteLine("Krivi unos");
                        break;
                }
            }
        }
        static void PregledStatistike(List<Tim> ulaznaListaTimova, List<ProgramskiJezik> ulaznaListaProgramskihJezika)
        {
            /*
             * Funkcija za pregled statistike
             */
            Console.WriteLine($"Broj timova u natjecanju je{ulaznaListaTimova.Count}");
            int najzastupljenijiProgramskiJezik = 0;
            List<Guid> listaNajzastupljenijihJezika = new List<Guid>();
            List<String> lInstitucijaKojeSudjeluju = new List<String>();

            foreach (ProgramskiJezik pJezik in ulaznaListaProgramskihJezika)
            {
                int brojTimovaUJeziku = 0;
                foreach (Tim tim in ulaznaListaTimova)
                {
                    if (!lInstitucijaKojeSudjeluju.Contains(tim.institucija))
                    {
                        lInstitucijaKojeSudjeluju.Add(tim.institucija);
                    }
                    if (tim.lIdProgramskihJezikaTima.Contains(pJezik.id))
                    {
                        brojTimovaUJeziku++;
                    }
                }
                Console.WriteLine($"Broj timova koji se natječe u programskom jeziku {pJezik.imeProgramskogJezika} je {brojTimovaUJeziku}");
                if (brojTimovaUJeziku > najzastupljenijiProgramskiJezik)
                {
                    najzastupljenijiProgramskiJezik = brojTimovaUJeziku;
                }
            }
            foreach (ProgramskiJezik pjezik in ulaznaListaProgramskihJezika)
            {
                int brojTimovaUJeziku = 0;
                foreach (Tim tim in ulaznaListaTimova)
                {
                    if (tim.lIdProgramskihJezikaTima.Contains(pjezik.id))
                    {
                        brojTimovaUJeziku++;
                    }
                }
                if (brojTimovaUJeziku == najzastupljenijiProgramskiJezik)
                {
                    listaNajzastupljenijihJezika.Add(pjezik.id);
                }
            }
            if (listaNajzastupljenijihJezika.Count == 1)
            {
                foreach(ProgramskiJezik pJezik in ulaznaListaProgramskihJezika)
                {
                    if(listaNajzastupljenijihJezika[0] == pJezik.id)
                    {
                        Console.WriteLine($"Najzastupljeniji jezik na natjecanju je {pJezik.imeProgramskogJezika}");
                    }
                }
            }
            else
            {
                foreach(Guid id in listaNajzastupljenijihJezika)
                {
                    foreach(ProgramskiJezik pJezik in ulaznaListaProgramskihJezika)
                    {
                        if(pJezik.id == id)
                        {
                            Console.WriteLine($"Jedan od najzastupljenijih jezika na natjecanju je {pJezik.imeProgramskogJezika}");
                        }
                    }
                }
            }
            foreach(String institucija in lInstitucijaKojeSudjeluju)
            {
                int brojTimovaUInstituciji = 0 ;
                foreach(Tim tim in ulaznaListaTimova)
                {
                    if(institucija == tim.institucija)
                    {
                        brojTimovaUInstituciji++;
                    }
                }
                Console.WriteLine($"Iz institucije {institucija} sudjeluje {brojTimovaUInstituciji} timova u natjecanju");
            }
        }
        static void prikaziIzbornikAdmin(List<Tim> ulaznaListaTimova, List<Organizator> ulaznaListaOrganizatora, List<ProgramskiJezik> ulaznaListaProgJezika, List<Natjecatelj> ulaznaListaNatjecatelja, List<Rezultati> ulaznaListaRezultata, List<Obavijest> ulaznaListaObavijesti)
        {
            /*
             * Funkcija koja prikazuje izbornik za admina
             */
            bool exit = true;
            Console.WriteLine("Unesite vaš izbor");
            while (exit)
            {
                Console.WriteLine("1. Pregled svih timova\n2. Pregled organizatora natjecanja\n3. Dodavanje timova\n4. Dodavanje osoba\n5. Ažuriranje timova\n6. Brisanje timova\n7. Pretraživanje timova\n8. Simulacija Lige\n9. Statistika\n10. Za pregled obavijesti\n0. za Izlaz");
                string odabir = Console.ReadLine();
                Console.Clear();
                switch (odabir)
                {
                    case "0":
                        exit = false;
                        break;
                    case "1":
                        ZapisiLog("je odabrao prikazi timove");
                        PrikaziTimove(ulaznaListaTimova, ulaznaListaNatjecatelja, ulaznaListaProgJezika);
                        break;
                    case "2":
                        ZapisiLog("je odabrao prikazi organizatore");
                        prikaziOrganizatore(ulaznaListaOrganizatora, ulaznaListaProgJezika, false);
                        break;
                    case "3":
                        ZapisiLog("je odabrao dodavanje tima");
                        dodavanjeTima(ulaznaListaTimova, ulaznaListaNatjecatelja, ulaznaListaProgJezika);
                        break;
                    case "4":
                        DodavanjeOsobe(ulaznaListaOrganizatora, ulaznaListaProgJezika, ulaznaListaNatjecatelja);
                        break;
                    case "5":
                        ZapisiLog("je odabrao azuriranje tima");
                        azurirajTimove(ulaznaListaTimova, ulaznaListaProgJezika, ulaznaListaNatjecatelja);
                        break;
                    case "6":
                        ZapisiLog("je odabrao izbrisi tim");
                        izbrisiTim(ulaznaListaTimova: ulaznaListaTimova, odabir: true);
                        break;
                    case "7":
                        ZapisiLog("je odabrao pretrazi tim");
                        PretraziTimove(ulaznaListaTimova, ulaznaListaNatjecatelja, ulaznaListaProgJezika, ulaznaListaRezultata);
                        break;
                    case "8":
                        ZapisiLog("je odabrao simulacije lige");
                        SimulacijaLige(ulaznaListaTimova,ulaznaListaProgJezika,ulaznaListaRezultata);
                        break;
                    case "9":
                        ZapisiLog("je odabrao pregled statistike");
                        PregledStatistike(ulaznaListaTimova, ulaznaListaProgJezika);
                        break;
                    case "10":
                        ZapisiLog("je odabrao pregled obavijesti");
                        pregledObavijesti(ulaznaListaNatjecatelja, ulaznaListaOrganizatora, ulaznaListaObavijesti);
                        break;
                    default:
                        Console.WriteLine("Krivi unos");
                        break;
                }
            }
        }
        static void PrikaziDetaljneInformacijeOtimu(Tim timKojiPrikazujemo, List<Natjecatelj> ulaznaListaNatjecatelja, List<ProgramskiJezik> ulaznaListaProgramskihJezika, List<Rezultati> ulaznaListaRezultata)
        {
            /*
             * Funkcija koja prikazuje detaljne informacije o timu
             */
            Console.WriteLine("Natjecatelji koji su u tom timu:");
            foreach(Guid idNatjecateljaUtimu in timKojiPrikazujemo.lIdClanovaTima)
            {
                foreach(Natjecatelj natjecatelj in ulaznaListaNatjecatelja)
                {
                    if(natjecatelj.id == idNatjecateljaUtimu)
                    {
                        Console.WriteLine($"Ime natjecatelja {natjecatelj.imeNatjecatelja} {natjecatelj.prezimeNatjecatelja}, OIB natjecatelja {natjecatelj.OIBNatjecatelja}");
                    }
                }
            }
            foreach(Rezultati rezultat in ulaznaListaRezultata)
            {
                if(rezultat.idTima == timKojiPrikazujemo.id)
                {
                    string brojBodovaPoZadatcima = "";
                    int rBr = 1;
                    foreach(int brojBodova in rezultat.zadaci)
                    {
                        brojBodovaPoZadatcima += $"U zadatku {rBr++} ima {brojBodova} bodova\n";
                    }
                    string progJezik = "";
                    foreach(ProgramskiJezik pJezik in ulaznaListaProgramskihJezika)
                    {
                        if(pJezik.id == rezultat.idProgramskogJezika)
                        {
                            progJezik = $"{pJezik.imeProgramskogJezika}";
                        }
                    }
                    Console.WriteLine($"Tim u kolu {rezultat.redniBrojKola + 1} je ostvario {rezultat.ukupniBrojBodova} ukupno bodova u programskom jeziku {progJezik}. A po zadacima:\n{brojBodovaPoZadatcima}");
                }
            }
        }
        static void PretraziTimove(List<Tim> ulaznaListaTimova, List<Natjecatelj> ulaznaListaNatjecatelja, List<ProgramskiJezik> ulaznaListaProgramskihJezika, List<Rezultati> ulaznaListaRezultata)
        {
            /*
             * Funkcija koja nam sluzi za pretrazivanje timova.
             */
            Console.WriteLine("Unesite pojam koji želite tražiti");
            string pojam = Console.ReadLine();
            List<Tim> lTimovaKojiSadrzePojam = new List<Tim>();
            int rbr = 1;
            for(int i = 0; i < ulaznaListaTimova.Count; i++)
            {
                if (ulaznaListaTimova[i].imeTima.Contains(pojam))
                {
                    lTimovaKojiSadrzePojam.Add(ulaznaListaTimova[i]);
                }
            }
            for(int i = 0; i < lTimovaKojiSadrzePojam.Count; i++)
            {
                Console.WriteLine($"{rbr++}.\t{lTimovaKojiSadrzePojam[i].imeTima}");
            }
            bool loop = true;
            while (loop)
            {
                Console.WriteLine("1. Ako želite vidjeti detaljne rezultate o timu\n2. Za povratak");
                string odabir = Console.ReadLine();
                switch (odabir)
                {
                    case "1":
                        Console.WriteLine("Unesite Broj tima kojeg zelite pregledati");
                        int timKojiGledamo = Convert.ToInt32(Console.ReadLine());
                        ZapisiLog("Je odabrao pogledati detaljne informacije o nekom timu");
                        PrikaziDetaljneInformacijeOtimu(lTimovaKojiSadrzePojam[timKojiGledamo], ulaznaListaNatjecatelja, ulaznaListaProgramskihJezika, ulaznaListaRezultata);
                        break;
                    case "2":
                        loop = false;
                        break;
                    default:
                        Console.WriteLine("Krivi unos");
                        break;
                }
            }
        }
        static void prikaziIzbornikKorisnik(List<Tim> ulaznaListaTimova, List<Organizator> ulaznaListaOrganizatora, List<ProgramskiJezik> ulaznaListaProgJezika, List<Natjecatelj> ulaznaListaNatjecatelja, List<Rezultati> ulaznaListaRezultata, Guid idKorisnika)
        {
            /*
             *  ------------------------------------------------------------------------------------
             *  Funkcija prikazuje izbornik za korisnika u kojem on može unijeti svoj odabir.
             *  ------------------------------------------------------------------------------------
             */
            bool exit = true;
            do
            {
                Console.WriteLine("Unesite vaš izbor");
                Console.WriteLine("1. Pregled svih timova\n2. Pregled organizatora natjecanja\n3. Pretraživanje Tima\n4. Simulacija Lige\n5. Statistika\n6. Za izlaz iz programa");
                string odabir = Console.ReadLine();
                Console.Clear();
                switch (odabir)
                {
                    case "1":
                        ZapisiLog("Je odabrao opciju prikazi timove");
                        PrikaziTimove(ulaznaListaTimova, ulaznaListaNatjecatelja, ulaznaListaProgJezika);
                        break;
                    case "2":
                        ZapisiLog("Je odabrao opciju prikazi Organizatore");
                        prikaziOrganizatore(ulaznaListaOrganizatora, ulaznaListaProgJezika, true, idKorisnika); ;
                        break;
                    case "3":
                        ZapisiLog("Je odabrao opciju prikazi pretrazi timove");
                        PretraziTimove(ulaznaListaTimova, ulaznaListaNatjecatelja, ulaznaListaProgJezika, ulaznaListaRezultata);
                        break;
                    case "4":
                        ZapisiLog("Je odabrao opciju Simulacije Lige");
                        SimulacijaLige(ulaznaListaTimova, ulaznaListaProgJezika, ulaznaListaRezultata);
                        break;
                    case "5":
                        ZapisiLog("Je odabrao opciju pregled statistike");
                        PregledStatistike(ulaznaListaTimova, ulaznaListaProgJezika);
                        break;
                    case "6":
                        ZapisiLog("Je izasao iz programa");
                        exit = false;
                        break;
                    default:
                        Console.WriteLine("Error pri switch-u u funkciji prikaziIzbornikKorisnik");
                        break;
                }
            } while (exit);
        }
        static void Izbornik()
        {
            /*
             * -------------------------------------------------------------------------------------
             * Funkcija prikazuje odgovarajući izbornik korisniku nakon što se ulogirao u program.
             * -------------------------------------------------------------------------------------
             */
            Korisnik trenutacniKorisnik = Login();
            TRENUTACNI_KORISNIK = trenutacniKorisnik;
            ZapisiLog("se ulogirao");
            List<Tim> lTimova = JsonConvert.DeserializeObject<List<Tim>>(dohvatiDatoteku("timovi.json"));
            List<Organizator> lOrganizatora = JsonConvert.DeserializeObject<List<Organizator>>(dohvatiDatoteku("organizatori.json"));
            List<ProgramskiJezik> lPJezika = JsonConvert.DeserializeObject<List<ProgramskiJezik>>(dohvatiDatoteku("programski_jezici.json"));
            List<Natjecatelj> lNatjecatelja = JsonConvert.DeserializeObject<List<Natjecatelj>>(dohvatiDatoteku("natjecatelji.json"));
            List<Rezultati> lRezultata = JsonConvert.DeserializeObject<List<Rezultati>>(dohvatiDatoteku("rezultati.json"));
            List<Obavijest> lObavijesti = JsonConvert.DeserializeObject<List<Obavijest>>(dohvatiDatoteku("obavijesti.json"));
            switch (trenutacniKorisnik.razinaPravaKorisnika)
            {
                case "admin":
                    prikaziIzbornikAdmin(lTimova, lOrganizatora, lPJezika, lNatjecatelja, lRezultata, lObavijesti);
                    break;
                case "natjecatelj":
                    prikaziIzbornikKorisnik(lTimova, lOrganizatora, lPJezika, lNatjecatelja, lRezultata, trenutacniKorisnik.id);
                    break;
                default:
                    Console.WriteLine("Error pri prikazivanju izbornika");
                    break;
            }
        }
        
        static void Main(string[] args)
        {
            Izbornik();
        }
    }
}
