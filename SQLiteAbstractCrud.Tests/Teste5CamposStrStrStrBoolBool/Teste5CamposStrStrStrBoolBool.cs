namespace SQLiteAbstractCrud.Tests.Teste5CamposStrStrStrBoolBool
{
    public class Teste5CamposStrStrStrBoolBool
    {
        [PrimaryKey]
        public string Valor1 { get; init; }
        public string Valor2 { get; init; }
        public string Valor3 { get; init; }
        public bool Valor4 { get; init; }
        public bool Valor5 { get; init; }
        
        public Teste5CamposStrStrStrBoolBool(string valor1, string valor2, string valor3, bool valor4, bool valor5)
        {
            Valor1 = valor1;
            Valor2 = valor2;
            Valor3 = valor3;
            Valor4 = valor4;
            Valor5 = valor5;
        }
    }
}
