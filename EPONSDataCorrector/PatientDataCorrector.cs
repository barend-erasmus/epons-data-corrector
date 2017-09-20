using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPONSDataCorrector
{
    public class PatientDataCorrector
    {
        private readonly DbExecutor _dbExecutor;

        public PatientDataCorrector()
        {
            string host = "epons.dedicated.co.za";
            string user = "";
            string name = "SADFM_Live";
            string password = "";

            string connectionString = $"data source={host};Initial Catalog={name};User ID={user};Password={password};";

            _dbExecutor = new DbExecutor(connectionString);
        }

        public void RemoveDuplicates()
        {
            IList<Guid[]> duplicates = FindDuplicates();

            foreach (Guid[] duplicate in duplicates)
            {
                Guid keepPatientId = Guid.Empty;

                int maxVisitCount = -1;

                foreach (Guid id in duplicate)
                {
                    int visitCount = VisitCount(id);

                    if (visitCount > maxVisitCount)
                    {
                        keepPatientId = id;
                        maxVisitCount = visitCount;
                    }
                }

                if (keepPatientId != Guid.Empty)
                {
                    foreach (Guid id in duplicate)
                    {
                        if (id != keepPatientId)
                        {
                            RemoveDuplicate(keepPatientId, id);
                        }
                    }
                }
            }
        }

        public IList<Guid[]> FindDuplicates()
        {
            IList<dynamic> patients = _dbExecutor.Query<dynamic>("SELECT  * FROM [Patient].[Details]", null);

            IList<Guid[]> result = new List<Guid[]>();

            foreach (dynamic patient in patients)
            {

                if (result.Count((x) => x.Count((y) => y == patient.PatientId) > 1) > 1)
                {
                    continue;
                }

                IList<dynamic> patientMatches = patients.Where((x) => 
                x.Firstname == patient.Firstname &&
                x.Lastname == patient.Lastname &&
                x.DateOfBirth == patient.DateOfBirth &&
                x.IdentificationNumber == x.IdentificationNumber).ToList();

                if (patientMatches.Count > 1)
                {
                    result.Add(patientMatches.Select((x) => (Guid)x.PatientId).ToArray());
                }
            }

            return result;
        }

        public void RemoveDuplicate(Guid keepPatientId, Guid removePatientId)
        {
            _dbExecutor.QueryProc<dynamic>("[EPONS].[RemoveDuplicatePatient]", new
            {
                keepPatientId = keepPatientId,
                removePatientId = removePatientId
            });
        }

        public int VisitCount(Guid patientId)
        {
            dynamic visits = _dbExecutor.QueryOne<dynamic>("SELECT COUNT(*) AS NumberOfVisits FROM [Visit].[Details] WHERE [PatientId] = @patientId", new
            {
                patientId = patientId
            });

            return visits.NumberOfVisits;
        }

        public int LevenshteinDistance(string source1, string source2)
        {
            var source1Length = source1.Length;
            var source2Length = source2.Length;

            var matrix = new int[source1Length + 1, source2Length + 1];

            // First calculation, if one entry is empty return full length
            if (source1Length == 0)
                return source2Length;

            if (source2Length == 0)
                return source1Length;

            // Initialization of matrix with row size source1Length and columns size source2Length
            for (var i = 0; i <= source1Length; matrix[i, 0] = i++) { }
            for (var j = 0; j <= source2Length; matrix[0, j] = j++) { }

            // Calculate rows and collumns distances
            for (var i = 1; i <= source1Length; i++)
            {
                for (var j = 1; j <= source2Length; j++)
                {
                    var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }

            // return result
            return matrix[source1Length, source2Length];
        }
    }
}
