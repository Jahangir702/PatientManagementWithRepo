using MasterDetails.Models;
using MasterDetails.Repositories.interfaces;
using MasterDetails.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace MasterDetails.Controllers
{
    public class PatientController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGenericRepo<Patient> repo;

        public PatientController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.repo = this.unitOfWork.GetRepo<Patient>();
        }
        public async Task<ActionResult> Index(int pg = 1)
        {
            var data = await this.repo.GetAllAsync(x => x.Include(y => y.Medicines));
            var pagedData = await data.OrderBy(x => x.PatientId).ToPagedListAsync(pg, 5);
            return View(pagedData);
        }
        public ActionResult Create()
        {
            PatientInputModel db = new PatientInputModel();
            db.Medicines.Add(new Medicine { });
            return View(db);
        }
        [HttpPost]
        public async Task<IActionResult> Create(PatientInputModel model, string act = "")
        {
            if (act == "add")
            {
                model.Medicines.Add(new Medicine { });
            }
            if (act.StartsWith("remove"))
            {
                var index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.Medicines.RemoveAt(index);

                foreach (var v in ModelState.Values)
                {
                    v.RawValue = null;
                    v.Errors.Clear();
                }
            }
            if (act == "insert")
            {
                if (ModelState.IsValid)
                {
                    Patient a = new Patient
                    {
                        PatientName = model.PatientName,
                        Age = model.Age,
                        Gender = model.Gender,
                    };
                    foreach (var l in model.Medicines)
                    {

                        a.Medicines.Add(l);
                    }
                    await this.repo.InsertAsync(a);
                    bool saved = await this.unitOfWork.SaveAsync();
                    if (saved)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to saved data");
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var a = await repo.GetByIdAsync(x => x.PatientId == id, x => x.Include(y => y.Medicines));

            if (a == null)
            {
                return NotFound();
            }

            var model = new PatientEditModel
            {
                PatientId = a.PatientId,
                PatientName = a.PatientName,
                Age = a.Age,
                Gender = a.Gender,
            };

            foreach (var medicine in a.Medicines)
            {
                model.Medicines.Add(new Medicine
                {
                    MedicineId = medicine.MedicineId,
                    MedicineName = medicine.MedicineName,
                    Quantity = medicine.Quantity,
                    Advice = medicine.Advice
                });
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PatientEditModel model, string act = "")
        {
            if (act == "add")
            {
                model.Medicines.Add(new Medicine());
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.Medicines.RemoveAt(index);

                foreach (var v in ModelState.Values)
                {
                    v.RawValue = null;
                    v.Errors.Clear();
                }
            }
            if (act == "update")
            {
                if (ModelState.IsValid)
                {
                    var patientRepo = unitOfWork.GetRepo<Patient>();

                    var patient = await patientRepo.GetByIdAsync(x => x.PatientId == model.PatientId, x => x.Include(y => y.Medicines));

                    if (patient == null)
                    {
                        return NotFound();
                    }

                    patient.PatientName = model.PatientName;
                    patient.Age = model.Age;
                    patient.Gender = model.Gender;


                    var medicineRepo = unitOfWork.GetRepo<Medicine>();

                    patient.Medicines.Clear();
                    // Delete existing qualifications using the DeleteAsync method
                    await medicineRepo.DeleteAsync(q => q.PatientId == model.PatientId);

                    // Add new qualifications
                    foreach (var x in model.Medicines)
                    {
                        patient.Medicines.Add(new Medicine { MedicineName = x.MedicineName, Quantity = x.Quantity, Advice = x.Advice});
                    }

                    await patientRepo.UpdateAsync(patient);
                    bool saved = await unitOfWork.SaveAsync();

                    if (saved)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to save data");
                    }
                }
            }

            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var patientRepo = unitOfWork.GetRepo<Patient>();
            // Delete the applicant
            await patientRepo.DeleteAsync(x => x.PatientId == id);
            bool saved = await unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
