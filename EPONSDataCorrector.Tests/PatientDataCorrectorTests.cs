using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPONSDataCorrector.Tests
{
    [TestClass]
    public class PatientDataCorrectorTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            PatientDataCorrector patientDataCorrector = new PatientDataCorrector();

            patientDataCorrector.RemoveDuplicates();
        }
    }
}
