using System.Globalization;

namespace Questao1
{
     public class ContaBancaria
    {
        
        public int NumeroConta { get; } 
        public string Titular { get; set; }
        private double _saldo;

        public ContaBancaria(int numeroConta, string titular)
        {
            NumeroConta = numeroConta;
            Titular = titular;
            _saldo = 0;  
        }

        public ContaBancaria(int numeroConta, string titular, double depositoInicial)
        {
            NumeroConta = numeroConta;
            Titular = titular;
            Deposito(depositoInicial);
        }

        public void Deposito(double valor)
        {
            _saldo += valor;
        }

        public void Saque(double valor)
        {
            _saldo -= (valor + 3.50);
        }

        public double Saldo
        {
            get { return _saldo; }
        }

      
    }
}
