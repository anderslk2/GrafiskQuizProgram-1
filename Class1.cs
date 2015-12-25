using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ConsoleApplication1
{
//TODO Fjern denne : test kommentar til at illustrere om GitHub virker
    class Datahandler
    {
        int maxsymptomantal;
        public XElement doc;
        public string sygdom;
        public List<string> kandidatsymptomer;
        public List<string> korrektesymptomer;
        private Random rnd;

        public Datahandler(int antal)
        {
            maxsymptomantal = antal;
        }

        public void indlæsdata(string stinavn)
        {
            rnd = new Random();
            doc = XElement.Load(stinavn);
        }

        public void VælgSygdomAtSpørgeOm()
        {
            sygdom = doc.Elements("sygdom").OrderBy(elt => rnd.Next()).ToList()[0].Attribute("navn").Value;

            korrektesymptomer = doc.Elements("sygdom").Where(asymp => asymp.Attribute("navn").Value == sygdom).ToList().SelectMany(syg => syg.Elements("symptom").ToList(), (a, b) => b.Attribute("navn").Value).ToList();
            kandidatsymptomer = GenererListeAfkandidatsymptomer();
        }

        List<string> GenererListeAfkandidatsymptomer()
        {
            List<string> AlleSymptomer = doc.Elements("sygdom").ToList<XElement>().SelectMany(syg => syg.Elements("symptom").ToList(), (a, b) => b.Attribute("navn").Value).ToList();

            var SymptomerIkkeFordenneSygdom = AlleSymptomer.Except(korrektesymptomer).OrderBy(sfds => rnd.Next()); ;

            var maxantalsymptomerfralister = Math.Min((decimal)SymptomerIkkeFordenneSygdom.Count(), (decimal)korrektesymptomer.Count());
            List<string> displaysymptomer = new List<string>() { korrektesymptomer.ToList()[0] };

            for (int i = 1; i < Math.Min((decimal)maxsymptomantal, (decimal)maxantalsymptomerfralister); i++)
            {
                if (rnd.Next(2) == 0)
                    displaysymptomer.Add(korrektesymptomer.ToList()[i]);
                else
                    displaysymptomer.Add(SymptomerIkkeFordenneSygdom.ToList()[i]);
            }
            displaysymptomer = displaysymptomer.OrderBy(ds => rnd.Next(1000)).ToList();

            return displaysymptomer;
        }
    }
}
