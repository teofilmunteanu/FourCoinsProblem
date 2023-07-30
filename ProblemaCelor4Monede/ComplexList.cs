using System;
namespace ComplexExplorer
{
     
    public class ComList   // Lista simplu inlantuita  
    {
        protected class ComNod     //noduri 
        {
            public Complex z;
            public ComNod next;

            public ComNod(Complex z)
            {
                this.z = z;     //nu este nevoie de copiere, Complex este o structura
                this.next = null;
            }
        }
        protected ComNod antet;
        protected ComNod ultim;
        protected ComNod index; //pentru parcurgerea listei
        public ComList()
        {
            ultim = antet = new ComNod(0);
            index = null;
        }

        public Complex firstZet
        {
            get     //initiaza parcurgerea listei si returneaza prima valoare
            {
                index = antet.next;
                if (index != null) return index.z;
                return 0; //daca lista e vida returnam 0
            }
        }

        public bool ended
        {
            get     //semnaleaza daca s-a terminat parcurgerea listei
            {
                return (index == null);
            }
        }

        public Complex nextZet
        {
            get     //incrementeaza indexul si returneaza valoarea gasita
            {
                if (index != null) index = index.next;
                if (index != null) return index.z;
                return 0; //ATENTIE: dincolo de capatul listei returnam 0!
            }
            set     //adauga valoarea la sfarsitul listei
            {
                ultim = ultim.next = new ComNod(value);
            }

        }
    }

    public class GenList<T> where T: struct   // Lista Generica simplu inlantuita  
    {
        protected class GenNod     //noduri 
        {
            public T z;
            public GenNod next;

            public GenNod(T z)
            {
                this.z = z; 
                this.next = null;
            }
        }
        protected GenNod antet;
        protected GenNod ultim;
        protected GenNod index; //pentru parcurgerea listei
        public GenList()
        {
            ultim = antet = new GenNod(default(T));
            index = null;
        }

        public T firstZet
        {
            get     //initiaza parcurgerea listei si returneaza prima valoare
            {
                index = antet.next;
                if (index != null) return index.z;
                return default(T); //daca lista e vida returnam 0
            }
        }

        public bool ended
        {
            get     //semnaleaza daca s-a terminat parcurgerea listei
            {
                return (index == null);
            }
        }

        public T nextZet
        {
            get     //incrementeaza indexul si returneaza valoarea gasita
            {

                if (index != null) index = index.next;
                if (index != null) return index.z;
                return default(T);  //ATENTIE: dincolo de capatul listei returnam 0
            }
            set     //adauga valoarea la sfarsitul listei
            {
                ultim = ultim.next = new GenNod(value);
            }

        }
    }
    
}