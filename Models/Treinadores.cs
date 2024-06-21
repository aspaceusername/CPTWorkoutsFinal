namespace CPTWorkouts.Models
{
    public class Treinadores:Utilizadores
    {
        public Treinadores()
        {
            ListaEquipas = new HashSet<Equipas>();
        }

        /* ****************************************
         * Construção dos Relacionamentos
         * *************************************** */

        // relacionamento N-M
        public ICollection<Equipas> ListaEquipas { get; set; }

    }
}

