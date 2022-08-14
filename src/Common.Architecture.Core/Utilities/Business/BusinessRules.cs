using Common.Architecture.Core.Utilities.Results;

namespace Common.Architecture.Core.Utilities.Business
{
    public class BusinessRules
    {
        // params keyword'ü ile istediğimiz kadar iş kuralını Run methoduna yollayabileceğiz.
        // List<IResult>
        public static IResult Run(params IResult[] logics)
        {
            foreach (var logic in logics)
            {
                // List<IResult> errorResults = new List<IResult>();
                // logic'in Success durumu başarısız ise bunu döndürebiliriz.
                if (!logic.Success)
                {
                    // ErrorResult & ErrorDataResult
                    return logic;
                    // errorResults.Add(logic);
                }
            }
            return null;
        }
    }
}
