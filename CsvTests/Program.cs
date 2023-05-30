using System.Text;
using System.Text.RegularExpressions;

namespace CsvTests;

public class Program
{
    public static void Main(string[] args)
    {
        string csvFilePath = Path.Combine(@"C:\DevOps\teste\FormataDadosGMDeCSV\Data\GM.csv");
        string txtFilePath = Path.Combine(@"C:\DevOps\teste\FormataDadosGMDeCSV\Data\output.txt");

        var headerMapping = new Dictionary<string, string>
        {
            { "numslc_mud", "Número da GM" },
            { "dscast", "Descrição" },
            { "nome", "Nome do desenvolvedor" },
            { "inf_st", "Sistema" },
            { "inf_bd", "Script de banco" },
            { "dsc_celula_grupo_software", "Mesa" },
            { "versao_sistema", "Versão do Sistema" },
            { "indcrs_sis", "Principal" }
        };

        Encoding enc = Encoding.UTF8;
        string[] lines = File.ReadAllLines(csvFilePath, enc);

        #region 
        for (int i = 0; i < lines.Length; i++)
        {
            string modifiedLine = Regex.Replace(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)", ";");
            lines[i] = modifiedLine;
        }
        #endregion

        string headerLine = lines[0];
        string[] headers = headerLine.Split(';');
        var columnData = new Dictionary<string, List<string>>();

        for (int i = 0; i < headers.Length; i++)
        {
            if (headerMapping.ContainsKey(headers[i]))
            {
                string aux = "";
                bool success1 = headerMapping.TryGetValue(headers[i], out aux);
                headers[i] = aux;
            }
        }

        foreach (string header in headers)
            columnData[header] = new List<string>();

        for (int i = 1; i < lines.Length; i++)
        {
            string dataLine = lines[i];
            string[] values = dataLine.Split(';');

            // Assign each value to the corresponding column
            for (int j = 0; j < values.Length; j++)
            {
                string header = headers[j];
                string value = values[j];

                columnData[header].Add(value);
            }
        }
        var formattedData = new System.Text.StringBuilder();

        for (int x = 0; x < lines.Length-1; x++)
        {
            foreach (string header in headers)
            {
                List<string> columnValues = columnData[header];
                for(int i=0; i<1; i++)
                {
                    formattedData.Append(header);
                    formattedData.Append(": ");
                    formattedData.AppendLine(columnValues[x]);
                }
            }

            formattedData.AppendLine();
        }

        File.WriteAllText(txtFilePath, formattedData.ToString(), enc);
    }
}

