namespace CPTWorkouts.Models
{
    public class Treinadores:Utilizadores
    {
        public Treinadores()
        {
            ListaEquipas = new HashSet<Equipas>();
            Servicos = new HashSet<Servicos>();
        }

        public string TreinadorID { get; set; }

        /* ****************************************
         * Construção dos Relacionamentos
         * *************************************** */

        // relacionamento N-M
        public ICollection<Equipas> ListaEquipas { get; set; }

        // relacionamento N-M
        public ICollection<Servicos> Servicos { get; set; }

    }
}

