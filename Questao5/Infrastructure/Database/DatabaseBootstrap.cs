using System.Data;
using Dapper;

namespace Infrastructure.Database
{
    public static class DatabaseBootstrap
    {
        public static void Initialize(IDbConnection dbConnection)
        {
            dbConnection.Execute(@"
                CREATE TABLE IF NOT EXISTS contacorrente (
                    idcontacorrente TEXT PRIMARY KEY,
                    numero INTEGER UNIQUE NOT NULL,
                    nome TEXT NOT NULL,
                    ativo INTEGER NOT NULL CHECK (ativo IN (0,1))
                );
                CREATE TABLE IF NOT EXISTS movimento (
                    idmovimento TEXT PRIMARY KEY,
                    idcontacorrente TEXT NOT NULL,
                    datamovimento TEXT NOT NULL,
                    tipomovimento TEXT NOT NULL CHECK (tipomovimento IN ('C', 'D')),
                    valor REAL NOT NULL,
                    FOREIGN KEY(idcontacorrente) REFERENCES contacorrente(idcontacorrente)
                );
                CREATE TABLE IF NOT EXISTS idempotencia (
                    chave_idempotencia TEXT PRIMARY KEY,
                    requisicao TEXT,
                    resultado TEXT
                );
            ");
        }
    }
}