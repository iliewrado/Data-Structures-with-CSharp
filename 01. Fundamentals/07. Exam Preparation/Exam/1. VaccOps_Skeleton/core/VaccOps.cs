using System;
using System.Collections.Generic;
using System.Linq;

namespace VaccTests
{
    public class VaccOps
    {
        private Dictionary<string, Doctor> doctors;
        private List<Patient> patients;

        public VaccOps()
        {
            doctors = new Dictionary<string, Doctor>();
            patients = new List<Patient>();
        }

        public void AddDoctor(Doctor d)
        {
            CheckDoctorExist(d.Name);
            doctors.Add(d.Name, d);
        }

        private void CheckDoctorExist(string name)
        {
            if (doctors.ContainsKey(name))
                throw new ArgumentException();
        }

        private void CheckDoctor(string name)
        {
            if (!doctors.ContainsKey(name))
                throw new ArgumentException();
        }

        public void AddPatient(Doctor d, Patient p)
        {
            CheckDoctor(d.Name);
            doctors[d.Name].Patients.Add(p);
            patients.Add(p);
        }

        public IEnumerable<Doctor> GetDoctors()
        {
            return doctors.Values;
        }

        public IEnumerable<Patient> GetPatients()
        {
            return patients;
        }

        public bool Exist(Doctor d)
        {
            return doctors.ContainsKey(d.Name);
        }

        public bool Exist(Patient p)
        {
            return patients.Contains(p);
        }


        public Doctor RemoveDoctor(string name)
        {
            CheckDoctor(name);
            Doctor doctor = doctors[name];
            doctors.Remove(name);
            return doctor;
        }

        public void ChangeDoctor(Doctor from, Doctor to, Patient p)
        {
            CheckDoctor(from.Name);
            CheckDoctor(to.Name);

            if (!Exist(p))
                throw new ArgumentException();
           
            doctors[from.Name].Patients.Remove(p);
            doctors[to.Name].Patients.Add(p);
        }

        public IEnumerable<Doctor> GetDoctorsByPopularity(int populariry)
        {
            List<Doctor> result = new List<Doctor>();

            foreach (var item in doctors)
            {
                if (item.Value.Popularity == populariry)
                {
                    result.Add(item.Value);
                }
            }

            return result;
        }

        public IEnumerable<Patient> GetPatientsByTown(string town)
        {
            return patients.Where(x => x.Town == town);
        }

        public IEnumerable<Patient> GetPatientsInAgeRange(int lo, int hi)
        {
            List<Patient> result = new List<Patient>();

            foreach (var item in patients)
            {
                if (item.Age >= lo && item.Age <= hi)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public IEnumerable<Doctor> GetDoctorsSortedByPatientsCountDescAndNameAsc()
        {
            return doctors.Values.OrderByDescending(x => x.Patients.Count).ThenBy(x => x.Name);
        }


        public IEnumerable<Patient> GetPatientsSortedByDoctorsPopularityAscThenByHeightDescThenByAge()
        {
            return null;
        }
    }
}
