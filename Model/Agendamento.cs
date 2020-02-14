using System;

namespace Model
{
    public class Agendamento
    {
        public int codigo { get; set; }
        public Prontuario Prontuario { get; set; }
        public string nome_tmp { get; set; }
        public string telcelular_tmp { get; set; }
        public string telfixo_tmp { get; set; }
        public DateTime data_nascimento { get; set; }
        public DateTime data_agendamento { get; set; }
        public string hora { get; set; }
        public string consultorio { get; set; }
        public string observacoes { get; set; }
        public DateTime data_registro { get; set; }
        public string tipo { get; set; }
        public string status { get; set; }
    }
}
